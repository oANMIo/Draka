using System.Collections;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private int _damage = 5;
    [SerializeField] private float _StartTime = 0.8f;

    private bool _isPlayerDamage;
    private bool _isEnemyDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            _isPlayerDamage = true;
            StartCoroutine(PlayerTakeDamage(player));
        }
        else if (collision.TryGetComponent(out ItemDamage enemy) && collision.gameObject.tag != "Box")
        {
            _isEnemyDamage = true;
            StartCoroutine(EnemyTakeDamage(enemy));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            _isPlayerDamage = false;
        }
        else if (collision.TryGetComponent(out ItemDamage enemy))
        {
            _isEnemyDamage = false;
        }
    }

    private IEnumerator PlayerTakeDamage(Player player)
    {
        while (_isPlayerDamage)
        {
            yield return null;
            player.TakeDamage(_damage);
            yield return new WaitForSeconds(_StartTime);
        }
    }

    private IEnumerator EnemyTakeDamage(ItemDamage enemy)
    {
        while (_isEnemyDamage)
        {
            yield return null;
            enemy.TakeDamage(_damage, Vector2.zero);
            yield return new WaitForSeconds(_StartTime);
        }
    }
}
