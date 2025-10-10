using UnityEngine;

public enum WeaponType { Bottle, Knife }

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private WeaponType _weaponType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerThrowableAttack player))
        {
            if (_weaponType == WeaponType.Bottle)
            {
                player.UpdateBottle(1);
            }

            if (_weaponType == WeaponType.Knife)
            {
                player.UpdateKnife(1);
            }

            Destroy(gameObject);
            AudioManager.Instance.Play(AudioManager.Clip.CoinPickup);
        }
    }
}
