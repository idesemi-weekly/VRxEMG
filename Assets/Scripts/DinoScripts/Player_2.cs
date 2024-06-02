using UnityEngine;
using UnityEngine.InputSystem;

public class Player2 : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;
    public InputActionReference jumpAction;
    

    public float gravity = 19.62f;
    public float jumpForce = 8f;

    private void Awake()
    {
        character = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        direction = Vector3.zero;
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        jumpAction.action.Disable();
    }

    private void Update()
    {
        direction += Vector3.down * gravity * Time.deltaTime;

        if (character.isGrounded)
        {
            direction = Vector3.down;

            if (jumpAction.action.triggered) // Use EMGS INSTEAD
            {
                direction = Vector3.up * jumpForce;
            }
        }

        character.Move(direction * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ObstacleCollision"))
        {
            GameManager.Instance.GameOver(2);
        }
    }
}
