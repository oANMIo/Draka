using UnityEngine;

public class ParabolicThrow : MonoBehaviour
{
    private Vector2 _startPos;
    private Vector2 _targetPos;
    private float _duration;
    private float _height;
    private float _time;

    public void StartThrow(Vector2 start, Transform target, float dur, float h)
    {
        _startPos = start;
        _targetPos = target.position; // фиксируем позицию игрока в момент броска
        _duration = dur;
        _height = h;
        _time = 0;
    }

    private void Update()
    {
        if (_duration <= 0) return;

        _time += Time.deltaTime / _duration;

        if (_time > 1f)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 linearPos = Vector2.Lerp(_startPos, _targetPos, _time);
        float parabola = 4 * _height * _time * (1 - _time);

        transform.position = new Vector2(linearPos.x, linearPos.y + parabola);
    }
}
