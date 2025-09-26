using UnityEngine;

public class mov : MonoBehaviour
{
    public float speedHorizontal = 5f;
    public float speedVertical = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is required on this GameObject.");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer component not found; sprite flipping won't work.");
        }
    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(
            moveX * speedHorizontal,
            moveY * speedVertical
        );

        rb.linearVelocity = movement;

        if (spriteRenderer != null)
        {
            if (moveX > 0) spriteRenderer.flipX = false;
            else if (moveX < 0) spriteRenderer.flipX = true;
            // ��� moveX == 0 ������ ��������� ������� �������
        }
    }
}