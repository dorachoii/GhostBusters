using UnityEngine;
using UnityEngine.AI;

public class BossPatrol : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] public Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Patrol(int speed)
    {
        agent.isStopped = false;
        agent.speed = speed;
        agent.SetDestination(player.position);
    }

    public void Stop()
    {
        agent.speed = 0;
        agent.isStopped = true;
    }
}
