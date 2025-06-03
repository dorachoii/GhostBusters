using UnityEngine;

public class PrepareState : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var boss = animator.GetComponent<BossContoller>();
        //boss.ChangeState(BossState.Attack_3Balls);
        boss.DecideAttackAfterPrepare();
    }
}

