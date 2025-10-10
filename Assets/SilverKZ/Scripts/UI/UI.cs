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
}
