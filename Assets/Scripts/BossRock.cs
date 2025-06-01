using Unity.Cinemachine;
using UnityEngine;

public class BossRock : MonoBehaviour
{
    private int damage = 20;
    public float lifeTime = 3f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damage = gameObject.tag == "SmallBall" ? 20 : 30;
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerStats stats = collision.gameObject.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
