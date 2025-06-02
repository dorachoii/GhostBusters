using System;
using Unity.VisualScripting;
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

    PlayerController controller;

    private void Awake()
    {
        currentHP = maxHP;
        currentItem = 0;
        OnHPChanged?.Invoke(currentHP);
        OnCoinChanged?.Invoke(currentItem);
        controller = gameObject.GetComponent<PlayerController>();
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        OnHPChanged?.Invoke(currentHP);
        controller.StateMachine.TransitionTo(controller.StateMachine.hitState, true);
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        OnHPChanged?.Invoke(currentHP);
        controller.StateMachine.TransitionTo(controller.StateMachine.healState, true);
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

