using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerReadyCheck : NetworkBehaviour
{
    private NetworkVariable<bool> p1ready = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> p2ready = new NetworkVariable<bool>(false);

    [SerializeField] private GameObject check1;
    [SerializeField] private GameObject check2;
    
    [SerializeField] private InputActionReference Player1Ready;
    [SerializeField] private InputActionReference Player2Ready;

    private void OnEnable()
    {
        Player1Ready.action.Enable();
        Player2Ready.action.Enable();

        // Adding context calls allows us to not use Update() to check for input
        Player1Ready.action.started += ctx => OnPlayer1Ready();
        Player2Ready.action.started += ctx => OnPlayer2Ready();
    }

    private void OnDisable()
    {
        Player1Ready.action.Disable();
        Player2Ready.action.Disable();

        Player1Ready.action.started -= ctx => OnPlayer1Ready();
        Player2Ready.action.started -= ctx => OnPlayer2Ready();
    }

    private void Start()
    {
        p1ready.OnValueChanged += OnP1ReadyChanged;
        p2ready.OnValueChanged += OnP2ReadyChanged;
    }

    private void OnDestroy()
    {
        p1ready.OnValueChanged -= OnP1ReadyChanged;
        p2ready.OnValueChanged -= OnP2ReadyChanged;
    }

    private void OnPlayer1Ready()
    {
        if (!p1ready.Value && IsServer) // Check if the server and player is not ready
        {
            p1ready.Value = true;
            Debug.Log("Player 1 is ready");
            if (check1 != null) check1.SetActive(true);
            CheckPlayersReady();
        }
    }

    private void OnPlayer2Ready()
    {
        if (!p2ready.Value && IsServer) // Check if the server and player is not ready
        {
            p2ready.Value = true;
            Debug.Log("Player 2 is ready");
            if (check2 != null) check2.SetActive(true);
            CheckPlayersReady();
        }
    }

    private void OnP1ReadyChanged(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            check1.SetActive(true);
        }
    }

    private void OnP2ReadyChanged(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            check2.SetActive(true);
        }
    }

    private void CheckPlayersReady()
    {
        if (p1ready.Value && p2ready.Value)
        {
            Debug.Log("Both players are ready");
            Invoke(nameof(StartGame), 3f);
        }
    }

    private void StartGame()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
