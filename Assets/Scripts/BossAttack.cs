using UnityEngine;
using System.Collections;

public class BossAttack : MonoBehaviour
{
    public GameObject[] bossRocks;

    public Transform firePos;
    private float rockSpeed = 20f;
    private float delayBetweenShots = 0.5f;
    private bool isAttacking = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame

    public void Attack_3Balls(Transform target)
    {
        if (isAttacking) return;
        StartCoroutine(Fire3Shots(target));
    }

    void Fire(Vector3 dir)
    {
        GameObject rock = Instantiate(bossRocks[0], firePos.position, Quaternion.identity);
        rock.GetComponent<Rigidbody>().linearVelocity = dir * rockSpeed;
    }

    IEnumerator Fire3Shots(Transform target)
    {
        isAttacking = true;
        Vector3 dir = (target.position - firePos.position).normalized;

        Fire(dir);
        yield return new WaitForSeconds(delayBetweenShots);

        Fire(Quaternion.Euler(0, -15f, 0) * dir);
        yield return new WaitForSeconds(delayBetweenShots);

        Fire(Quaternion.Euler(0, 15f, 0) * dir);
        isAttacking = false;
    }
}
