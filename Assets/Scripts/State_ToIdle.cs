using UnityEngine;
/// <summary>
/// アニメーション状態が終了したときに呼び出されるスクリプトです。
/// すべての攻撃や行動が終わった後、ボスをIdle状態に戻す役割をします。
/// </summary>
public class State_ToIdle : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var boss = animator.GetComponent<BossContoller>();
        boss.ChangeState(BossState.Idle);
    }
}
