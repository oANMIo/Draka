using System.Collections;
using UnityEngine;

public class EnemyCop : ItemDamage
{
    [SerializeField] private int _health = 200;

    [Header("AI Movement")]
    [SerializeField] private float _speed = 1.6f;
    [SerializeField] private float _separationRadius = 2.5f;
    [SerializeField] private float _maxForce = 5f;
    [SerializeField] private float _slowingRadius = 1f;
    [SerializeField] private float _attackCooldown = 1.5f;

    [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private float _chaseRange = 5f;
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private Vector2 _boxSize = new Vector2(2.3f, 0.6f);
    [SerializeField] private int _damage = 40;

    [Header("FX Damage")]
    [SerializeField] private float _knockbackForce = 40f;
    [SerializeField] private float _knockbackDuration = 0.03f; 
    [SerializeField] private GameObject _hitEffectPrefab;

    private bool _isKnockedBack = false;
    private Vector2 _velocity;
    private BoxCollider2D _collider;
    private Rigidbody2D _rb;
    private Animator _animator;
    private bool _isDamage = false;
    private bool _isAlive = true;
    private float _lastAttackTime;

    private bool _targetInChaseRange = false;

    public bool IsChase { get;  set; }
    //public Player Target { get;  set; }

    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);
        _animator.Play(state.fullPathHash, -1, UnityEngine.Random.Range(0f, 1f));
        _animator.speed = UnityEngine.Random.Range(0.9f, 1.1f);
        _targetInChaseRange = false;
    }

    private void FixedUpdate()
    {
        if (_isAlive == false || _isKnockedBack) return;

        _targetInChaseRange = Physics2D.OverlapCircle(transform.position, _chaseRange, _whatIsPlayer);
       
        if (_targetInChaseRange)
        {
            Flip();
            Move();
        }
        else
        {
            _velocity = Vector2.zero;
            _rb.MovePosition(_rb.position + _velocity * Time.fixedDeltaTime);
        }

        _animator.SetBool("Damage", _isDamage);
        _animator.SetFloat("Speed", _velocity.sqrMagnitude);
    }

    public void SetTriggerDamage() 
    {
        _isDamage = false;
    }

    public override void TakeDamage(int damage, Vector2 hitDirection)
    {
        if (_isKnockedBack) return;

        _health -= damage;

        if (hitDirection != Vector2.zero)
        {
            AudioManager.Instance.Play(AudioManager.Clip.Hit);
            SpawnHitEffect(hitDirection);
            StartCoroutine(DoKnockback(hitDirection));
        }

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

    private void Flip()
    {
        if (Player == null) return;
        
        transform.localScale = new Vector3(Mathf.Sign(transform.position.x - Player.transform.position.x), 1, 1);
    }

    private void Move() 
    {
        if (Player == null) return;

        float dist = Vector2.Distance(transform.position, Player.transform.position);

        Vector2 target = (Vector2)Player.transform.position + LateralOffset(); // flang
        Vector2 arrive = Arrive(target);
        Vector2 sep = Separation();
        _velocity += (arrive + sep) * Time.deltaTime;

        if (dist < _attackRange)
        {
            TryAttack();
            _velocity = Vector2.zero;
        }

        _velocity = Vector2.ClampMagnitude(_velocity, _speed);

        _rb.MovePosition(_rb.position + _velocity * Time.fixedDeltaTime);
    }

    private Vector2 LateralOffset()
    {
        Vector2 dir = (transform.position - Player.transform.position).normalized;
        Vector2 right = Vector3.Cross(Vector2.up, dir);
        float offset = UnityEngine.Random.Range(-1f, 1f) * 1.5f;
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

        if (Friends.Count == 0) return steer;

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
        if (Time.time - _lastAttackTime < _attackCooldown && _isAlive) 
            return;

        _lastAttackTime = Time.time;
        _animator.SetTrigger("Attack");
    }

    public void Attack()
    {
        Collider2D player = Physics2D.OverlapBox(_attackPoint.position, _boxSize, 0, _whatIsPlayer);

        if (player != null)
        {
            player.gameObject.GetComponent<Player>().TakeDamage(_damage);
        }
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

    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_attackPoint.position, _boxSize);
    }
}
