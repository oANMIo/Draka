using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private float _smoothSpeed = 5f;

    private float _targetValue;

    private void Start()
    {
        _targetValue = _slider.value;
    }

    private void Update()
    {
        // ѕлавное приближение текущего значени€ к целевому
        _slider.value = Mathf.Lerp(_slider.value, _targetValue, Time.deltaTime * _smoothSpeed);
    }

    public void SetMaxHealth(float maxHealth)
    {
        _slider.maxValue = maxHealth;
        _slider.value = maxHealth;
        _targetValue = maxHealth;
    }

    public void SetHealth(float health)
    {
        _targetValue = health;
    }
}
