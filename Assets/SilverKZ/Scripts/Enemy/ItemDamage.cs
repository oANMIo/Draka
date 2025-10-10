using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemDamage : MonoBehaviour
{
    public int spawnID;

    public List<ItemDamage> Friends { get; set; }

    public static Action<ItemDamage> onDeathEnemy;

    private void Start()
    {
        Friends = new List<ItemDamage>();
    }

    public void DeathEnemy()
    {
        onDeathEnemy?.Invoke(this);
    }

    public virtual void TakeDamage(int damage, Vector2 hitDirection) { }
}
