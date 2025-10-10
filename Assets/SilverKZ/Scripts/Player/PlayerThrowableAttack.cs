using System;
using UnityEngine;

public class PlayerThrowableAttack : MonoBehaviour
{
    [SerializeField] private ThrowableWeapon _prefabBottle;
    [SerializeField] private ThrowableWeapon _prefabKnife;
    [SerializeField] private float _attackRate = 2f;
    [SerializeField] private float _throwForce = 10f;
    [SerializeField] private Transform _attackPoint;

    private int _direction = 1;
    private float _nextAttackTime = 0f;
    private WeaponType _activeWeapon;
    private int _countBottle;
    private int _countKnife;

    public static Action<int> onAddBottle;
    public static Action<int> onAddKnife;
    public static Action<WeaponType> onActiveWeapon;

    private void Start()
    {
        _activeWeapon = WeaponType.Bottle;
        onActiveWeapon?.Invoke(_activeWeapon);
        _countBottle = 0;
        _countKnife = 0;
    }

    private void Update()
    {
        if (Time.time < _nextAttackTime) return;

        SelectWeapon();
        ThrowableAttack();
    }

    public void UpdateBottle(int amount)
    {
        _countBottle += amount;
        onAddBottle?.Invoke(_countBottle);
    }

    public void UpdateKnife(int amount)
    {
        _countKnife += amount;
        onAddKnife?.Invoke(_countKnife);
    }

    private void UseWeapon() 
    {
        AudioManager.Instance.Play(AudioManager.Clip.Swing);

        _direction = transform.localScale.x > 0 ? 1 : -1;
        _nextAttackTime = Time.time + 1f / _attackRate;

        ThrowableWeapon activeWeapon = (_activeWeapon == WeaponType.Bottle) ? _prefabBottle : _prefabKnife;

        ThrowableWeapon throwableWeapon = Instantiate(activeWeapon, _attackPoint.position, Quaternion.identity);
        Rigidbody2D rb = throwableWeapon.GetComponent<Rigidbody2D>();
        Vector3 weaponScale = throwableWeapon.transform.localScale;
        weaponScale.x = Mathf.Abs(weaponScale.x) * _direction;
        throwableWeapon.transform.localScale = weaponScale;
        rb.linearVelocity = new Vector2(_direction * _throwForce, 0);

        Destroy(throwableWeapon.gameObject, 2f);
    }

    private void SelectWeapon()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            if (_activeWeapon == WeaponType.Bottle)
            {
                _activeWeapon = WeaponType.Knife;
            }
            else
            {
                _activeWeapon = WeaponType.Bottle;
            }

            onActiveWeapon?.Invoke(_activeWeapon);
        }
    }

    private void ThrowableAttack()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (_activeWeapon == WeaponType.Bottle && _countBottle > 0)
            {
                UseWeapon();
                _countBottle--;
                onAddBottle?.Invoke(_countBottle);
            } 
            else if (_activeWeapon == WeaponType.Knife && _countKnife > 0)
            {
                UseWeapon();
                _countKnife--;
                onAddKnife?.Invoke(_countKnife);
            }
        }
    }
}
