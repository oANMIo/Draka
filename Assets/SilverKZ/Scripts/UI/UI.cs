using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Image _imageBottle;
    [SerializeField] private TMPro.TextMeshProUGUI _textBottle;
    [SerializeField] private Image _imageKnife;
    [SerializeField] private TMPro.TextMeshProUGUI _textKnife;
    [SerializeField] private TMPro.TextMeshProUGUI _textEnemyCount;
    [SerializeField] private Image _arrow;
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _death;

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
        EnemySpawn.onEnemyCount += EnemyCount;
    }

    private void OnDisable()
    { 
        PlayerThrowableAttack.onAddBottle -= SetAmmountBottle;
        PlayerThrowableAttack.onAddKnife -= SetAmmountKnife;
        PlayerThrowableAttack.onActiveWeapon -= SetActiveWeapon;
        Player.onAddHealth -= SetAmmountHealth;
        EnemySpawn.onStartSpawn -= HideArrow;
        EnemySpawn.onAllKill -= ShowArrow;
        EnemySpawn.onEnemyCount -= EnemyCount;
    }

    private void Start()
    {
        _maxValue = 100f;
        _slider.maxValue = _maxValue;
        _slider.value = _maxValue;
        _textEnemyCount.enabled = false;
    }

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

        if (amount <=0)
        {
            StartCoroutine(Die());
        }
    }

    private void ShowArrow()
    {
        _arrow.enabled = true;
        _textEnemyCount.enabled = false;
    }

    private void HideArrow()
    {
        _arrow.enabled = false;
        _textEnemyCount.enabled = true;
    }

    private void EnemyCount(int amount)
    {
        _textEnemyCount.text = "Врагов: " + amount.ToString();
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(1.017f);
        _death.gameObject.SetActive(true);
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
