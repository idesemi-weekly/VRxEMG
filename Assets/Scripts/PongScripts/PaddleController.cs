using UnityEngine;
using Unity.Netcode;

public class PaddleController : NetworkBehaviour
{
    public float speed = 500f;

    private Vector3 direction;

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }

        if (direction != Vector3.zero)
        {
            MovementServerRpc(direction);
            direction = Vector3.zero;
        }
    }

    public void MoveUp()
    {
        Debug.Log("MoveUp");
        direction = Vector3.up * speed;//change this
    }

    public void MoveDown()
    {
        Debug.Log("MoveDown");
        direction = Vector3.down * speed;
    }

    [ServerRpc]
    private void MovementServerRpc(Vector3 moveDirection)
    {
        transform.Translate(moveDirection * Time.deltaTime);
        UpdateClientRpc(transform.position);
    }

    [ClientRpc]
    private void UpdateClientRpc(Vector3 serverPosition)
    {
        if (!IsServer)
        {
            transform.position = serverPosition;
        }
    }
}
