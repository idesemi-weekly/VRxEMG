using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameSpawn : NetworkBehaviour
{

    public static GameSpawn Instance { get; private set; }
    
    private GameObject game;
    private GameObject tutorial;
    private GameObject player1;
    private GameObject player2;

    public GameObject[] games;
    public GameObject[] tutorials;
    public GameObject[] players1;
    public GameObject[] players2;
    private bool p1ready = false;
    private bool p2ready = false;
    private int randomIndex;
    
    [SerializeField] private GameObject check1;
    [SerializeField] private GameObject check2;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject player1Win;
    [SerializeField] private GameObject player2Win;

    [SerializeField] private InputActionReference Player1Ready;
    [SerializeField] private InputActionReference Player2Ready;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void OnEnable()
    {
        Player1Ready.action.Enable();
        Player2Ready.action.Enable();

        // Adding context calls allows us to not use Update() to check for input
        Player1Ready.action.started += OnPlayer1Ready;
        Player2Ready.action.started += OnPlayer2Ready;

        randomIndex = Random.Range(0, games.Length); //choose random game
        Debug.Log("Random index: " + randomIndex);
        //TutorialSpawn(); // Spawn tutorial at the start of the game
    }

    private void OnDisable()
    {
        Player1Ready.action.Disable();
        Player2Ready.action.Disable();

        Player1Ready.action.started -= OnPlayer1Ready;
        Player2Ready.action.started -= OnPlayer2Ready;
    }

    private void TutorialSpawn()
    {
        tutorial = Instantiate(tutorials[randomIndex], Vector3.zero, Quaternion.identity);
        tutorial.GetComponent<NetworkObject>().Spawn();
    }

    private void OnPlayer1Ready(InputAction.CallbackContext ctx)
    {
        if (!p1ready)
        {
            p1ready = true;
            Debug.Log("Player 1 is ready");
            if (check1 != null) check1.SetActive(true);
            CheckPlayersReady();
        }
    }

    private void OnPlayer2Ready(InputAction.CallbackContext ctx)
    {
        if (!p2ready)
        {
            p2ready = true;
            Debug.Log("Player 2 is ready");
            if (check2 != null) check2.SetActive(true);
            CheckPlayersReady();
        }
    }

    private void CheckPlayersReady()
    {
        if (p1ready && p2ready)
        {
            Debug.Log("Both players are ready");
            p1ready = false;
            p2ready = false;
            if (check1 != null) check1.SetActive(false);//remove condition soon
            if (check2 != null) check2.SetActive(false);
            OnDisable(); // Disable input actions after game starts
            SpawnGame();
        }
    }

    private void GameOver(){

        if (HP.Hearts_1 == 0)
        {
            Debug.Log("Game Over for P1");
            if (gameOver != null) gameOver.SetActive(true);
            if (player2Win != null) player2Win.SetActive(true);
            Invoke("EndGame", 5);
        }
        else if (HP.Hearts_2 == 0)
        {
            Debug.Log("Game Over for P2");
            if (gameOver != null) gameOver.SetActive(true);
            if (player1Win != null) player1Win.SetActive(true);
            Invoke("EndGame", 5);
        }

    }


    private void EndGame()
    {
        HP.Hearts_1 = 3;
        HP.Hearts_2 = 3;
        NetworkManager.Singleton.SceneManager.LoadScene("Main Lobby", LoadSceneMode.Single);
    }


    private void SpawnGame()//async void await
    {
        game = Instantiate(games[randomIndex], Vector3.zero, Quaternion.identity);
        game.GetComponent<NetworkObject>().Spawn();
        player1 = Instantiate(players1[randomIndex], players1[randomIndex].transform.position, Quaternion.identity);
        player1.GetComponent<NetworkObject>().Spawn();
        player2 = Instantiate(players2[randomIndex], players2[randomIndex].transform.position, Quaternion.identity);
        player2.GetComponent<NetworkObject>().Spawn();
    }

    public void StartDestroyGame()
    {
        StartCoroutine(DestroyGame());
    }

    public IEnumerator DestroyGame()
    {   
        Destroy(player1);
        Destroy(player2);
        yield return new WaitForSeconds(3);
        Debug.Log("Destroying game");
        Destroy(game);
        

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("ObstacleCollision");
        foreach (GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }

        if (HP.Hearts_1 == 0 || HP.Hearts_2 == 0)
        {
            GameOver();
        }
        else
        {
            OnEnable();
        }
    }
}
