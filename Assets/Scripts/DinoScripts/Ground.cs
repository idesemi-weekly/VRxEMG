using UnityEngine;

public class Ground : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {   
        float gameSpeed = GameManager.Instance.speed / transform.localScale.x;
        meshRenderer.material.mainTextureOffset += Vector2.right * gameSpeed * Time.deltaTime;
    }
}
