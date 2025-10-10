using UnityEngine;

public class ThrowableWeapon : MonoBehaviour
{
    [SerializeField] private int _damage = 30;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ItemDamage enemy))
        {
            Vector2 knockDir = (enemy.transform.position - transform.position).normalized;
            enemy.TakeDamage(_damage, knockDir);
            Destroy(gameObject);
        }
    }
}
