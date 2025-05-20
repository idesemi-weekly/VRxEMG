using UnityEngine;
using Unity.Netcode;

public class ClientSpawn: NetworkBehaviour
{
    public GameObject playerPrefab;
    public Transform spawnPoint;

    public override void OnNetworkSpawn()
    {
        if (IsClient && !IsHost)
        {
            RequestSpawnPlayerAtSetPointServerRpc(NetworkManager.Singleton.LocalClientId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestSpawnPlayerAtSetPointServerRpc(ulong clientId)
    {
        GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();
        networkObject.SpawnAsPlayerObject(clientId);
    }
}