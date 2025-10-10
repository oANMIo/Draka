using System.Collections;
using UnityEngine;

public class Trash : ItemDamage
{
    [SerializeField] private int _health = 100;
    [SerializeField] private GameObject _prefabSpawn;
    [Header("FX Damage")]
    [SerializeField] private float _knockbackForce = 50f;
    [SerializeField] private float _knockbackDuration = 0.03f;

    private bool _isKnockedBack = false;
    private Animator _animator;
    private Rigidbody2D _rb;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    public override void TakeDamage(int damage, Vector2 hitDirection)
    {
        if (_isKnockedBack) return;

        _animator.SetTrigger("Active");
        AudioManager.Instance.Play(AudioManager.Clip.Trash);
        _health -= damage;

        StartCoroutine(DoKnockback(hitDirection));

        if (_health <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(ObjectDestroy());
            
        }
    }

    private IEnumerator DoKnockback(Vector2 direction)
    {
        if (_isKnockedBack || _rb == null) yield break;

        _isKnockedBack = true;

        _rb.WakeUp();
        _rb.linearVelocity = Vector2.zero;
        _rb.linearVelocity = direction.normalized * _knockbackForce;

        yield return new WaitForSeconds(_knockbackDuration);

        _rb.linearVelocity = Vector2.zero;
        _isKnockedBack = false;
    }

    private IEnumerator ObjectDestroy()
    {
        yield return new WaitForSeconds(0.217f);
        Instantiate(_prefabSpawn, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
