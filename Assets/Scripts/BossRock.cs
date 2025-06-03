using Unity.Cinemachine;
using UnityEngine;

public class BossRock : MonoBehaviour
{
    private int damage = 20;
    private float lifeTime = 15f;
    private Transform bossTransform;
    private Transform playerTransform;
    private Rigidbody rb;

    public Vector3 dir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        damage = gameObject.tag == "SmallBall" ? 20 : 30;
        Destroy(gameObject, lifeTime);

        rb = GetComponent<Rigidbody>();

        bossTransform = GameObject.FindWithTag("Boss").transform;
        playerTransform = GameObject.FindWithTag("Player").transform;
        dir = (playerTransform.position - bossTransform.position).normalized;
    }

    // TODO: 
    // 이 락이 생성될 때, 보스와 플레이어를 이은 일직선 범위로만 이동하고 싶어.
    // 이동 범위를 일직선으로 한정하고 싶은 거지. 무슨 말인지 알지?



    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameObject player = collision.gameObject;

            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            BossStats boss = collision.gameObject.GetComponent<BossStats>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
    
}
