using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PongGameManager : NetworkBehaviour
{
    public static PongGameManager Instance { get; private set; }

    private bool game = true;

    private GameObject paddle1;
    private GameObject paddle2;
    [SerializeField] private GameObject ball;
    [SerializeField] private Text player1ScoreText;
    [SerializeField] private Text player2ScoreText;
    [SerializeField] private GameObject player1WinText;
    [SerializeField] private GameObject player2WinText;

    private int player1Score = 0;
    private int player2Score = 0;

    [SerializeField] private InputActionReference Player1Up;
    [SerializeField] private InputActionReference Player1Down;
    [SerializeField] private InputActionReference Player2Up;
    [SerializeField] private InputActionReference Player2Down;

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
        Player1Up.action.Enable();
        Player1Down.action.Enable();
        Player2Up.action.Enable();
        Player2Down.action.Enable();

        Player1Up.action.started += OnPlayer1MoveUp;
        Player1Down.action.started += OnPlayer1MoveDown;
        Player2Up.action.started += OnPlayer2MoveUp;
        Player2Down.action.started += OnPlayer2MoveDown;
    }

    private void OnDisable()
    {
        Player1Up.action.Disable();
        Player1Down.action.Disable();
        Player2Up.action.Disable();
        Player2Down.action.Disable();

        Player1Up.action.started -= OnPlayer1MoveUp;
        Player1Down.action.started -= OnPlayer1MoveDown;
        Player2Up.action.started -= OnPlayer2MoveUp;
        Player2Down.action.started -= OnPlayer2MoveDown;
    }

    private void OnPlayer1MoveUp(InputAction.CallbackContext ctx)
    {
        paddle1.GetComponent<PaddleController>().MoveUp();
    }

    private void OnPlayer1MoveDown(InputAction.CallbackContext ctx)
    {
        paddle1.GetComponent<PaddleController>().MoveDown();
    }

    private void OnPlayer2MoveUp(InputAction.CallbackContext ctx)
    {
        paddle2.GetComponent<PaddleController>().MoveUp();
    }

    private void OnPlayer2MoveDown(InputAction.CallbackContext ctx)
    {
        paddle2.GetComponent<PaddleController>().MoveDown();
    }

    public void UpdateScore(int player)
    {
        if (player == 1)
        {
            player1Score++;
            player1ScoreText.text = player1Score.ToString();
            
            if (player1Score >= 3)
            {
                HP.Hearts_2--;
                game = false;
                ball.SetActive(false);
                player1WinText.SetActive(true);
                GameSpawn.Instance.StartDestroyGame();
            }
        }
        else
        {
            player2Score++;
            player2ScoreText.text = player2Score.ToString();

            if (player2Score >= 3)
            {
                HP.Hearts_1--;
                game = false;
                ball.SetActive(false);
                player2WinText.SetActive(true);
                GameSpawn.Instance.StartDestroyGame();
            }
        }

        if (game == true)
        {
            ResetBall();
        }
    }

    private void ResetBall()
    {
        ball.GetComponent<BallController>().ResetPosition();
    }

    public void SetPaddles(GameObject p1, GameObject p2)
    {
        paddle1 = p1;
        paddle2 = p2;
    }
}
