using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    //private int _health = 100;
    private int _coin = 0;

    public static Action<int> onAddCoin;

    public void AddCoin(int amount)
    {
        _coin += amount;
        onAddCoin?.Invoke(_coin);
    }
}
