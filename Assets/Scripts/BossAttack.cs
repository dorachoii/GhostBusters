using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;

public class BossAttack : MonoBehaviour
{
    public GameObject[] bossRocks;
    public GameObject bossBreathFX;
    private GameObject currentBreath;

    public Transform firePos;
    public Transform bigBallPos;
    private float rockSpeed = 20f;
    private float delayBetweenShots = 0.5f;
    private bool isAttacking = false;

    float blowPower = 4.5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Attack_BigBalls(GameObject.FindGameObjectWithTag("Player").transform);
        }
    }

    // Update is called once per frame

    public void Attack_3Balls(Transform target)
    {
        if (isAttacking) return;
        StartCoroutine(IFire3Shot(target));
    }

    void FireSmallStone(Vector3 dir)
    {
        GameObject rock = Instantiate(bossRocks[0], firePos.position, Quaternion.identity);
        rock.GetComponent<Rigidbody>().linearVelocity = dir * rockSpeed;
    }



    IEnumerator IFire3Shot(Transform target)
    {
        isAttacking = true;
        Vector3 dir = (target.position - firePos.position).normalized;

        FireSmallStone(dir);
        yield return new WaitForSeconds(delayBetweenShots);

        FireSmallStone(Quaternion.Euler(0, -15f, 0) * dir);
        yield return new WaitForSeconds(delayBetweenShots);

        FireSmallStone(Quaternion.Euler(0, 15f, 0) * dir);
        isAttacking = false;
    }

    public void Attack_BigBalls(Transform target)
    {
        GameObject rock = Instantiate(bossRocks[1], bigBallPos.position + bigBallPos.forward * 1.3f, Quaternion.identity);
        currentBreath = Instantiate(bossBreathFX, bigBallPos.position, Quaternion.identity, bigBallPos);
        currentBreath.transform.forward = transform.forward;

        Vector3 dir = rock.GetComponent<BossRock>().dir;

        rock.GetComponent<Rigidbody>().AddForce(dir * blowPower, ForceMode.Impulse);
    }

    public void DestroyBreath()
    {
        if (currentBreath != null)
        {
            Destroy(currentBreath);
            currentBreath = null;
        }
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

