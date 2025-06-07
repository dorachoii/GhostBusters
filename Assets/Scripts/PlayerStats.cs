using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// player statusを管理し、変更時にイベントを発行します。
/// </summary>
public class PlayerStats : MonoBehaviour
{
    // Player Status
    public int maxHP = 100;
    public int currentHP;
    public int maxRingCount = 5;
    public int currentRingCount;

    // Events
    public event Action<int> OnHPChanged;
    public event Action<int> OnRingChanged;

    // Player Controller 参照
    PlayerController controller;

    private void Start()
    {
        currentHP = maxHP;
        currentRingCount = 0;
        OnHPChanged?.Invoke(currentHP);
        OnRingChanged?.Invoke(currentRingCount);
        controller = gameObject.GetComponent<PlayerController>();
    }

    /// Player HP減少、イベント発行
    public void TakeDamage(int amount)
    {
        currentHP = Mathf.Max(0, currentHP - amount);
        OnHPChanged?.Invoke(currentHP);
        controller.StateMachine.TransitionTo(controller.StateMachine.hitState, true);
    }

    /// Player HP回復、event発行
    public void Heal(int amount)
    {
        currentHP += amount;
        OnHPChanged?.Invoke(currentHP);
        controller.StateMachine.TransitionTo(controller.StateMachine.healState, true);
    }

    /// Player item数を増やし、eventを発行
    public void GainItem()
    {
        currentRingCount++;
        OnRingChanged?.Invoke(currentRingCount);
    }

    /// Player item数を減らし、eventを発行
    public void LoseItem()
    {
        currentRingCount--;
        OnRingChanged?.Invoke(currentRingCount);
    }
}

