using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Collections.Generic;

public class NetworkConnect : MonoBehaviour
{
    public int maxConnection = 3;
    public UnityTransport transport;

    private Lobby currentLobby;
    private float heartBeatTimer;
    
    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        JoinOrCreate();
    }

    public async void JoinOrCreate()
    {
        try
        {
            currentLobby = await Lobbies.Instance.QuickJoinLobbyAsync(); //to change if multiple lobbies
            string relayJoinCode = currentLobby.Data["JoinCode"].Value;

            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(relayJoinCode);

            transport.SetClientRelayData(allocation.RelayServer.IpV4,(ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData, allocation.HostConnectionData);
        
            NetworkManager.Singleton.StartClient();
        }
        catch{
            CreateServer();
        }
    }

    public async void CreateServer()
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnection);
        string newJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        transport.SetHostRelayData(allocation.RelayServer.IpV4,(ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);

        CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();
        lobbyOptions.IsPrivate = false;
        lobbyOptions.Data = new Dictionary<string, DataObject>();
        DataObject dataObject = new DataObject(DataObject.VisibilityOptions.Public, newJoinCode);
        lobbyOptions.Data.Add("JoinCode", dataObject);

        currentLobby = await Lobbies.Instance.CreateLobbyAsync("MyLobby", maxConnection, lobbyOptions);

        NetworkManager.Singleton.StartHost();
    }

    public async void JoinServer()
    {
        currentLobby = await Lobbies.Instance.QuickJoinLobbyAsync(); //to change if multiple lobbies
        string relayJoinCode = currentLobby.Data["JoinCode"].Value;

        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(relayJoinCode);

        transport.SetClientRelayData(allocation.RelayServer.IpV4,(ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData, allocation.HostConnectionData);
        
        NetworkManager.Singleton.StartClient();
    }

    private void Update(){
        if(heartBeatTimer > 15){
            heartBeatTimer -= 15;

            if(currentLobby != null && currentLobby.HostId == AuthenticationService.Instance.PlayerId){
                LobbyService.Instance.SendHeartbeatPingAsync(currentLobby.Id);
            }
        }

        heartBeatTimer += Time.deltaTime;
           
        
    }
}
