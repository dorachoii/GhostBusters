using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using SBS.ME;

/// <summary>
/// プレイヤーの攻撃（吸い込み/吹き飛ばし）機能を制御します。
/// 物理演算を使用するため、FixedUpdateで処理を行います。
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    // コンポーネント参照
    private PlayerController controller;  // プレイヤーの制御
    private PlayerStats stats;           // プレイヤーのステータス

    // 入力設定
    [SerializeField] private InputAction attack;  // 攻撃入力

    // 吸い込み/吹き飛ばしの設定
    private const float SuckRange = 8f;    // 効果範囲
    private const float SuckPower = 5f;    // 吸い込み/吹き飛ばしの力
    private const float SuckAngle = 60f;   // 効果角度

    // エフェクト関連
    public Transform hand;                 // エフェクトの生成位置
    public GameObject[] FX;                // エフェクトのプレハブ
    private bool isFXInstantiated = false; // エフェクトが生成済みかどうか

    private void OnEnable()
    {
        attack.Enable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // 必要なコンポーネントを取得
        controller = GetComponent<PlayerController>();
        stats = GetComponent<PlayerStats>();
    }

    /// <summary>
    /// 物理演算を使用するため、FixedUpdateで処理を行います。
    /// これにより、物理演算の安定性と一貫性が保たれます。
    /// </summary>
    private void FixedUpdate()
    {
        Attack();
    }

    /// <summary>
    /// 入力に応じて吸い込み/吹き飛ばしを実行します。
    /// </summary>
    private void Attack()
    {
        float direction = attack.ReadValue<float>();
        if (attack.IsPressed())
        {
            if (direction < 0) Suck();
            else Blow();
        }
        else
        {
            OffFX();
            controller.StateMachine.TransitionTo(controller.StateMachine.idleState);
        }
    }

    /// <summary>
    /// アイテムを吸い込む処理を実行します。
    /// </summary>
    private void Suck()
    {
        controller.StateMachine.TransitionTo(controller.StateMachine.suckState);
        ApplyVacuum(1);
        OnFX(0);
    }

    /// <summary>
    /// アイテムを吹き飛ばす処理を実行します。
    /// </summary>
    private void Blow()
    {
        controller.StateMachine.TransitionTo(controller.StateMachine.blowState);
        ReleaseItems();
        ApplyVacuum(-1);
        OnFX(1);
    }

    /// <summary>
    /// 指定されたエフェクトを生成します。
    /// </summary>
    private void OnFX(int idx)
    {
        if (!isFXInstantiated)
        {
            if (idx == 0)
            {
                Instantiate(FX[idx], hand.position + hand.transform.forward * 2, hand.rotation, hand);
            }
            else
            {
                Instantiate(FX[idx], hand.position, hand.rotation, hand);
            }
            isFXInstantiated = true;
        }
    }

    /// <summary>
    /// 生成されたエフェクトを削除します。
    /// </summary>
    private void OffFX()
    {
        foreach (Transform child in hand)
        {
            Destroy(child.gameObject);
        }
        isFXInstantiated = false;
    }

    /// <summary>
    /// 指定された方向に物理的な力を適用します。
    /// </summary>
    private void ApplyVacuum(float direction)
    {
        // 効果範囲内のオブジェクトを検出
        Collider[] targets = Physics.OverlapSphere(transform.position, SuckRange, LayerMask.GetMask("Item", "BossRock"));
        
        Vector3 forward = transform.forward;
        float halfFOV = SuckAngle * 0.5f;

        foreach (var target in targets)
        {
            if (target.attachedRigidbody != null)
            {
                Vector3 dir = (transform.position - target.transform.position).normalized;
                float angle = Vector3.Angle(forward, dir);
                float distance = Vector3.Distance(transform.position, target.transform.position);

                // 十分に近づいた場合、アイテムを収納
                if (distance < 0.7f)
                {
                    if (!target.attachedRigidbody.isKinematic)
                    {
                        StoreItem(target);
                    }
                    continue;
                }

                // 視野角外のオブジェクトは無視
                if (angle < halfFOV) continue;

                // 距離に応じて力を減衰
                float power = Mathf.Lerp(SuckPower, 0, distance / SuckRange);

                // ボスの岩の場合は特別な処理
                if (target.gameObject.layer == LayerMask.NameToLayer("BossRock") && target.gameObject.CompareTag("BigBall"))
                {
                    dir = target.gameObject.GetComponent<BossRock>().dir;
                }

                // 物理的な力を適用
                target.attachedRigidbody.AddForce(direction * dir * power, ForceMode.Impulse);

                // 建物の場合は爆発処理
                if (target.gameObject.layer == LayerMask.NameToLayer("Item") && target.gameObject.CompareTag("Building"))
                {
                    target.GetComponent<MeshExploder>().enabled = true;
                    target.GetComponent<MeshExploder>().EXPLODE();
                }
            }
        }
    }

    /// <summary>
    /// アイテムを収納します。
    /// </summary>
    private void StoreItem(Collider target)
    {
        target.attachedRigidbody.linearVelocity = Vector3.zero;

        if (target.gameObject.CompareTag("Ring"))
        {
            // リングの場合は所持数制限をチェック
            if (transform.GetChild(1).childCount > stats.maxItem) return;
            target.attachedRigidbody.isKinematic = true;
            target.transform.SetParent(transform.GetChild(1), true);
            stats.GainItem();
        }
        else if (target.gameObject.CompareTag("Heart"))
        {
            // 回復アイテムの場合は即座に効果を適用
            target.gameObject.GetComponent<HealItem>().Heal(stats);
        }
    }

    /// <summary>
    /// 収納したアイテムを放出します。
    /// </summary>
    private void ReleaseItems()
    {
        Transform pocket = transform.GetChild(1);
        Vector3 forward = transform.forward;

        // 放出するアイテムのリストを作成
        List<Transform> itemsToRelease = new List<Transform>();
        foreach (Transform child in pocket)
        {
            itemsToRelease.Add(child);
        }

        // アイテムを放出
        foreach (Transform child in itemsToRelease)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb != null)
            {
                child.SetParent(null);
                child.position += forward * 0.5f;
                rb.isKinematic = false;
                rb.AddForce(forward * 10f + Vector3.up * 5f, ForceMode.Impulse);
                stats.LoseItem();
            }
        }
    }

    /// <summary>
    /// エディタ上で効果範囲と視野角を可視化します。
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // 効果範囲の表示
        Gizmos.color = Color.darkRed;
        Gizmos.DrawWireSphere(transform.position, SuckRange);

        // 視野角の表示
        Vector3 forward = transform.forward;
        float halfFOV = SuckAngle * 0.5f;

        Quaternion leftRotation = Quaternion.Euler(0, -halfFOV, 0);
        Quaternion rightRotation = Quaternion.Euler(0, halfFOV, 0);

        Vector3 leftDir = leftRotation * forward;
        Vector3 rightDir = rightRotation * forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, leftDir * SuckRange);
        Gizmos.DrawRay(transform.position, rightDir * SuckRange);
    }
}
