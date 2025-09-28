using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 6f;

    [Header("Attack")] 
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange = 0.5f;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private int _attackDamage = 30;
    [SerializeField] private float _attackRate = 2f;

    private Rigidbody2D _rb;
    private Animator _animator;
    private Vector2 _movement;

    private float _nextAttackTime = 0f;
    private bool _isAttack = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update() 
    {
        if (_isAttack) return;

        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        // Flip
        if (_movement.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(_movement.x), 1, 1);
        }

        // Attack
        if (Time.time >= _nextAttackTime && _isAttack == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                _isAttack = true;
                _nextAttackTime = Time.time + 1f / _attackRate;
            }
        }

        _animator.SetFloat("Speed", _movement.sqrMagnitude);
        _animator.SetBool("isAttacking", _isAttack);
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movement.normalized * _moveSpeed * Time.fixedDeltaTime);
    }

    public void PlaySwing()
    {
        AudioManager.Instance.Play(AudioManager.Clip.Swing);
    }

    public void SetAttack()
    {
        _isAttack = false;
    }

    private void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(_attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }
}
