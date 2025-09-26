using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _health = 100;

    private Collider2D _collider;
    private Animator _animator;
    private bool _isDamage = false;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool("Damage", _isDamage);
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
        _collider.enabled = false;
        _animator.SetBool("Die", true);
    }
}
