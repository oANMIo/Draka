using System.Collections;
using UnityEngine;

public class Trash : MonoBehaviour
{
    [SerializeField] private int _health = 100;
    [SerializeField] private GameObject _prefabSpawn;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        _animator.SetTrigger("Active");

        AudioManager.Instance.Play(AudioManager.Clip.Trash);
        _health -= damage;

        if (_health <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(ObjectDestroy());
            
        }
    }
    private IEnumerator ObjectDestroy()
    {
        yield return new WaitForSeconds(0.217f);
        Instantiate(_prefabSpawn, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
