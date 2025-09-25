using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float acceleration = 15f;
    public Vector2 horizontalRouteLimits = new Vector2(-3f, 3f);
    public Vector2 verticalRouteLimits = new Vector2(-2f, 2f);

    private Rigidbody2D rb;
    private float targetVelocityX;
    private float targetVelocityY;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Пример управления: стрелки/AWSD
        float moveX = Input.GetAxisRaw("Horizontal"); // -1..1
        float moveY = Input.GetAxisRaw("Vertical");   // -1..1

        // Направление в зависимости от ввода
        targetVelocityX = moveX * moveSpeed;
        targetVelocityY = moveY * moveSpeed;

        // Ограничения маршрута
        targetVelocityX = Mathf.Clamp(targetVelocityX, -moveSpeed, moveSpeed);
        targetVelocityY = Mathf.Clamp(targetVelocityY, -moveSpeed, moveSpeed);

        // Ограничить текущую позицию по маршруту
        Vector3 pos = transform.position;
        if (horizontalRouteLimits != Vector2.zero)
            pos.x = Mathf.Clamp(pos.x, horizontalRouteLimits.x, horizontalRouteLimits.y);
        if (verticalRouteLimits != Vector2.zero)
            pos.y = Mathf.Clamp(pos.y, verticalRouteLimits.x, verticalRouteLimits.y);
        transform.position = pos;
    }

    void FixedUpdate()
    {
        Vector2 currentVel = rb.velocity;
        Vector2 targetVel = new Vector2(targetVelocityX, targetVelocityY);

        if (acceleration > 0f)
        {
            currentVel = Vector2.MoveTowards(currentVel, targetVel, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentVel = targetVel;
        }

        rb.velocity = currentVel;
    }
}