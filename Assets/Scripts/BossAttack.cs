using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;

public class BossAttack : MonoBehaviour
{
    public GameObject[] bossRocks;
    public GameObject bossBreathFX;

    public Transform firePos;
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
        StartCoroutine (IFire3Shot(target));
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
        GameObject rock = Instantiate(bossRocks[1], firePos.position + Vector3.down * 0.8f, Quaternion.identity);
        GameObject breath = Instantiate(bossBreathFX, firePos.position, Quaternion.identity);
        breath.transform.forward = transform.forward;

        Vector3 dir = rock.transform.position - gameObject.transform.position;

        rock.GetComponent<Rigidbody>().AddForce(dir * blowPower, ForceMode.Impulse);
    }
}
