using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _textCoin;
    [SerializeField] private TMPro.TextMeshProUGUI _textHealth;
    [SerializeField] private Image _arrow;

    private void OnEnable()
    {
        Player.onAddCoin += SetAmmountCoin;
        Player.onAddHealth += SetAmmountHealth;
        EnemySpawn.onStartSpawn += HideArrow;
        EnemySpawn.onAllKill += ShowArrow;
    }

    private void OnDisable()
    {
        Player.onAddCoin -= SetAmmountCoin;
        Player.onAddHealth -= SetAmmountHealth;
        EnemySpawn.onStartSpawn -= HideArrow;
        EnemySpawn.onAllKill -= ShowArrow;
    }

    private void SetAmmountCoin(int amount)
    {
        _textCoin.text = "$ " + amount.ToString();
    }

    private void SetAmmountHealth(int amount)
    {
        _textHealth.text = "HP " + amount.ToString();
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
