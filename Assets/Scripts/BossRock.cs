using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// ボスが攻撃時に使用する岩のオブジェクトを管理し、プレイヤーやボスへのダメージを処理します。
/// </summary>

// 岩の種類を定義する列挙型
public enum RockType
{
    SmallBall, // 小さい岩
    BigBall,   // 大きい岩
    Ring       // リング状の岩
}

// 岩のプロパティを定義する構造体
public struct RockProperties
{
    public int damageToPlayer; // プレイヤーへのダメージ
    public int damageToBoss;   // ボスへのダメージ
    public float lifeTime;     // 生存時間

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
    private const int SmallBallDamageToPlayer = 20;
    private const int BigBallDamageToPlayer = 30;
    private const int BigBallDamageToBoss = 10;
    private const int RingDamageToBoss = 2;

    // 生存時間定数
    private const float SmallBallLifeTime = 15f;
    private const float BigBallLifeTime = 10f;
    private const float RingLifeTime = 30f;

    public RockType rockType; // 岩の種類
    private bool hasHit = false; // 衝突済みかどうか
    public static event Action<RockType> OnBigBallHit; // 大きい岩が衝突した時のイベント

    // 岩の種類ごとのプロパティを定義
    private static readonly Dictionary<RockType, RockProperties> rockDic = new Dictionary<RockType, RockProperties>
    {
        {RockType.SmallBall, new RockProperties(toPlayer: SmallBallDamageToPlayer, toBoss: 0, life: SmallBallLifeTime)},
        {RockType.BigBall, new RockProperties(toPlayer: BigBallDamageToPlayer, toBoss: BigBallDamageToBoss, life: BigBallLifeTime)},
        {RockType.Ring, new RockProperties(toPlayer: 0, toBoss: RingDamageToBoss, life: RingLifeTime)},
    };

    private float lifeTime = 15f; // 生存時間
    private GameObject boss; // ボスのゲームオブジェクト
    private Transform playerTransform; // プレイヤーのTransform
    private Transform bossTransform; // ボスのTransform
    private Rigidbody rb; // Rigidbodyコンポーネント

    public Vector3 dir; // 移動方向

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        // 岩の生存時間を設定し、Rigidbodyを取得
        DestroySelf(rockDic[rockType].lifeTime);
        rb = GetComponent<Rigidbody>();

        // ボスとプレイヤーのTransformを取得
        bossTransform = GameObject.FindWithTag("Boss").transform;
        playerTransform = GameObject.FindWithTag("Player").transform;
        dir = (playerTransform.position - bossTransform.transform.position).normalized;
    }

    private void Start()
    {
        // ボスのゲームオブジェクトを取得
        boss = GameObject.FindWithTag("Boss");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;

        // プレイヤーとの衝突処理
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

        // ボスとの衝突処理
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

    // 自身を破壊する
    private void DestroySelf(float lifeTime = 0)
    {
        switch (rockType)
        {
            case RockType.SmallBall: break;
            case RockType.BigBall:
                OnBigBallHit?.Invoke(rockType);
                break;
            case RockType.Ring: break;
        }

        Destroy(gameObject, lifeTime);
    }
}
