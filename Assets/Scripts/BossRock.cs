using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum RockType
{
    SmallBall,
    BigBall,
    Ring
}

public struct RockProperties
{
    public int damageToPlayer;
    public int damageToBoss;
    public float lifeTime;

    public RockProperties(int toPlayer, int toBoss, float life)
    {
        damageToPlayer = toPlayer;
        damageToBoss = toBoss;
        lifeTime = life;
    }
}

public class BossRock : MonoBehaviour
{
    public RockType rockType;
    private bool hasHit = false;

    private static readonly Dictionary<RockType, RockProperties> rockDic = new Dictionary<RockType, RockProperties>
    {
        {RockType.SmallBall, new RockProperties(toPlayer: 20, toBoss: 0, life: 15f)},
        {RockType.BigBall, new RockProperties(toPlayer: 30, toBoss: 10, life: 10f)},
        {RockType.Ring, new RockProperties(toPlayer: 0, toBoss: 2, life: 30f)},
    };

    private float lifeTime = 15f;
    private GameObject boss;
    private Transform playerTransform;
    private Rigidbody rb;

    public Vector3 dir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        DestroySelf(rockDic[rockType].lifeTime);
        rb = GetComponent<Rigidbody>();

        boss = GameObject.FindWithTag("Boss");
        playerTransform = GameObject.FindWithTag("Player").transform;
        dir = (playerTransform.position - boss.transform.position).normalized;

    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                int damageToPlayer = rockDic[rockType].damageToPlayer;
                playerStats.TakeDamage(damageToPlayer);
            }
            hasHit = true;
            DestroySelf();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            BossStats boss = collision.gameObject.GetComponent<BossStats>();
            if (boss != null)
            {
                int damageToBoss = rockDic[rockType].damageToBoss;
                boss.TakeDamage(damageToBoss);
            }
            hasHit = true;
            DestroySelf();
        }
    }

    void DestroySelf(float lifeTime = 0)
    {
        switch (rockType)
        {
            case RockType.SmallBall: break;
            case RockType.BigBall:
            case RockType.Ring: break;
        }

        Destroy(gameObject, lifeTime);
    }

}
