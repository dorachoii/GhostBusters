using UnityEngine;

public class State_Prepare : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var boss = animator.GetComponent<BossContoller>();
        boss.DecideAttackAfterPrepare();
    }
}

