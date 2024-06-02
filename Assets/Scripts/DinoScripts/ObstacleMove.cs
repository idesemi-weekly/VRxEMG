using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    private float leftBound;

    private void Start()
    {
        leftBound = Camera.main.ViewportToWorldPoint(Vector3.zero).x - 1f;
    }

    private void Update()
    {
        float gameSpeed = GameManager.Instance.speed;
        transform.position += Vector3.left * gameSpeed * Time.deltaTime;

        if (transform.position.x < leftBound)
        {
            Destroy(gameObject);
        }
    }
}
