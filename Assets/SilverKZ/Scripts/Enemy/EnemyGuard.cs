using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGuard : ItemDamage
{
    [Header("Settings")]
    [SerializeField] private int _health = 100;
    [SerializeField] private float _activationDistance = 10f;
    [SerializeField] private float _moveSpeed = 2f;

    [Header("Throw")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _throwPoint;        // точка броска
    [SerializeField] private float _yTolerance = 0.3f;     // допустимая разница по Y
    [SerializeField] private float _throwCooldown = 2f;    // перезарядка броска
    [SerializeField] private float _throwHeight = 3f;      // высота дуги
    [SerializeField] private float _throwDuration = 1f;    // время полета

    [Header("FX Damage")]
    [SerializeField] private GameObject _hitEffectPrefab;

    private float _throwTimer = 0f;
    private Animator _animator;
    private BoxCollider2D _collider;
    private float _speed = 0f;
    private bool _isDamage = false;
    private bool _isAlive = true;

    public Player Player { get; set; }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (_isAlive == false || Player == null) return;

        float dist = Vector2.Distance(transform.position, Player.transform.position);
        _speed = 0f;

        // 1. Активация
        if (dist < _activationDistance && dist > 2f)
        {
            // 2. Двигаемся по Y
            Vector2 pos = transform.position;
            pos.y = Mathf.MoveTowards(pos.y, Player.transform.position.y, _moveSpeed * Time.deltaTime);
            _speed = (pos - (Vector2)transform.position).magnitude;
            transform.position = pos;

            Flip();

            // 3. Проверяем, выровнялись ли с игроком
            if (Mathf.Abs(transform.position.y - Player.transform.position.y) < _yTolerance)
            {
                // 4. Таймер броска
                _throwTimer -= Time.deltaTime;

                if (_throwTimer <= 0f)
                {
                    Throw();
                    _throwTimer = _throwCooldown;
                }
            }
        }

        _animator.SetFloat("Speed", _speed);
        _animator.SetBool("Damage", _isDamage);
    }

    public void SetTriggerDamage()
    {
        _isDamage = false;
    }

    public override void TakeDamage(int damage, Vector2 hitDirection)
    {
        AudioManager.Instance.Play(AudioManager.Clip.Hit);
        _health -= damage;
        _isDamage = true;

        SpawnHitEffect(hitDirection);

        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        AudioManager.Instance.Play(AudioManager.Clip.MaleDamage);
        _isAlive = false;
        _collider.enabled = false;
        _animator.SetBool("Die", true);
        DeathEnemy();
    }

    private void Throw()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _throwPoint.position, Quaternion.identity);

        var parabola = bullet.GetComponent<ParabolicThrow>();

        if (parabola != null)
        {
            parabola.StartThrow(_throwPoint.position, Player.transform, _throwDuration, _throwHeight);
        }
    }

    private void Flip()
    {
        if (Player == null) return;

        transform.localScale = new Vector3(Mathf.Sign(transform.position.x - Player.transform.position.x), 1, 1);
    }

    private void SpawnHitEffect(Vector2 hitDirection)
    {
        if (_hitEffectPrefab == null) return;

        Vector3 spawnOffset = new Vector3(0, 2f, 0);
        Vector3 spawnPos = transform.position + spawnOffset + (Vector3)(hitDirection.normalized * 0.3f);

        GameObject effect = Instantiate(_hitEffectPrefab, spawnPos, Quaternion.identity);

        // Разворачиваем в сторону удара (чтобы летели “в обратную”)
        float angle = Mathf.Atan2(hitDirection.y, hitDirection.x) * Mathf.Rad2Deg;
        effect.transform.rotation = Quaternion.Euler(0, 0, angle + 180f);
    }
}
