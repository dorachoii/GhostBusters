using UnityEngine;

/// <summary>
/// Animation状態が終了したときに呼び出されるスクリプトです。
/// LookState -> AttackState
/// </summary>

public class State_Prepare : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var boss = animator.GetComponent<BossContoller>();
        boss.DecideAttackAfterPrepare();
    }
}

