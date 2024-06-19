using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

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
    private NetworkVariable<bool> p1ready = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> p2ready = new NetworkVariable<bool>(false);
    private int randomIndex;
    
    [SerializeField] private GameObject P1HUD;
    [SerializeField] private GameObject P2HUD;
    [SerializeField] private GameObject check1;
    [SerializeField] private GameObject check2;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject player1Win;
    [SerializeField] private GameObject player2Win;

    [SerializeField] private InputActionReference Player1Ready;
    [SerializeField] private InputActionReference Player2Ready;

    [SerializeField] private TMP_Text P1HP_text;
    [SerializeField] private TMP_Text P2HP_text;

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

        Player1Ready.action.started += OnPlayer1Ready;
        Player2Ready.action.started += OnPlayer2Ready;

        randomIndex = Random.Range(0, games.Length); //choose random game
        Debug.Log("Game n°" + randomIndex);
    }

    private void OnDisable()
    {
        Player1Ready.action.Disable();
        Player2Ready.action.Disable();

        Player1Ready.action.started -= OnPlayer1Ready;
        Player2Ready.action.started -= OnPlayer2Ready;
    }

    private void Start()
    {
        p1ready.OnValueChanged += OnP1ReadyChanged;
        p2ready.OnValueChanged += OnP2ReadyChanged;

        P1HP_text.text = HP.Hearts_1 + " HP";
        P2HP_text.text = HP.Hearts_2 + " HP";
    }

    private void OnDestroy()
    {
        p1ready.OnValueChanged -= OnP1ReadyChanged;
        p2ready.OnValueChanged -= OnP2ReadyChanged;
    }

    private void OnPlayer1Ready(InputAction.CallbackContext ctx)
    {
        if (!p1ready.Value && IsServer)
        {
            p1ready.Value = true;
            Debug.Log("Player 1 is ready");
            check1.SetActive(true);
            UpdateCheckOnClientRPC(1, true);
            CheckPlayersReady();
        }
    }

    private void OnPlayer2Ready(InputAction.CallbackContext ctx)
    {
        if (!p2ready.Value && IsServer)
        {
            p2ready.Value = true;
            Debug.Log("Player 2 is ready");
            check2.SetActive(true);
            UpdateCheckOnClientRPC(2, true);
            CheckPlayersReady();
        }
    }

    private void OnP1ReadyChanged(bool oldValue, bool newValue)
    {
        UpdateCheckOnClientRPC(1, newValue);
    }

    private void OnP2ReadyChanged(bool oldValue, bool newValue)
    {
        UpdateCheckOnClientRPC(2, newValue);
    }

    [ClientRpc]
    private void UpdateCheckOnClientRPC(int player, bool ready)
    {
        if (player == 1)
        {
            check1.SetActive(ready);
        }
        else if (player == 2)
        {
            check2.SetActive(ready);
        }
    }

    private void CheckPlayersReady()
    {
        if (p1ready.Value && p2ready.Value)
        {
            Debug.Log("Both players are ready");
            p1ready.Value = false;
            p2ready.Value = false;
            check1.SetActive(false);
            check2.SetActive(false);
            P1HUD.SetActive(false);
            P2HUD.SetActive(false);
            UpdateHUDOnClientRPC(false);
            OnDisable(); // Disable input actions after game starts  
            SpawnGame();
        }
    }

    [ClientRpc]
    private void UpdateHUDOnClientRPC(bool active)
    {
        P1HUD.SetActive(active);
        P2HUD.SetActive(active);
    }

    private void GameOver()
    {
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

    private void SpawnGame()
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
        P1HP_text.text = HP.Hearts_1 + " HP";
        P2HP_text.text = HP.Hearts_2 + " HP";
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
            P1HUD.SetActive(true);
            P2HUD.SetActive(true);
            UpdateHUDOnClientRPC(true);
            OnEnable();
        }
    }
}
