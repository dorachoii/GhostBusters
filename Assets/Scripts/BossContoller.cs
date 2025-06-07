using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ボスの状態管理と行動制御を行い、各フェーズに応じた攻撃パターンを実行します。
/// </summary>

// ボスの状態を定義する列挙型
public enum BossState
{
    Idle,           // 待機状態
    Patrol,         // 通常パトロール
    FastPatrol,     // 高速パトロール
    Change,         // フェーズ変更
    Attack_Prepare, // 攻撃準備
    Attack_BigBall, // 大きい岩攻撃
    Attack_3Balls,  // 3つの小さい岩攻撃
    Attack_Spin,    // 回転攻撃
    Hit,           // 被ダメージ
    Die            // 死亡
}

public class BossContoller : MonoBehaviour
{
    // 現在の状態と最後の攻撃
    public BossState currentState = BossState.Idle;
    public BossState LastAttack = BossState.Attack_BigBall;

    // ボスのコンポーネント
    private BossStats stats;
    private Animator animator;
    private AudioPlayer audioPlayer;
    private BossPatrol patrol;
    private BossAttack attack;
    private BossChange change;

    // 待機時間計測用
    private float idleTimer = 0f;

    private void Start()
    {
        // コンポーネントの取得とイベントの購読
        stats = GetComponent<BossStats>();
        stats.OnPhaseChanged += HandlePhaseChanged;

        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioPlayer>();
        patrol = GetComponent<BossPatrol>();
        attack = GetComponent<BossAttack>();
        change = GetComponent<BossChange>();
    }

    private void Update()
    {
        // 現在の状態に応じた処理
        switch (currentState)
        {
            case BossState.Idle:
                HandleIdle();
                break;
            case BossState.Patrol:
            case BossState.FastPatrol:
                DetectPlayer();
                break;
        }
    }

    // 状態を変更し、対応する処理を実行
    public void ChangeState(BossState s)
    {
        if (currentState == s) return;
        currentState = s;

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
                audioPlayer.Play((int)BossAudio.changed);
                animator.SetTrigger("ChangeTrigger");
                change.PhaseChange();
                AudioManager.Instance.ChangePlaySpeed(1.3f);
                break;
            case BossState.Hit:
                audioPlayer.Play((int)BossAudio.damaged);
                animator.SetTrigger("HitTrigger");
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
        }
    }

    // 攻撃準備後の次の攻撃を決定
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

    // フェーズ変更時の処理
    private void HandlePhaseChanged(BossPhase newPhase)
    {
        if (newPhase == BossPhase.Phase3)
        {
            ChangeState(BossState.Change);
        }
    }

    // 待機状態の処理
    private void HandleIdle()
    {
        idleTimer += Time.deltaTime;

        float waitTime = (stats.currentPhase == BossPhase.Phase3) ? 1f : 2f;

        if (idleTimer >= waitTime)
        {
            idleTimer = 0f;

            if (stats.currentPhase == BossPhase.Phase3) 
                ChangeState(BossState.FastPatrol);
            else 
                ChangeState(BossState.Patrol);
        }
    }

    // プレイヤーの検出
    private void DetectPlayer()
    {
        float dist = Vector3.Distance(patrol.player.position, transform.position);
        
        if (dist < 10f)
        {
            ChangeState(BossState.Attack_Prepare);
        }
    }
}