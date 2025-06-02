using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem.Interactions;

// 현재 상태: PlayerStateMachine이 animator로서의 역할만 하는 중
// idle, move(bool event)와 heal, hit (trigger event) 구분해서 짜는 게 어려움.
// 우선, 연속 공격을 맞을 때를 대비해, transitionTo함수에 force변수 하나 추가가

public interface IState
{
    public void Enter()
    {

    }
    public void Execute()
    {

    }

    public void Exit()
    {

    }
}

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
    public void Execute()
    {

    }
    public void Exit()
    {

    }
}

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
    public void Execute()
    {

    }
    public void Exit()
    {
        
    }
}

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
        //Debug.Log("Blowing Enter");
    }
    public void Execute()
    {

    }
    public void Exit()
    {
        player.animator.SetBool("IsBlowing", false);
        //Debug.Log("Blowing Exit");
    }
}

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
        //Debug.Log("Sucking Enter");
    }
    public void Execute()
    {

    }
    public void Exit()
    {
        player.animator.SetBool("IsSucking", false);
        //Debug.Log("Sucking Exit");
    }
}

public class HitState : IState
{
    private PlayerController player;

    public HitState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.GetComponentInChildren<FlickerEffectHandler>().PlayFlicker(FlickerType.Hit);
    }

    public void Execute()
    {

    }
    public void Exit()
    {
        
    }
}

public class HealState : IState
{
    private PlayerController player;

    public HealState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.GetComponentInChildren<FlickerEffectHandler>().PlayFlicker(FlickerType.Heal);
    }

    public void Execute()
    {

    }
    public void Exit()
    {
        
    }
}

public class StateMachine
{
    public IState CurrentState { get; private set; }

    public IdleState idleState;
    public WalkState walkState;
    public BlowState blowState;
    public SuckState suckState;
    public HitState hitState;
    public HealState healState;

    public StateMachine(PlayerController player)
    {
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

    public void TransitionTo(IState nextState, bool force = false)
    {
        if (!force && CurrentState == nextState) return;
        CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();
    }

    public void Execute()
    {
        if (CurrentState != null)
        {
            CurrentState.Execute();
        }
    }

}

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    private StateMachine stateMachine;

    public StateMachine StateMachine => stateMachine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        stateMachine = new StateMachine(this);
        stateMachine.Initialize(stateMachine.idleState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Execute();
    }
}
