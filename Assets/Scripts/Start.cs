using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
 
public class Start : MonoBehaviour
{

    public InputActionReference startGameAction;
    //make a code that will allow the player to start the game by pressing a controller button
    private void OnEnable()
    {
        startGameAction.action.Enable();
        startGameAction.action.performed += StartGame;
    }

    private void OnDisable()
    {
        startGameAction.action.Disable();
        startGameAction.action.performed -= StartGame;
    }

    private void StartGame(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(1);
    }
}