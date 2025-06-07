using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem.Interactions;

/// <summary>
/// プレイヤーの状態管理と制御を行うクラスです。
/// </summary>

// 状態の基本Interface
public interface IState
{
    void Enter();    // 開始時の処理
    void Execute();  // 実行中の処理
    void Exit();     // 終了時の処理
}


/// 待機状態
public class IdleState : IState
{
    private PlayerController player;

    public IdleState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.animator.SetBool("IsMoving", false);
    }

    public void Execute() { }
    public void Exit() { }
}

/// 移動状態
public class WalkState : IState
{
    private PlayerController player;

    public WalkState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.animator.SetBool("IsMoving", true);
    }

    public void Execute() { }
    public void Exit() { }
}

/// 吹き飛ばし攻撃
public class BlowState : IState
{
    private PlayerController player;

    public BlowState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.animator.SetBool("IsBlowing", true);
        player.audioPlayer.Play((int)PlayerAudio.blow);
    }

    public void Execute() { }

    public void Exit()
    {
        player.animator.SetBool("IsBlowing", false);
    }
}

/// 吸い込み攻撃
public class SuckState : IState
{
    private PlayerController player;

    public SuckState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.animator.SetBool("IsSucking", true);
        player.audioPlayer.Play((int)PlayerAudio.suck);
    }

    public void Execute() { }

    public void Exit()
    {
        player.animator.SetBool("IsSucking", false);
    }
}

/// 被ダメージ
public class HitState : IState
{
    private PlayerController player;

    public HitState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.GetComponentInChildren<PlayerBlink>().PlayFlicker(FlickerType.Hit);
        player.audioPlayer.Play((int)PlayerAudio.damaged);
    }

    public void Execute() { }
    public void Exit() { }
}


/// 回復
public class HealState : IState
{
    private PlayerController player;

    public HealState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.GetComponentInChildren<PlayerBlink>().PlayFlicker(FlickerType.Heal);
        player.audioPlayer.Play((int)PlayerAudio.healed);
    }

    public void Execute() { }
    public void Exit() { }
}

/// <summary>
/// 状態遷移を管理するStateMachine Class
/// </summary>
public class StateMachine
{
    public IState CurrentState { get; private set; }

    // 各状態のインスタンス
    public IdleState idleState;
    public WalkState walkState;
    public BlowState blowState;
    public SuckState suckState;
    public HitState hitState;
    public HealState healState;

    public StateMachine(PlayerController player)
    {
        // 各状態のインスタンスを初期化
        this.idleState = new IdleState(player);
        this.walkState = new WalkState(player);
        this.blowState = new BlowState(player);
        this.suckState = new SuckState(player);
        this.hitState = new HitState(player);
        this.healState = new HealState(player);
    }

    public void Initialize(IState startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
    }

    /// 別の状態に遷移します
    public void TransitionTo(IState nextState, bool force = false)
    {
        if (!force && CurrentState == nextState) return;
        CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();
    }

    /// 現在の状態の実行処理を呼び出します
    public void Execute()
    {
        if (CurrentState != null)
        {
            CurrentState.Execute();
        }
    }
}

/// <summary>
/// プレイヤーの制御を担当するメインクラス
/// </summary>
public class PlayerController : MonoBehaviour
{
    // コンポーネント参照
    public AudioPlayer audioPlayer;
    public Animator animator;
    private StateMachine stateMachine;

    // StateMachineへのaccess用プロパティ
    public StateMachine StateMachine => stateMachine;

    private void Awake()
    {
        audioPlayer = gameObject.GetComponent<AudioPlayer>();
        animator = gameObject.GetComponent<Animator>();
        stateMachine = new StateMachine(this);
        stateMachine.Initialize(stateMachine.idleState);
    }

    private void Update()
    {
        // 現在の状態の実行処理を呼び出し
        stateMachine.Execute();
    }
}
