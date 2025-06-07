using UnityEngine;

/// <summary>
/// Animation状態が終了したときに呼び出されるスクリプトです。
/// AttackState -> IdleState

public class State_ToIdle : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var boss = animator.GetComponent<BossContoller>();
        boss.ChangeState(BossState.Idle);
    }
}
