using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public enum BossState
{
    Idle,
    Patrol,
    FastPatrol,
    Attack_Prepare,
    Attack_BigBall,
    Attack_3Balls,
    Attack_8Balls,
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
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stats = GetComponent<BossStats>();
        animator = GetComponent<Animator>();
        patrol = GetComponent<BossPatrol>();
        attack = GetComponent<BossAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ChangeState(BossState.Attack_Prepare);
        }

        // if (Input.GetKeyUp(KeyCode.Space))
        // {
        //     ChangeState(BossState.Idle);
        // }
    }

    public void ChangeState(BossState s)
    {
        if (currentState == s) return;
        currentState = s;

        switch (currentState)
        {
            case BossState.Idle:
                patrol.Stop();
                animator.SetBool("IsMoving", false);
                break;
            case BossState.Patrol:
                patrol.Patrol();
                animator.SetBool("IsMoving", true);
                break;
            case BossState.FastPatrol:
                break;
            case BossState.Hit:
                animator.SetTrigger("HitTrigger");
                break;
            case BossState.Attack_Prepare:
                animator.SetTrigger("AttackTrigger");
                
                break;
            case BossState.Attack_BigBall:
                attack.StartSmoothLookAt(patrol.player.transform);
                attack.Attack_BigBalls(patrol.player.transform);
                break;
            case BossState.Attack_3Balls:
                attack.StartSmoothLookAt(patrol.player.transform);
                attack.Attack_3Balls(patrol.player.transform);
                break;
            case BossState.Attack_8Balls:
                break;
            case BossState.Die:
                break;
            default:
                break;
        }
    }

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
        }
    }
}



// 보스 목숨 3개

// 목숨 3,2개 남았을 때:
// 공격 패턴 1: 따라가면서 불쏘기 1번
// 공격 패턴 2: 큰 공쏴서 줄다리기 1번

// 큰 공 맞으면 목숨 1개 사라짐 
// 혹은 링 모아서 5개 맞추면 목숨 한 개 사라짐짐 

// 목숨 1개 남았을 때:

// 공격 패턴 1: 불 빨라짐 (사방 공격하지뭐)
// 공격 패턴 2: 큰 공쏴서 줄다리기

// 큰 공 맞추면 목숨 1개 사라짐. 근데 아까보다 공 미는 힘이 강해짐
// 혹은 링 모아서 10개 맞추면 목숨 한 개 사라짐짐
