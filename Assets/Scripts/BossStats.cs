using System;
using UnityEngine;
using UnityEngine.Events;

public enum BossPhase
{
    Phase1,
    Phase2,
    Phase3
}

public class BossStats : MonoBehaviour
{
    public int maxHP = 30;
    public int currentHP;
    public BossPhase currentPhase = BossPhase.Phase1;

    public event Action<int> OnHPChanged;
    public event Action<BossPhase> OnPhaseChanged;

    BossContoller controller;

    void Awake()
    {
        currentHP = maxHP;
        OnHPChanged?.Invoke(currentHP);
        controller = gameObject.GetComponent<BossContoller>();

    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        OnHPChanged?.Invoke(currentHP);

        controller.ChangeState(BossState.Hit);

        if (currentHP == 20) ChangePhase(BossPhase.Phase2);
        else if (currentHP == 10) ChangePhase(BossPhase.Phase3);
    }

    private void ChangePhase(BossPhase newPhase)
    {
        if (currentPhase == newPhase) return;

        currentPhase = newPhase;
        OnPhaseChanged?.Invoke(currentPhase);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"boss collision occured{collision.gameObject.tag}");
        Debug.Log($"boss collision occured bossHP {currentHP}");
        
    }
}
