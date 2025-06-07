using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// ボスの攻撃パターン（小さい岩、大きい岩、回転攻撃など）を管理し、実行するコンポーネントです。
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

    // 攻撃用の岩のプレハブ
    public GameObject[] bossRocks;
    // 攻撃時のエフェクト
    public GameObject bossBreathFX;

    // 攻撃発射位置
    public Transform firePos;
    public Transform bigBallPos;
    // 攻撃中フラグ
    private bool isAttacking = false;

    private void OnEnable()
    {
        // 大きい岩のヒットイベントを購読
        BossRock.OnBigBallHit += HandleBigBallHit;
    }

    private void OnDisable()
    {
        // イベントの購読解除
        BossRock.OnBigBallHit -= HandleBigBallHit;
    }

    // 大きい岩のヒット時の処理
    private void HandleBigBallHit(RockType rockType)
    {
        if (rockType == RockType.BigBall)
        {
            SetBreathActive(false);
        }
    }

    // 小さい岩を複数発射する攻撃
    public void Attack_SmallBalls(Transform target, int cnt,
        float delay = SmallRockDelay,
        int angleStep = SmallRockAngleStep)
    {
        if (isAttacking) return;
        StartCoroutine(IFireNShot(target, cnt, delay, angleStep));
    }

    // 小さい岩を1発発射
    void FireSmallBall(Vector3 dir, float rockSpeed = SmallRockSpeed)
    {
        GameObject rock = Instantiate(bossRocks[0], firePos.position, Quaternion.identity);
        rock.GetComponent<Rigidbody>().linearVelocity = dir * rockSpeed;
    }

    // 複数の小さい岩を順番に発射するコルーチン
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

    // 回転しながら攻撃する
    public void Attack_Spin(int spinCount = 1, int bulletCount = 12, float duration = 3)
    {
        StartCoroutine(SpinAndFire(spinCount, bulletCount, duration));
    }

    // 回転攻撃のコルーチン
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

    // 大きい岩を発射する攻撃
    public void Attack_BigBalls(Transform target)
    {
        GameObject rock = Instantiate(bossRocks[1], bigBallPos.position + bigBallPos.forward * 1.3f, Quaternion.identity);
        SetBreathActive(true);

        Vector3 dir = rock.GetComponent<BossRock>().dir;

        rock.GetComponent<Rigidbody>().AddForce(dir * BigRockBlowPower, ForceMode.Impulse);
    }

    // 攻撃エフェクトの表示/非表示
    public void SetBreathActive(bool active)
    {
        bossBreathFX.SetActive(active);
    }

    // ターゲットを滑らかに見る
    public void StartSmoothLookAt(Transform target)
    {
        StartCoroutine(SmoothLookAtCoroutine(target));
    }

    // 滑らかにターゲットを見るコルーチン
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

