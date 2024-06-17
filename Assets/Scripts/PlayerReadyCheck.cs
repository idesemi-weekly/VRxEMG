using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerReadyCheck : NetworkBehaviour
{
    private bool p1ready = false;
    private bool p2ready = false;

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

    private void OnPlayer1Ready()
    {
        if (!p1ready) //no need to servercheck since this is going to be on serveronly component
        {
            p1ready = true;
            Debug.Log("Player 1 is ready");
            if (check1 != null) check1.SetActive(false);
            CheckPlayersReady();
        }
    }

    private void OnPlayer2Ready()
    {
        if (!p2ready)
        {
            p2ready = true;
            Debug.Log("Player 2 is ready");
            if (check2 != null) check2.SetActive(false);
            CheckPlayersReady();
        }
    }

    private void CheckPlayersReady()//this is not necessary, calling a function in onplayer1ready and onplayer2ready is better but not working???
    {
        if (p1ready && p2ready)
        {
            Debug.Log("Both players are ready");
            Invoke(nameof(StartGame), 5f);
            //add object
        }
    }

    private void StartGame()
    {
        // This will load the scene for all clients
        NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    /*
    //not sure if this is necessary, since if we want to play again it might cause a conflict
    private void OnDestroy()
    {
        Player1Ready.action.Dispose();
        Player2Ready.action.Dispose();
    }*/
}
