using System.Collections;
using UnityEngine;

public class Box : ItemDamage
{
    [SerializeField] private int _health = 100;
    [SerializeField] private GameObject _prefabSpawn;

    [Header("FX Damage")]
    [SerializeField] private float _knockbackForce = 50f;
    [SerializeField] private float _knockbackDuration = 0.03f;
    [SerializeField] private GameObject _hitEffectPrefab;

    private bool _isKnockedBack = false;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public override void TakeDamage(int damage, Vector2 hitDirection)
    {
        if (_isKnockedBack) return;

        AudioManager.Instance.Play(AudioManager.Clip.Trash);
        _health -= damage;

        SpawnHitEffect(hitDirection);
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

    private void SpawnHitEffect(Vector2 hitDirection) 
    {
        if (_hitEffectPrefab == null) return;

        Vector3 spawnOffset = new Vector3(0, 1f, 0);
        Vector3 spawnPos = transform.position + spawnOffset + (Vector3)(-hitDirection.normalized * 0.5f);

        GameObject effect = Instantiate(_hitEffectPrefab, spawnPos, Quaternion.identity);

        // Разворачиваем в сторону удара (чтобы летели “в обратную”)
        float angle = Mathf.Atan2(hitDirection.y, hitDirection.x) * Mathf.Rad2Deg;
        effect.transform.rotation = Quaternion.Euler(0, 0, angle + 180f);
    }
}
