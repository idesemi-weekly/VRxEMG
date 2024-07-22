using UnityEngine;
using Unity.Netcode;

public class BallController : NetworkBehaviour
{
    private NetworkVariable<Vector3> networkVelocity = new NetworkVariable<Vector3>(writePerm: NetworkVariableWritePermission.Server);
    private NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(writePerm: NetworkVariableWritePermission.Server);

    private float speed = 9f;
    private Rigidbody rb;
    private Vector3 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        startPosition = transform.position;

        if (IsServer)
        {
            Invoke("LaunchBall", 2f);
        }
    }

    void Update()
    {
        if (!IsServer)
        {
            rb.linearVelocity = networkVelocity.Value;
            transform.position = networkPosition.Value;
        }
    }

    private void LaunchBall()
    {
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = 0f;

        while (y >= -0.25f && y <= 0.25f)
        {
            y = Random.Range(-1f, 1f);
        }

        Vector3 initialVelocity = new Vector3(x, y, 0) * speed;
        UpdateVelocityServerRpc(initialVelocity);
    }

    public void ResetPosition()
    {
        if (!IsServer) return;

        rb.linearVelocity = Vector3.zero;
        transform.position = startPosition;
        networkVelocity.Value = Vector3.zero;
        networkPosition.Value = startPosition;
        Invoke("LaunchBall", 2f);
    }

    [ServerRpc]
    private void UpdateVelocityServerRpc(Vector3 newVelocity)
    {
        rb.linearVelocity = newVelocity;
        networkVelocity.Value = newVelocity;
        networkPosition.Value = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        else if (other.gameObject.CompareTag("Goal1"))
        {
            speed = Random.Range(9f, 12f);
            PongGameManager.Instance.UpdateScoreServerRPC(1);
            ResetPosition();
        }
        else if (other.gameObject.CompareTag("Goal2"))
        {
            speed = Random.Range(9f, 12f);
            PongGameManager.Instance.UpdateScoreServerRPC(2);
            ResetPosition();
        }
        else if (other.gameObject.CompareTag("ObstacleCollision"))
        {
            Vector3 newVelocity = new Vector3(rb.linearVelocity.x, -rb.linearVelocity.y, 0);
            UpdateVelocityServerRpc(newVelocity);
        }
        else
        {
            float newSpeed = speed * 1.2f;
            if (newSpeed > 36f)
            {
                newSpeed = 36f;
            }
            Vector3 currentVelocity = rb.linearVelocity;
            Vector3 newVelocity = currentVelocity.normalized * newSpeed;
            newVelocity = new Vector3(-newVelocity.x, newVelocity.y, 0);
            UpdateVelocityServerRpc(newVelocity);
            speed = newSpeed;
        }
    }
}
