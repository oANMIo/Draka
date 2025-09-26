using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _textCoin;

    private void OnEnable()
    {
        Player.onAddCoin += SetAmmountCoin;
    }

    private void OnDisable()
    {
        Player.onAddCoin -= SetAmmountCoin;
    }

    private void SetAmmountCoin(int amount)
    {
        AudioManager.Instance.Play(AudioManager.Clip.CoinPickup);
        _textCoin.text = "$ " + amount.ToString();
    }
}
