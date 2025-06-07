using UnityEngine;

/// <summary>
/// アニメーション状態が終了したときに呼び出されるスクリプトです。
/// 準備動作が終わった後、次の攻撃パターンを選択して実行する役割をします。
/// </summary>
public class State_Prepare : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var boss = animator.GetComponent<BossContoller>();
        boss.DecideAttackAfterPrepare();
    }
}

