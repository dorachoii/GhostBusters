using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem.Interactions;

/// <summary>
/// Implements a simple state machine pattern for player behavior management.
///
/// The player can be in one of four states:
/// - Idle: Default resting state
/// - Walk: Movement state
/// - Suck: Suction action 
/// - Blow: Blowing action 
///
/// Each state controls corresponding animation parameters and can be transitioned through the StateMachine.
/// </summary>
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
    private float hitDuration = 0.5f;
    private float timer = 0f;

    public HitState(PlayerController player)
    {
        this.player = player;
    }

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

public class StateMachine
{
    public IState CurrentState { get; private set; }

    public IdleState idleState;
    public WalkState walkState;
    public BlowState blowState;
    public SuckState suckState;

    public StateMachine(PlayerController player)
    {
        this.idleState = new IdleState(player);
        this.walkState = new WalkState(player);
        this.blowState = new BlowState(player);
        this.suckState = new SuckState(player);
    }

    public void Initialize(IState startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
    }

    public void TransitionTo(IState nextState)
    {
        if (CurrentState == nextState) return;
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
