using UnityEngine;

public class AnimatedSprite : MonoBehaviour
{
    public Sprite[] sprites;

    private SpriteRenderer spriteRenderer;
    private int frameIndex;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Invoke(nameof(Animate), 0f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Animate()
    {
        frameIndex = (frameIndex + 1) % sprites.Length;
        spriteRenderer.sprite = sprites[frameIndex];

        Invoke(nameof(Animate), 1f / GameManager.Instance.speed);
    }
}
