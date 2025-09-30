using UnityEngine;

public class ChaseTrigger : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            _enemy.IsChase = true;
            _enemy.Target = player;
            Destroy(gameObject);
        }
    }
}
