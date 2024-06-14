using UnityEngine;
using TMPro;
using Unity.Netcode;
using System.Collections;

public class GameManager : NetworkBehaviour
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
    private Spawner1 spawner1;
    private Spawner2 spawner2;

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

    public void OnDestroy()
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
        spawner1 = FindFirstObjectByType<Spawner1>();
        spawner2 = FindFirstObjectByType<Spawner2>();
        
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
        spawner1.gameObject.SetActive(true);
        spawner2.gameObject.SetActive(true);
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
            player1.gameObject.SetActive(false);
            HP.Hearts_1--;
        }
        else
        {
            player2.gameObject.SetActive(false);
            youlostTextP2.gameObject.SetActive(true);
            youwinTextP1.gameObject.SetActive(true);
            HP.Hearts_2--;
        }
    
        spawner1.gameObject.SetActive(false);
        spawner2.gameObject.SetActive(false);
        //wait 3 seconds and remove the game on the screen
        GameSpawn.Instance.DestroyGame();
    }

    private void Update()
    {
        speed += acceleration * Time.deltaTime;
    }
}
