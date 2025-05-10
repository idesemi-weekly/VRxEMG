using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class Player1 : NetworkBehaviour
{
    private CharacterController character;
    private Vector3 velocity;
    public InputActionReference jumpAction;

    public float gravity = 19.62f;
    public float jumpForce = 8f;

    // Add a NetworkVariable for position
    private NetworkVariable<Vector3> netPosition = new NetworkVariable<Vector3>();

    private void Awake()
    {
        character = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        velocity = Vector3.zero;
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        jumpAction.action.Disable();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            netPosition.Value = transform.position;
        }
    }

    private void Update()
    {
        if (IsServer)
        {
            HandleMovement();
        }
        else
        {
            // Clients update their position based on the NetworkVariable
            transform.position = netPosition.Value;
        }
    }

    private void HandleMovement()
    {
        velocity += Vector3.down * gravity * Time.deltaTime;

        if (character.isGrounded && jumpAction.action.triggered)
        {
            velocity = Vector3.up * jumpForce;
        }

        character.Move(velocity * Time.deltaTime);

        // Update the NetworkVariable with the new position
        netPosition.Value = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ObstacleCollision"))
        {
            GameManager.Instance.GameOver(1);
        }
    }
}