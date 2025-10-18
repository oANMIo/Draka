using UnityEngine;

public class Medical : MonoBehaviour
{
    [SerializeField] private int _health = 50;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            if (player.Health < 100)
            {
                player.AddHealth(_health);
                Destroy(gameObject);
            }
        }
    }
}
