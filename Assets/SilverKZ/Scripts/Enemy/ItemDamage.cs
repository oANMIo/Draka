using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemDamage : MonoBehaviour
{
    public int spawnID;

    public List<GameObject> Friends { get; set; }
    public Player Player { get; set; }

    public static Action<ItemDamage> onDeathEnemy;

    private void Start()
    {
        Friends = new List<GameObject>();
    }

    public void DeathEnemy()
    {
        onDeathEnemy?.Invoke(this);
    }

    public virtual void TakeDamage(int damage, Vector2 hitDirection) { }
}
