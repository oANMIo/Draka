using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;               // игрок
    [SerializeField] private float _smoothSpeed = 0.15f;
    [SerializeField] private Vector3 _offset;                 // смещение камеры
    [SerializeField] private Vector2 _minPos;                 // минимальные координаты (границы уровня)
    [SerializeField] private Vector2 _maxPos;                 // максимальные координаты
    [SerializeField] private float _lookAheadDistance = 2f;   // на сколько сдвигается вперёд
    
    private float _currentLookAhead = 0f;

    private void LateUpdate()
    {
        if (_target == null) 
            return;

        // Определяем направление
        float moveDir = Mathf.Sign(_target.localScale.x);
        _currentLookAhead = Mathf.Lerp(_currentLookAhead, moveDir * _lookAheadDistance, Time.deltaTime * 2f);

        // Позиция камеры с учётом смещения
        Vector3 desiredPosition = _target.position + _offset + new Vector3(_currentLookAhead, 0, 0);

        // Позиция камеры с учётом смещения
        //Vector3 desiredPosition = _target.position + _offset;

        // Ограничение в пределах уровня
        float clampedX = Mathf.Clamp(desiredPosition.x, _minPos.x, _maxPos.x);
        float clampedY = Mathf.Clamp(desiredPosition.y, _minPos.y, _maxPos.y);

        Vector3 clampedPosition = new Vector3(clampedX, clampedY, desiredPosition.z);

        // Плавное движение камеры
        transform.position = Vector3.Lerp(transform.position, clampedPosition, _smoothSpeed);
    }
}
