using UnityEngine;

public class WalkSwayAnimation : MonoBehaviour
{
    public float rotationAngle = 15f;       // максимальный угол качани€ (в градусах)
    public float swaySpeed = 5f;             // скорость качани€ (частота)
    public float lerpSpeed = 5f;             // скорость возвращени€ в нейтральное положение
    public float movementThreshold = 0.01f;  // минимальный порог движени€ по X дл€ активации качани€

    private float baseZRotation;
    private Rigidbody2D rb2d;
    private float swayTime = 0f;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        baseZRotation = transform.eulerAngles.z;
    }

    private void Update()
    {
        float horizontalSpeed = 0f;

        if (rb2d != null)
            horizontalSpeed = rb2d.linearVelocity.x;
        else
            horizontalSpeed = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(horizontalSpeed) > movementThreshold)
        {
            // ƒвигаемс€ Ч увеличиваем врем€ качани€
            swayTime += Time.deltaTime * swaySpeed;

            // –ассчитываем цикличный угол качани€: синусоидально между -rotationAngle и rotationAngle
            float swayAngle = Mathf.Sin(swayTime) * rotationAngle;

            // ”станавливаем поворот объекта с учЄтом базового угла
            transform.rotation = Quaternion.Euler(0f, 0f, baseZRotation + swayAngle);
        }
        else
        {
            // Ќет движени€ Ч сбрасываем swayTime и плавно возвращаемс€ в нейтральное положение
            swayTime = 0f;

            float currentZ = Mathf.LerpAngle(transform.eulerAngles.z, baseZRotation, Time.deltaTime * lerpSpeed);
            transform.rotation = Quaternion.Euler(0f, 0f, currentZ);
        }
    }
}