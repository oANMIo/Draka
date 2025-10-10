using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int _punchDamage = 20;
    [SerializeField] private int _kickDamage = 45;
    [SerializeField] private float _attackRate = 2f;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private Vector2 _boxSize = new Vector2(2.3f, 0.6f);

    private Animator _animator;
    private float _nextAttackTime = 0f;
    private bool _isPunch;
    private bool _isKick;

    public bool IsAttack { get; private set; }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        IsAttack = false;
        _isPunch = false;
        _isKick = false;
    }

    private void Update()
    {
        if (Time.time >= _nextAttackTime && IsAttack == false)
        {
            _isPunch = false;
            _isKick = false;

            if (Input.GetMouseButtonDown(0))
            {
                _isPunch = true;
                IsAttack = true;
                _nextAttackTime = Time.time + 1f / _attackRate;
                StartCoroutine(DoAttack());
            }
            else if (Input.GetMouseButtonDown(1))
            {
                _isKick = true;
                IsAttack = true;
                _nextAttackTime = Time.time + 1f / _attackRate;
                StartCoroutine(DoAttack());
            }
        }

        _animator.SetBool("isAttacking", IsAttack);
        _animator.SetBool("isPunch", _isPunch);
        _animator.SetBool("isKick", _isKick);
    }

    public void PunchAttack()
    {
        Attack(_punchDamage);
    }

    public void KickAttack()
    {
        Attack(_kickDamage);
    }
 
    private void Attack(int attackDamage)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(_attackPoint.position, _boxSize, 0, _enemyLayer); 

        foreach (Collider2D enemy in hitEnemies) 
        {
            if (enemy != null)
            {
                Vector2 knockDir = (enemy.transform.position - transform.position).normalized;
                enemy.gameObject.GetComponent<ItemDamage>().TakeDamage(attackDamage, knockDir);
            }
        }
    }

    public void PlaySwing()
    {
        AudioManager.Instance.Play(AudioManager.Clip.Swing);
    }

    private IEnumerator DoAttack()
    {
        //yield return new WaitForSeconds(0.05f);sssss
        yield return new WaitForSeconds(0.283f);
        IsAttack = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_attackPoint.position, _boxSize);
    }
}
