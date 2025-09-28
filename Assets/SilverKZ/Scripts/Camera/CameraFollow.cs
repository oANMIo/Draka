using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _smoothSpeed = 0.15f;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector2 _minPos; 
    [SerializeField] private Vector2 _maxPos;
    [SerializeField] private float _lookAheadDistance = 2f;
    
    private float _currentLookAhead = 0f;

    private void LateUpdate()
    {
        if (_target == null) 
            return;

        float moveDir = Mathf.Sign(_target.localScale.x);
        _currentLookAhead = Mathf.Lerp(_currentLookAhead, moveDir * _lookAheadDistance, Time.deltaTime * 2f);

        Vector3 desiredPosition = _target.position + _offset + new Vector3(_currentLookAhead, 0, 0);

        float clampedX = Mathf.Clamp(desiredPosition.x, _minPos.x, _maxPos.x);
        float clampedY = Mathf.Clamp(desiredPosition.y, _minPos.y, _maxPos.y);

        Vector3 clampedPosition = new Vector3(clampedX, clampedY, desiredPosition.z);

        transform.position = Vector3.Lerp(transform.position, clampedPosition, _smoothSpeed);
    }
}
