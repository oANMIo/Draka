using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField] private int _damage = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            player.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
