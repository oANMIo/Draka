using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;

    private int _coin = 0;
    private int _health;
    private Animator _animator;

    public static Action<int> onAddCoin;
    public static Action<int> onAddHealth;

    private void Start()
    {
        _health = _maxHealth;
        onAddHealth?.Invoke(_health);
        onAddCoin?.Invoke(_coin);
        _animator = GetComponent<Animator>();
    }

    public void AddCoin(int amount)
    {
        _coin += amount;
        onAddCoin?.Invoke(_coin);
        AudioManager.Instance.Play(AudioManager.Clip.CoinPickup);
    }

    public void AddHealth(int amount)
    {
        _health += amount;
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        onAddHealth?.Invoke(_health);
        AudioManager.Instance.Play(AudioManager.Clip.CoinPickup);
    }

    public void TakeDamage(int damage)
    {
        AudioManager.Instance.Play(AudioManager.Clip.MaleDamage);
     
        _health -= damage;
        onAddHealth?.Invoke(_health);

        _animator.SetBool("Damage", true);
        StopAllCoroutines();
        StartCoroutine(Delay());

        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        ReloadCurrentScene();
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.35f);
        _animator.SetBool("Damage", false);
    }

    private void ReloadCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
