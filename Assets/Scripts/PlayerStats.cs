using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// プレイヤーのステータス（HP、コイン数など）を管理し、変更時にイベントを発行します。
/// </summary>
public class PlayerStats : MonoBehaviour
{
    // Player Status
    public int maxHP = 100;
    public int currentHP;
    public int maxItem = 5;
    public int currentItem;

    // Events
    public event Action<int> OnHPChanged;
    public event Action<int> OnCoinChanged;

    // Player Controller Reference
    PlayerController controller;

    private void Start()
    {
        currentHP = maxHP;
        currentItem = 0;
        OnHPChanged?.Invoke(currentHP);
        OnCoinChanged?.Invoke(currentItem);
        controller = gameObject.GetComponent<PlayerController>();
    }

    public void TakeDamage(int amount)
    {
        currentHP = Mathf.Max(0, currentHP - amount);
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

    // Get Current HP
    public int GetHP() => currentHP;
}

