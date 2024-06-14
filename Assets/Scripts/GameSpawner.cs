using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameSpawner : NetworkBehaviour//might need to change this to NetworkBehaviour
{   

    public GameObject[] games;
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
            check1.SetActive(true);
        }
    }

    private void OnPlayer2Ready()
    {
        if (!p2ready)
        {
            p2ready = true;
            Debug.Log("Player 2 is ready");
            check2.SetActive(true);
        }
    }

    private void Update()//this is not necessary, calling a function in onplayer1ready and onplayer2ready is better but not working???
    {
        if (p1ready && p2ready)
        {
            Debug.Log("Both players are ready");
            SpawnGame();
            p1ready = false;
            p2ready = false;
            check1.SetActive(false);
            check2.SetActive(false);
            OnDisable();
        }
    }

    public void SpawnGame()
    {
        int randomIndex = Random.Range(0, games.Length);
        GameObject game = Instantiate(games[randomIndex], Vector3.zero, Quaternion.identity);
        game.GetComponent<NetworkObject>().Spawn();
    }
    
    //if gameover remove game (i know the index of the game i want to remove which is randomIndex)
    public void DestroyGame(GameObject game)
    {
        Destroy(game);
        OnEnable();
    }
}
