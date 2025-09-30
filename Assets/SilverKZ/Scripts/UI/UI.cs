using System;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _textCoin;
    [SerializeField] private Image _arrow;

    private void OnEnable()
    {
        Player.onAddCoin += SetAmmountCoin;
        EnemySpawn.onStartSpawn += HideArrow;
        EnemySpawn.onAllKill += ShowArrow;
    }

    private void OnDisable()
    {
        Player.onAddCoin -= SetAmmountCoin;
        EnemySpawn.onStartSpawn -= HideArrow;
        EnemySpawn.onAllKill -= ShowArrow;
    }

    private void SetAmmountCoin(int amount)
    {
        AudioManager.Instance.Play(AudioManager.Clip.CoinPickup);
        _textCoin.text = "$ " + amount.ToString();
    }

    private void ShowArrow()
    {
        _arrow.enabled = true;
    }

    private void HideArrow()
    {
        _arrow.enabled = false;
    }
}
