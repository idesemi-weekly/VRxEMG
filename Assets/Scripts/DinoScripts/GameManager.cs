using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float initialSpeed = 5f;
    public float acceleration = 0.1f;
    public float speed {get; private set;}

    public TextMeshProUGUI youlostText;
    public TextMeshProUGUI youwinText;

    private Player player;
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
        player = FindFirstObjectByType<Player>();
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

        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
        youlostText.gameObject.SetActive(false);
        youwinText.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        speed = 0f;
        enabled = false;

        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);
        youlostText.gameObject.SetActive(true);
        //youwinText.gameObject.SetActive(true); NEED CONDITION/RPC FOR THAT
    }

    private void Update()
    {
        speed += acceleration * Time.deltaTime;
    }
}
