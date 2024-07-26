using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class EmgData
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("data")]
    public List<double> Data { get; set; }
}

public class EMG : MonoBehaviour
{
    private static EMG instance;
    public static EMG Instance => instance;

    private const int ListenPort = 12345;
    private UdpClient udpServer;
    private CancellationTokenSource cancellationTokenSource;
    private Gamepad virtualGamepad;
    private EmgData currentEmgData;
    private bool newEmgDataReceived;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    async void Start()
    {
        cancellationTokenSource = new CancellationTokenSource();
        udpServer = new UdpClient(ListenPort);
        virtualGamepad = InputSystem.AddDevice<Gamepad>();
        Task.Run(() => ListenForData(cancellationTokenSource.Token));
        await Task.Run(() => ProcessDataEveryInterval(cancellationTokenSource.Token));
    }

    private async Task ListenForData(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            UdpReceiveResult receiveResult = await udpServer.ReceiveAsync();
            string receivedData = Encoding.UTF8.GetString(receiveResult.Buffer);

            EmgData emgData;
            try
            {
                emgData = JsonConvert.DeserializeObject<EmgData>(receivedData);
            }
            catch (JsonException)
            {
                continue;
            }

            if (emgData != null && emgData.Type == "emg" && emgData.Data != null)
            {
                currentEmgData = emgData;
                newEmgDataReceived = true;
            }
        }
    }

    private async Task ProcessDataEveryInterval(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(100, cancellationToken);
            if (newEmgDataReceived)
            {
                ProcessCurrentEmgData();
                newEmgDataReceived = false;
            }
        }
    }

    private void ProcessCurrentEmgData()
    {
        if (currentEmgData == null) return;
        for (int i = 0; i < currentEmgData.Data.Count; i++)
        {
            if (currentEmgData.Data[i] >= 0.9)
            {
                SimulateButtonPress(i);
            }
        }
    }

    private void SimulateButtonPress(int channel)
    {
        if (virtualGamepad == null) return;

        GamepadState gamepadState = new GamepadState();
        switch (channel)
        {
            case 0:
                gamepadState = gamepadState.WithButton(GamepadButton.South, true);
                break;
            case 1:
                gamepadState = gamepadState.WithButton(GamepadButton.North, true);
                break;
            case 2:
                gamepadState = gamepadState.WithButton(GamepadButton.East, true);
                break;
            case 3:
                gamepadState = gamepadState.WithButton(GamepadButton.West, true);
                break;
            default:
                return;
        }

        InputSystem.QueueStateEvent(virtualGamepad, gamepadState);

        gamepadState = new GamepadState();
        InputSystem.QueueStateEvent(virtualGamepad, gamepadState);
    }

    private void OnDestroy()
    {
        Cleanup();
    }

    private void OnApplicationQuit()
    {
        Cleanup();
    }

    private void Cleanup()
    {
        cancellationTokenSource?.Cancel();
        udpServer?.Close();
        InputSystem.RemoveDevice(virtualGamepad);
    }
}
