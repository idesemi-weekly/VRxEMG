using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using System.Collections;

public class ShyGuyGameManager : NetworkBehaviour
{
    public static ShyGuyGameManager Instance { get; private set; }

    private bool game = false;

    private float flagDelay = 3.5f;

    [SerializeField] private InputActionReference player1Left;
    [SerializeField] private InputActionReference player1Right;
    [SerializeField] private InputActionReference player2Left;
    [SerializeField] private InputActionReference player2Right;

    [SerializeField] private Animator shyGuyAnimator;
    [SerializeField] private GameObject player1WinText;
    [SerializeField] private GameObject player2WinText;

    private string raisedFlag;

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

    private void OnEnable()
    {
        player1Left.action.Enable();
        player1Right.action.Enable();
        player2Left.action.Enable();
        player2Right.action.Enable();
    }

    private void OnDisable()
    {
        player1Left.action.Disable();
        player1Right.action.Disable();
        player2Left.action.Disable();
        player2Right.action.Disable();
    }

    private void Start()
    {
        player1WinText.SetActive(false);
        player2WinText.SetActive(false);
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {   
        if(IsServer){
            yield return new WaitForSeconds(1f);
            game = true;

            while (game)
            {
                raisedFlag = GenerateCommand();
                UpdateShyGuyAnimationClientRPC(raisedFlag);

                //for loop here with yield return
                yield return new WaitForSeconds(flagDelay);

                bool player1Correct = CheckPlayerResponse(1);
                bool player2Correct = CheckPlayerResponse(2);

                if (!player1Correct && !player2Correct)
                {
                    Debug.Log("Both players failed to respond");
                }
                else if (!player1Correct)
                {
                    GameOverClientRPC(1);
                }
                else if (!player2Correct)
                {
                    GameOverClientRPC(2);
                }

                if (flagDelay > 1.5f) flagDelay -= 0.25f;

                UpdateShyGuyAnimationClientRPC("Idle");
                yield return new WaitForSeconds(flagDelay / 2);
            }
        }
        
    }

    [ClientRpc]
    private void UpdateShyGuyAnimationClientRPC(string command)
    {
        UpdateShyGuyAnimation(command);
    }

    [ClientRpc]
    private void GameOverClientRPC(int playerNumber)
    {
        GameOver(playerNumber);
    }

    private string GenerateCommand()
    {
        string[] commands = { "Raise Red Flag", "Raise White Flag" };
        int index = Random.Range(0, commands.Length);
        return commands[index];
    }

    private void UpdateShyGuyAnimation(string command)
    {
        if (command == "Raise Red Flag")
        {
            shyGuyAnimator.SetTrigger("RaiseRedFlag");
        }
        else if (command == "Raise White Flag")
        {
            shyGuyAnimator.SetTrigger("RaiseWhiteFlag");
        }
        else
        {
            shyGuyAnimator.SetTrigger("Idle");
        }
    }

    private bool CheckPlayerResponse(int playerNumber)
    {
        if (playerNumber == 1)
        {
            if (raisedFlag == "Raise Red Flag" && player1Left.action.IsPressed() && !player1Right.action.IsPressed())
                return true;
            if (raisedFlag == "Raise White Flag" && player1Right.action.IsPressed() && !player1Left.action.IsPressed())
                return true;
        }
        else if (playerNumber == 2)
        {
            if (raisedFlag == "Raise Red Flag" && player2Left.action.IsPressed() && !player2Right.action.IsPressed())
                return true;
            if (raisedFlag == "Raise White Flag" && player2Right.action.IsPressed() && !player2Left.action.IsPressed())
                return true;
        }

        return false;
    }

    public void GameOver(int playerNumber)
    {
        game = false;

        if (playerNumber == 1)
        {
            player2WinText.SetActive(true);
            HP.Hearts_1--;
        }
        else
        {
            player1WinText.SetActive(true);
            HP.Hearts_2--;
        }

        GameSpawn.Instance.StartDestroyGame();
    }
}
