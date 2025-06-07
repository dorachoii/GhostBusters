using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ボスがプレイヤーを追跡するためのパトロール機能を提供します。
/// </summary>

public class BossPatrol : MonoBehaviour
{
    // NavMeshAgentコンポーネント
    private NavMeshAgent agent;
    // プレイヤーのTransform
    [SerializeField] public Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // NavMeshAgentコンポーネントを取得
        agent = GetComponent<NavMeshAgent>();
    }

    // 指定された速度でプレイヤーを追跡
    public void Patrol(int speed)
    {
        agent.isStopped = false;
        agent.speed = speed;
        agent.SetDestination(player.position);
    }

    // パトロールを停止
    public void Stop()
    {
        agent.speed = 0;
        agent.isStopped = true;
    }
}
