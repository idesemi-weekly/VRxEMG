using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb;
    private Vector3 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        startPosition = transform.position;
        LaunchBall();
    }

    private void LaunchBall()
    {
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(0, 2) == 0 ? -1 : 1;
        rb.linearVelocity = new Vector3(x, y, 0) * speed;
    }

    public void ResetPosition()
    {
        rb.linearVelocity = Vector3.zero;
        transform.position = startPosition;
        LaunchBall();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal1"))
        {
            speed = 5f;
            PongGameManager.Instance.UpdateScore(1);
        }
        else if (other.gameObject.CompareTag("Goal2"))
        {
            speed = 5f;
            PongGameManager.Instance.UpdateScore(2);
        }
        else if (other.gameObject.CompareTag("ObstacleCollision"))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -rb.linearVelocity.y, 0);
        }
        else{
            rb.linearVelocity = new Vector3(-rb.linearVelocity.x, rb.linearVelocity.y, 0);
            speed += 0.5f;
        }
    }
}
