using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public enum BossState
{
    Idle,
    Patrol,
    FastPatrol,
    Change,
    Attack_Prepare,
    Attack_BigBall,
    Attack_3Balls,
    Attack_Spin,
    Hit,
    Die
}

public class BossContoller : MonoBehaviour
{
    public BossState currentState = BossState.Idle;
    public BossState LastAttack = BossState.Attack_BigBall;
    private BossStats stats;
    private Animator animator;
    private BossPatrol patrol;
    private BossAttack attack;
    private BossChange change;

    float idleTimer = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log($"currentState: first {currentState}");

        stats = GetComponent<BossStats>();
        stats.OnPhaseChanged += HandlePhaseChanged;

        animator = GetComponent<Animator>();
        patrol = GetComponent<BossPatrol>();
        attack = GetComponent<BossAttack>();
        change = GetComponent<BossChange>();
    }

    // Update is called once per frame
    void Update()
    {
    
        switch (currentState)
        {
            case BossState.Idle:
                HandleIdle();
                break;
            case BossState.Patrol:
            case BossState.FastPatrol:
                // TODO: Player가 감지되어야만 탈출하는 구조
                DetectPlayer();
                break;
        }
    }

    public void ChangeState(BossState s)
    {
        if (currentState == s) return;
        currentState = s;

        Debug.Log($"currentState: - ChangeState {currentState}");

        switch (currentState)
        {
            case BossState.Idle:
                patrol.Stop();
                animator.SetBool("IsMoving", false);
                animator.SetBool("IsFastMoving", false);
                break;
            case BossState.Patrol:
                patrol.Patrol(15);
                animator.SetBool("IsMoving", true);
                break;
            case BossState.FastPatrol:
                animator.SetBool("IsFastMoving", true);
                patrol.Patrol(30);
                break;
            case BossState.Change:
                animator.SetTrigger("ChangeTrigger");
                change.PhaseChange();
                break;
            case BossState.Hit:
                animator.SetTrigger("HitTrigger");
                // TODO: 링은 던졌을 때만 공격하도록 해야함..
                break;
            case BossState.Attack_Prepare:
                patrol.Stop();
                animator.SetTrigger("AttackTrigger");
                break;
            case BossState.Attack_BigBall:
                animator.SetTrigger("SpinTrigger");
                attack.StartSmoothLookAt(patrol.player.transform);
                attack.Attack_BigBalls(patrol.player.transform);
                break;
            case BossState.Attack_3Balls:
                animator.SetTrigger("SpinTrigger");
                attack.StartSmoothLookAt(patrol.player.transform);
                attack.Attack_SmallBalls(patrol.player.transform, 3);
                break;
            case BossState.Attack_Spin:
                animator.SetTrigger("SpinTrigger");
                attack.Attack_Spin();
                break;
            case BossState.Die:
                animator.SetTrigger("DeathTrigger");
                break;
            default:
                break;
        }
    }

    // TODO: 여기도 고쳐야함
    public void DecideAttackAfterPrepare()
    {
        switch (stats.currentPhase)
        {
            case BossPhase.Phase1:
            case BossPhase.Phase2:
                if (LastAttack == BossState.Attack_3Balls)
                {
                    LastAttack = BossState.Attack_BigBall;
                    ChangeState(BossState.Attack_BigBall);
                }
                else
                {
                    LastAttack = BossState.Attack_3Balls;
                    ChangeState(BossState.Attack_3Balls);
                }
                break;
            case BossPhase.Phase3:
                if (LastAttack == BossState.Attack_Spin)
                {
                    LastAttack = BossState.Attack_BigBall;
                    ChangeState(BossState.Attack_BigBall);
                }
                else
                {
                    LastAttack = BossState.Attack_BigBall;
                    ChangeState(BossState.Attack_Spin);
                }
                break;
        }
    }

    void HandlePhaseChanged(BossPhase newPhase)
    {
        if (newPhase == BossPhase.Phase3)
        {
            ChangeState(BossState.Change);
        }
    }

    void HandleIdle()
    {
        idleTimer += Time.deltaTime;

        float waitTime = (stats.currentPhase == BossPhase.Phase3) ? 1f : 2f;

        if (idleTimer >= waitTime)
        {
            Debug.Log($"currentState: idleTimerReset {idleTimer}");
            idleTimer = 0f;

            if (stats.currentPhase == BossPhase.Phase3) ChangeState(BossState.FastPatrol);
            else ChangeState(BossState.Patrol);
        }
    }

    void DetectPlayer()
    {
        float dist = Vector3.Distance(patrol.player.position, transform.position);
        
        if (dist < 10f)
        {
            Debug.Log($"currentState: detectPlayer {dist}");
            ChangeState(BossState.Attack_Prepare);
        }
    }
}