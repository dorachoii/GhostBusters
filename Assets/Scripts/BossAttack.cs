using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// ボスの攻撃パターンを管理、実行するコンポーネントです。
/// 攻撃１：Small Ball 
/// 攻撃２：spin & fire 
/// 攻撃３：bigball 
/// </summary>

public class BossAttack : MonoBehaviour
{
    // 攻撃パラメータ
    private const float BigRockBlowPower = 4.5f;
    private const float SmallRockSpeed = 20f;
    private const float SmallRockDelay = 0.5f;
    private const int SmallRockAngleStep = 15;
    private const float SpinRockSpeed = 30f;
    private const float SmoothRotationSpeed = 5f;

    // 攻撃プレハブ
    public GameObject[] bossRocks;
    public GameObject bossBreathFX;

    // 攻撃発射位置
    public Transform firePos;
    public Transform bigBallPos;
    // 攻撃中フラグ
    private bool isAttacking = false;

    private void OnEnable()
    {
        BossRock.OnBigRockHit += HandleBigBallHit;
    }

    private void OnDisable()
    {
        BossRock.OnBigRockHit -= HandleBigBallHit;
    }

    // BigBallHitの場合非表示
    private void HandleBigBallHit(RockType rockType)
    {
        if (rockType == RockType.BigRock)
        {
            SetBreathActive(false);
        }
    }

    // 攻撃１：Small Ball
    public void Attack_SmallBalls(Transform target, int cnt,
        float delay = SmallRockDelay,
        int angleStep = SmallRockAngleStep)
    {
        if (isAttacking) return;
        StartCoroutine(IFireNShot(target, cnt, delay, angleStep));
    }

    void FireSmallBall(Vector3 dir, float rockSpeed = SmallRockSpeed)
    {
        GameObject rock = Instantiate(bossRocks[0], firePos.position, Quaternion.identity);
        rock.GetComponent<Rigidbody>().linearVelocity = dir * rockSpeed;
    }

    IEnumerator IFireNShot(Transform target, int cnt, float delay, int angleStep)
    {
        isAttacking = true;
        Vector3 baseDir = (target.position - firePos.position).normalized;

        float totalSpread = angleStep * (cnt - 1);
        float startAngle = -totalSpread / 2f;

        for (int i = 0; i < cnt; i++)
        {
            float angle = startAngle + (angleStep * i);
            Vector3 dir = Quaternion.Euler(0, angle, 0) * baseDir;

            FireSmallBall(dir);
            yield return new WaitForSeconds(delay);
        }
        isAttacking = false;
    }

    // 攻撃２：spin & fire
    public void Attack_Spin(int spinCount = 1, int bulletCount = 12, float duration = 3)
    {
        StartCoroutine(SpinAndFire(spinCount, bulletCount, duration));
    }

    IEnumerator SpinAndFire(int spinCount, int bulletCount, float duration)
    {
        isAttacking = true;

        float elapsed = 0f;
        float fireInterval = duration / bulletCount;
        float rotationSpeed = 360f * spinCount / duration;

        int firedCount = 0;

        while (elapsed < duration)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            if (elapsed >= firedCount * fireInterval)
            {
                FireSmallBall(transform.forward - transform.up * 0.1f, SpinRockSpeed);
                firedCount++;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        isAttacking = false;
    }

    // 攻撃3：bigball
    public void Attack_BigBalls(Transform target)
    {
        GameObject rock = Instantiate(bossRocks[1], bigBallPos.position + bigBallPos.forward * 1.3f, Quaternion.identity);
        SetBreathActive(true);

        Vector3 dir = rock.GetComponent<BossRock>().dir;

        rock.GetComponent<Rigidbody>().AddForce(dir * BigRockBlowPower, ForceMode.Impulse);
    }

    // 以外１：攻撃エフェクトの表示/非表示切り替え
    public void SetBreathActive(bool active)
    {
        bossBreathFX.SetActive(active);
    }


    /// ターゲットを滑らかに見る処理を開始します
    public void StartSmoothLookAt(Transform target)
    {
        StartCoroutine(SmoothLookAtCoroutine(target));
    }

    IEnumerator SmoothLookAtCoroutine(Transform target)
    {
        while (true)
        {
            Vector3 dir = target.position - transform.position;

            Quaternion targetRot = Quaternion.LookRotation(dir.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot,
                Time.deltaTime * SmoothRotationSpeed);
            yield return null;
        }
    }
}

