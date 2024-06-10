using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
 
public class Start : MonoBehaviour
{
    
    public InputActionReference startGameAction;
    
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

    private void StartGame(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(1);
    }
}