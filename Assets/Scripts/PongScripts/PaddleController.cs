using UnityEngine;
using Unity.Netcode;

public class PaddleController : NetworkBehaviour
{
    private float speed = 500f;
    private float minY = -10f;
    private float maxY = 10f;

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
        direction = Vector3.up * speed;
    }

    public void MoveDown()
    {
        direction = Vector3.down * speed;
    }

    [ServerRpc]
    private void MovementServerRpc(Vector3 moveDirection)
    {
        Vector3 newPosition = transform.position + moveDirection * Time.deltaTime;
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        transform.position = newPosition;
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
