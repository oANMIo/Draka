using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;

    private int _health;
    private Animator _animator;

    public int Health { get { return _health; } }

    public static Action<int> onAddHealth;

    private void Start()
    {
        _health = _maxHealth;
        onAddHealth?.Invoke(_health);
        _animator = GetComponent<Animator>();
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

        //StopAllCoroutines();
        //StartCoroutine(Delay());

        if (_health <= 0)
        {
            _animator.SetTrigger("Die");
            StartCoroutine(Die());
        }
        else
        {
            _animator.SetBool("Damage", true);
            StartCoroutine(Delay());
        }
    }

    private IEnumerator Die()
    {
        gameObject.GetComponent<PlayerAttack>().enabled = false;
        gameObject.GetComponent<PlayerMovement>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        //yield return new WaitForSeconds(1.017f);
        //Time.timeScale = 0;
        yield return new WaitForSeconds(4f);
        ///Time.timeScale = 1;
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
