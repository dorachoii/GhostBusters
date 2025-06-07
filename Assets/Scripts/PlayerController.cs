using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem.Interactions;

/// <summary>
/// プレイヤーの状態管理と制御を行うクラスです。
/// 状態パターンを使用して、プレイヤーの様々な状態（待機、移動、攻撃など）を管理します。
/// 
/// 現在の実装:
/// - 基本的な状態（待機、移動、吸い込み、吹き飛ばし、被ダメージ、回復）を実装
/// - 各状態は独立したクラスとして実装され、明確な責任を持つ
/// - 状態の遷移はStateMachineクラスで一元管理
/// 
/// 今後の拡張計画:
/// - 移動関連: ジャンプ、ダッシュ、回避などの状態追加
/// - 戦闘関連: 特殊攻撃、コンボ攻撃などの状態追加
/// - インタラクション: アイテム使用、環境との相互作用などの状態追加
/// - アニメーション: より複雑なアニメーション遷移の実装
/// 
/// 注意: 現在はBOSSがswitch文で実装されているのに対し、プレイヤーは状態パターンを使用しています。
/// これは各キャラクターの特性と複雑さに基づく実装の違いです。
/// 将来的にはBOSSの実装も状態パターンに移行する可能性があります。
/// </summary>

// 状態の基本インターフェース
public interface IState
{
    void Enter();    // 状態開始時の処理
    void Execute();  // 状態実行中の処理
    void Exit();     // 状態終了時の処理
}

/// <summary>
/// 待機状態を管理するクラス
/// </summary>
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

/// <summary>
/// 移動状態を管理するクラス
/// </summary>
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

/// <summary>
/// 吹き飛ばし攻撃状態を管理するクラス
/// </summary>
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

/// <summary>
/// 吸い込み攻撃状態を管理するクラス
/// </summary>
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

/// <summary>
/// 被ダメージ状態を管理するクラス
/// </summary>
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

/// <summary>
/// 回復状態を管理するクラス
/// </summary>
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
/// 状態遷移を管理するステートマシンクラス
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

    /// <summary>
    /// 初期状態を設定します
    /// </summary>
    public void Initialize(IState startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
    }

    /// <summary>
    /// 別の状態に遷移します
    /// </summary>
    /// <param name="nextState">次の状態</param>
    /// <param name="force">強制的に遷移するかどうか</param>
    public void TransitionTo(IState nextState, bool force = false)
    {
        if (!force && CurrentState == nextState) return;
        CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();
    }

    /// <summary>
    /// 現在の状態の実行処理を呼び出します
    /// </summary>
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

    // ステートマシンへのアクセス用プロパティ
    public StateMachine StateMachine => stateMachine;

    private void Awake()
    {
        // 必要なコンポーネントを取得
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
