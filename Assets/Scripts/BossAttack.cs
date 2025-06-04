using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;

public class BossAttack : MonoBehaviour
{
    public GameObject[] bossRocks;
    public GameObject bossBreathFX;

    public Transform firePos;
    public Transform bigBallPos;
    private bool isAttacking = false;

    float blowPower = 4.5f;

    private void OnEnable()
    {
        BossRock.OnBigBallHit += HandleBigBallHit;
    }

    private void OnDisable()
    {
        BossRock.OnBigBallHit -= HandleBigBallHit;
    }

    private void HandleBigBallHit(RockType rockType)
    {
        if (rockType == RockType.BigBall)
        {
            SetBreathActive(false);
        }
    }

    public void Attack_SmallBalls(Transform target, int cnt, float delay = 0.5f, int angleStep = 15)
    {
        if (isAttacking) return;
        StartCoroutine(IFireNShot(target, cnt, delay, angleStep));
    }

    void FireSmallBall(Vector3 dir, float rockSpeed = 20f)
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
                FireSmallBall(transform.forward - transform.up*0.1f, 30);
                firedCount++;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        isAttacking = false;
    }
    

    public void Attack_BigBalls(Transform target)
    {
        GameObject rock = Instantiate(bossRocks[1], bigBallPos.position + bigBallPos.forward * 1.3f, Quaternion.identity);
        SetBreathActive(true);

        Vector3 dir = rock.GetComponent<BossRock>().dir;

        rock.GetComponent<Rigidbody>().AddForce(dir * blowPower, ForceMode.Impulse);
    }

    public void SetBreathActive(bool active)
    {
        bossBreathFX.SetActive(active);
    }

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
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);
            yield return null;
        }
    }
}

