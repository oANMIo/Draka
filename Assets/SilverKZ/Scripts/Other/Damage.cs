using System.Collections;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private int _damage = 5;
    [SerializeField] private float _startTime = 0.8f;

    private bool _isDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            _isDamage = true;
            StopAllCoroutines();
            StartCoroutine(TakeDamage(player));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            _isDamage = false;
        }
    }

    private IEnumerator TakeDamage(Player player)
    {
        while (_isDamage)
        {
            yield return null;
            player.TakeDamage(_damage);
            yield return new WaitForSeconds(_startTime);
        }
    }
}
