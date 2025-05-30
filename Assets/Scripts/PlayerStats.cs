using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;
    public int maxItem = 5;
    public int currentItem;

    public event Action<int> OnHPChanged;
    public event Action<int> OnCoinChanged;

    private void Awake()
    {
        currentHP = maxHP;
        currentItem = 0;
        OnHPChanged?.Invoke(currentHP);
        OnCoinChanged?.Invoke(currentItem);
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        OnHPChanged?.Invoke(currentHP);
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        OnHPChanged?.Invoke(currentHP);
    }

    public void GainItem()
    {
        currentItem++;
        OnCoinChanged?.Invoke(currentItem);
    }

    
    public void LoseItem()
    {
        currentItem--;
        OnCoinChanged?.Invoke(currentItem);
    }

    public int GetHP() => currentHP;
}

