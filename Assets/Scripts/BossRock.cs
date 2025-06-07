using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// ボスの攻撃用の岩を制御するコンポーネントです。
/// 岩の種類に応じて異なる動作をします。
/// </summary>

public enum RockType
{
    SmallRock, 
    BigRock,  
    Ring      
}

// 岩のプロパティを定義する構造体
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
    // ダメージ定数
    private const int SmallRockDamageToPlayer = 20;
    private const int BigRockDamageToPlayer = 30;
    private const int BigRockDamageToBoss = 10;
    private const int RingDamageToBoss = 2;

    // 生存時間定数
    private const float SmallRockLifeTime = 15f;
    private const float BigRockLifeTime = 10f;
    private const float RingLifeTime = 30f;

    public RockType rockType; 
    private bool hasHit = false; 
    // big rock 衝突時のイベント
    public static event Action<RockType> OnBigRockHit; 

    // rockごとのプロパティを定義
    private static readonly Dictionary<RockType, RockProperties> rockDic = new Dictionary<RockType, RockProperties>
    {
        {RockType.SmallRock, new RockProperties(toPlayer: SmallRockDamageToPlayer, toBoss: 0, life: SmallRockLifeTime)},
        {RockType.BigRock, new RockProperties(toPlayer: BigRockDamageToPlayer, toBoss: BigRockDamageToBoss, life: BigRockLifeTime)},
        {RockType.Ring, new RockProperties(toPlayer: 0, toBoss: RingDamageToBoss, life: RingLifeTime)},
    };

    private float lifeTime = 15f; 
    private GameObject boss; 
    private Transform playerTransform; 
    private Transform bossTransform; 
    private Rigidbody rb; 

    public Vector3 dir; 

    
    private void Awake()
    {
        DestroySelf(rockDic[rockType].lifeTime);
        rb = GetComponent<Rigidbody>();

        bossTransform = GameObject.FindWithTag("Boss").transform;
        playerTransform = GameObject.FindWithTag("Player").transform;
        // 生成時のボスとプレイヤー間の方向を計算し、その方向に移動するように設定
        dir = (playerTransform.position - bossTransform.transform.position).normalized;
    }

    private void Start()
    {
        boss = GameObject.FindWithTag("Boss");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;

        // Playerとの衝突処理
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

        // Bossとの衝突処理
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

    // 自身破壊
    private void DestroySelf(float lifeTime = 0)
    {
        switch (rockType)
        {
            case RockType.SmallRock: break;
            case RockType.BigRock:
                OnBigRockHit?.Invoke(rockType);
                break;
            case RockType.Ring: break;
        }

        Destroy(gameObject, lifeTime);
    }
}
