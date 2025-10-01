using System;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Idle,
    Chase
}

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _health = 100;
    [SerializeField] private EnemyType _type;

    [Header("AI Movement")]
    [SerializeField] private float _attackRange = 2.0f;
    [SerializeField] private float _speed = 1.6f;
    [SerializeField] private float _separationRadius = 2.5f;
    [SerializeField] private float _maxForce = 5f;
    [SerializeField] private float _slowingRadius = 2f;
    [SerializeField] private float _attackCooldown = 1.5f;

    private Vector2 _velocity;
    private BoxCollider2D _collider;
    private Rigidbody2D _rb;
    private Animator _animator;
    private bool _isDamage = false;
    private bool _isAlive = true;
    private float _lastAttackTime;

    public static Action<int> onDeathEnemy;

    public bool IsChase { get;  set; }
    public Player Target { get;  set; }
    public List<Enemy> Friends { private get; set; }

    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);
        _animator.Play(state.fullPathHash, -1, UnityEngine.Random.Range(0f, 1f));
        _animator.speed = UnityEngine.Random.Range(0.9f, 1.1f);
        Friends = new List<Enemy>();
    }

    private void FixedUpdate()
    {
        if (_isAlive == false) return;
  
        Flip();
        Move();

        _animator.SetBool("Damage", _isDamage);
        _animator.SetFloat("Speed", _velocity.sqrMagnitude);
    }

    public void DeathEnemy()
    {
        onDeathEnemy?.Invoke(1);
    }

    public void SetTriggerDamage()
    {
        _isDamage = false;
    }

    public void TakeDamage(int damage)
    {
        _animator.SetBool("Damage", _isDamage);
        //this.GetComponent<Animator>().SetBool("Damage", _isDamage);

        AudioManager.Instance.Play(AudioManager.Clip.Hit);
        _health -= damage;
        _isDamage = true;

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

    private void Flip()
    {
        if (Target == null) return;
        
        transform.localScale = new Vector3(Mathf.Sign(transform.position.x - Target.transform.position.x), 1, 1);
    }

    private void Move() 
    {
        if (Target == null) return;

        float dist = Vector2.Distance(transform.position, Target.transform.position);

        Vector2 target = (Vector2)Target.transform.position + LateralOffset(); // flang
        Vector2 arrive = Arrive(target);
        Vector2 sep = Separation();
        _velocity += (arrive + sep) * Time.deltaTime;
        //transform.position += _velocity * Time.deltaTime;

        // атака если близко
        if (dist < _attackRange)
        {
            TryAttack();
            _velocity = Vector2.zero;
        }

        // затухание скорости
        _velocity = Vector2.ClampMagnitude(_velocity, _speed);

        _rb.MovePosition(_rb.position + _velocity * Time.fixedDeltaTime);
    }

    private Vector2 LateralOffset()
    {
        // Смещение вбок от игрока (чтобы не толпились в одной точке)
        Vector2 dir = (transform.position - Target.transform.position).normalized;
        Vector2 right = Vector3.Cross(Vector2.up, dir);
        float offset = UnityEngine.Random.Range(-1f, 1f) * 1.5f; // боковое смещение
        return right * offset;
    }

    private Vector2 Arrive(Vector2 target)
    {
        Vector2 toTarget = target - (Vector2)transform.position;
        float dist = toTarget.magnitude;

        if (dist < 0.1f) 
            return Vector2.zero;

        float desiredSpeed = (dist < _slowingRadius) ? _speed * (dist / _slowingRadius) : _speed;
        Vector2 desired = toTarget.normalized * desiredSpeed;
        Vector2 steer = desired - _velocity;

        return Vector2.ClampMagnitude(steer, _maxForce);
    }

    private Vector2 Separation()
    {
        Vector2 steer = Vector2.zero;
        int count = 0;

        foreach (var other in Friends)
        {
            if (other == null || other == this) 
                continue;

            float d = Vector2.Distance(transform.position, other.transform.position);

            if (d > 0 && d < _separationRadius)
            {
                steer += ((Vector2)transform.position - (Vector2)other.transform.position).normalized / d;
                count++;
            }
        }

        if (count > 0) 
            steer /= count;

        return steer;
    }

    private void TryAttack()
    {
        if (Time.time - _lastAttackTime < _attackCooldown) 
            return;

        _lastAttackTime = Time.time;
        Debug.Log($"{gameObject.name} атакует игрока!");

        // Тут можно включить анимацию или вызвать урон игроку
        // animator.SetTrigger("Attack");
        // player.GetComponent<PlayerHealth>().TakeDamage(damage);
    }
}
