using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float initialSpeed = 5f;
    public float acceleration = 0.1f;
    public float speed {get; private set;}

    public TextMeshProUGUI youlostTextP1;
    public TextMeshProUGUI youwinTextP1;
    public TextMeshProUGUI youlostTextP2;
    public TextMeshProUGUI youwinTextP2;

    private Player1 player1;
    private Player2 player2;
    private Spawner spawner;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    [System.Obsolete]
    private void Start()
    {
        player1 = FindFirstObjectByType<Player1>();
        player2 = FindFirstObjectByType<Player2>();
        spawner = FindFirstObjectByType<Spawner>();
        
        Game();
    }

    [System.Obsolete]
    private void Game()
    {   
        ObstacleMove[] obstacles = FindObjectsOfType<ObstacleMove>();

        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        speed = initialSpeed;
        enabled = true;

        player1.gameObject.SetActive(true);
        player2.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
        youlostTextP1.gameObject.SetActive(false);
        youwinTextP1.gameObject.SetActive(false);
        youlostTextP2.gameObject.SetActive(false);
        youwinTextP2.gameObject.SetActive(false);
    }

    public void GameOver(int p)
    {
        speed = 0f;
        enabled = false;

        if(p == 1)
        {
            youlostTextP1.gameObject.SetActive(true);
            youwinTextP2.gameObject.SetActive(true);
        }
        else
        {
            youlostTextP2.gameObject.SetActive(true);
            youwinTextP1.gameObject.SetActive(true);
        }

        player1.gameObject.SetActive(false);
        player2.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);
    }

    private void Update()
    {
        speed += acceleration * Time.deltaTime;
    }
}
