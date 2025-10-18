using System;
using System.Collections;
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
    [SerializeField] private float _xTolerance = 0.3f;     // допустима€ разница по Y
    [SerializeField] private float _throwCooldown = 2f;    // перезар€дка броска
    [SerializeField] private float _throwHeight = 3f;      // высота дуги
    [SerializeField] private float _throwDuration = 1f;    // врем€ полета

    [Header("FX Damage")]
    [SerializeField] private GameObject _hitEffectPrefab;
    [SerializeField] private float _knockbackForce = 50f;
    [SerializeField] private float _knockbackDuration = 0.03f;

    private bool _isKnockedBack = false;
    private float _throwTimer = 0f;
    private Animator _animator;
    private BoxCollider2D _collider;
    private Rigidbody2D _rb;
    private float _speed = 0f;
    private bool _isDamage = false;
    private bool _isAlive = true;
    private bool _isLeftMove = true;
    private Vector2 _velocity;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _speed = 0f;
    }

    private void FixedUpdate()
    {
        if (_isAlive == false) return;

        _animator.SetBool("Damage", _isDamage);

        if (Player == null || _isKnockedBack) return;

        float dist = Vector2.Distance(transform.position, Player.transform.position);

        _velocity = Vector2.zero;
        _velocity = Vector2.ClampMagnitude(_velocity, _speed);
        _rb.MovePosition(_rb.position + _velocity * Time.fixedDeltaTime);

        if (dist < _activationDistance && dist > 1.5f)
        {
            Flip();

            if (Mathf.Abs(transform.position.x - Player.transform.position.x) < _xTolerance && _speed != 1f)
            {
                _speed = 1f;
                StartCoroutine(Move());
            }
            else if (_speed != 1f)
            {
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
        if (hitDirection != Vector2.zero && _isKnockedBack == false)
        {
            _health -= damage;
            AudioManager.Instance.Play(AudioManager.Clip.Hit);
            SpawnHitEffect(hitDirection);
            StartCoroutine(DoKnockback(hitDirection));
        }

        /*
        if (_isKnockedBack) return;

        _health -= damage;

        if (hitDirection != Vector2.zero)
        {
            AudioManager.Instance.Play(AudioManager.Clip.Hit);
            SpawnHitEffect(hitDirection);
            StartCoroutine(DoKnockback(hitDirection));
        }
        */

        if (_health <= 0)
        {
            Die();
        }
        else
        {
            _isDamage = true;
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

        // –азворачиваем в сторону удара (чтобы летели Ув обратнуюФ)
        float angle = Mathf.Atan2(hitDirection.y, hitDirection.x) * Mathf.Rad2Deg;
        effect.transform.rotation = Quaternion.Euler(0, 0, angle + 180f);
    }

    private IEnumerator Move()
    {
        Vector2 targetPos = transform.position;
        targetPos.x += (_isLeftMove) ? -3f : 3f;

        while (Vector2.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, _moveSpeed * Time.deltaTime);
            yield return null;
        }

        _isLeftMove = !_isLeftMove;
        _speed = 0f;
        yield return null;
    }

    private IEnumerator DoKnockback(Vector2 direction)
    {
        if (_isKnockedBack || _rb == null) yield break;

        _isKnockedBack = true;

        _rb.WakeUp();
        _rb.linearVelocity = Vector2.zero;
        Vector2 knockbackDir = new Vector2(Mathf.Sign(direction.x), 0f);
        _rb.AddForce(knockbackDir * _knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(_knockbackDuration);

        _rb.linearVelocity = Vector2.zero;
        _isKnockedBack = false;
    }
}
