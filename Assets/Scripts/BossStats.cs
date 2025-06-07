using System;
using UnityEngine;
using UnityEngine.Events;

public enum BossPhase
{
    Phase1,
    Phase2,
    Phase3
}

/// <summary>
/// boss statusを管理し、変更時にイベントを発行します。
/// </summary>

public class BossStats : MonoBehaviour
{
    // Boss Status
    public int maxHP = 100;
    public int currentHP;
    public BossPhase currentPhase = BossPhase.Phase1;

    // Events
    public event Action<int> OnHPChanged;
    public event Action<BossPhase> OnPhaseChanged;

    BossContoller controller;

    private void Start()
    {
        currentHP = maxHP;
        OnHPChanged?.Invoke(currentHP);
        controller = gameObject.GetComponent<BossContoller>();
    }

    public int GetHP() => currentHP;

    public void TakeDamage(int amount)
    {
        currentHP = Mathf.Max(currentHP - amount, 0);
        OnHPChanged?.Invoke(currentHP);

        if (currentHP <= 0)
        {
            controller.ChangeState(BossState.Die);
            return;
        }

        controller.ChangeState(BossState.Hit);

        //10以下になったら、Phase変更
        if (currentHP <= 10) ChangePhase(BossPhase.Phase3);
        else if (currentHP <= 20) ChangePhase(BossPhase.Phase2);
    }

    private void ChangePhase(BossPhase newPhase)
    {
        if (currentPhase == newPhase) return;

        currentPhase = newPhase;
        OnPhaseChanged?.Invoke(currentPhase);
    }
}
