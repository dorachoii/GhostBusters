using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public GameObject heartPrefab;

    public Transform[] heartSpawnPoints;

    [SerializeField] private PlayerStats playerStats;
    private bool heartSpawned = false;

    private void OnEnable()
    {
        playerStats.OnHPChanged += CheckForHeartSpawn;
    }


    public void SpawnCoin(Vector3 pos, Vector3 force)
    {
        GameObject coin = Instantiate(coinPrefab, pos, Quaternion.identity);
        Rigidbody rb = coin.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(force, ForceMode.Impulse);
        }
    }

    public void ExplodeCoins()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, Random.Range(-0.5f, 0.5f));
            Vector3 force = new Vector3(Random.Range(-1f, 1f), 5f, Random.Range(-1f, 1f));

            SpawnCoin(spawnPos, force);
        }
    }

    public void SpawnHeart()
    {
        if (heartSpawnPoints.Length == 0) return;
        int idx = Random.Range(0, heartSpawnPoints.Length);
        GameObject heart = Instantiate(heartPrefab, heartSpawnPoints[idx].position, Quaternion.identity);
    }

    
    public void CheckForHeartSpawn(int currentHP)
    {
        Debug.Log("HeartSpawn - occured");
        if (currentHP <= 50 && !heartSpawned)
        {
            Debug.Log("HeartSpawn");
            SpawnHeart();
            heartSpawned = true;
        }
    }
}
