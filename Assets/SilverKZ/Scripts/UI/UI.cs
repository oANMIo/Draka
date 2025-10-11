using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _textHealth;
    [SerializeField] private Image _imageBottle;
    [SerializeField] private TMPro.TextMeshProUGUI _textBottle;
    [SerializeField] private Image _imageKnife;
    [SerializeField] private TMPro.TextMeshProUGUI _textKnife;
    [SerializeField] private Image _arrow;
    [SerializeField] private Slider _slider;
    [SerializeField] private float _smoothSpeed = 5f;

    private float _targetValue;
    private float _maxValue;
    private Coroutine _showHealthRoutine;

    private void OnEnable()
    {
        PlayerThrowableAttack.onAddBottle += SetAmmountBottle;
        PlayerThrowableAttack.onAddKnife += SetAmmountKnife;
        PlayerThrowableAttack.onActiveWeapon += SetActiveWeapon;
        Player.onAddHealth += SetAmmountHealth;
        EnemySpawn.onStartSpawn += HideArrow;
        EnemySpawn.onAllKill += ShowArrow;
    }

    private void OnDisable()
    { 
        PlayerThrowableAttack.onAddBottle -= SetAmmountBottle;
        PlayerThrowableAttack.onAddKnife -= SetAmmountKnife;
        PlayerThrowableAttack.onActiveWeapon -= SetActiveWeapon;
        Player.onAddHealth -= SetAmmountHealth;
        EnemySpawn.onStartSpawn -= HideArrow;
        EnemySpawn.onAllKill -= ShowArrow;
    }

    private void Start()
    {
        _maxValue = 100f;
        _slider.maxValue = _maxValue;
        _slider.value = _maxValue;
    }
    /*
    private void Update()
    {
        _slider.value = Mathf.Lerp(_slider.value, _targetValue, Time.deltaTime * _smoothSpeed);
    }
    */
    private void SetAmmountBottle(int amount)
    {
        _textBottle.text = amount.ToString();
    }

    private void SetAmmountKnife(int amount)
    {
        _textKnife.text = amount.ToString();
    }

    private void SetActiveWeapon(WeaponType weaponType)
    {
        if (weaponType == WeaponType.Bottle)
        {
            _textBottle.enabled = true;
            _textKnife.enabled = false;

            _imageBottle.enabled = true;
            _imageKnife.enabled = false;
        }
        else if (weaponType == WeaponType.Knife)
        {
            _textBottle.enabled = false;
            _textKnife.enabled = true;

            _imageBottle.enabled = false;
            _imageKnife.enabled = true;
        }
    }

    private void SetAmmountHealth(int amount)
    {
        if (_showHealthRoutine != null)
            StopCoroutine(_showHealthRoutine);

        _showHealthRoutine = StartCoroutine(ShowHealth(amount));

        _textHealth.text = "HP " + amount.ToString();
    }

    private void ShowArrow()
    {
        _arrow.enabled = true;
    }

    private void HideArrow()
    {
        _arrow.enabled = false;
    }

    private IEnumerator ShowHealth(float targetHealth)
    {
        float recoveryRate = 20f;

        while (_slider.value != targetHealth)
        {
            _slider.value = Mathf.MoveTowards(_slider.value, targetHealth, recoveryRate * Time.deltaTime);
            yield return null;
        }
    }
}
