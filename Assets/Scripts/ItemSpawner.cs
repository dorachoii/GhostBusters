using UnityEngine;

/// <summary>
/// アイテムの生成を管理するクラスです。
/// 役割１：建物が破壊された時にコインを生成
/// 役割２：playerのHPが低下した時に回復アイテムを生成します。
/// </summary>

public class ItemSpawner : MonoBehaviour
{
    // 生成するアイテムのプレハブ
    [SerializeField] private GameObject coinPrefab;   
    [SerializeField] private GameObject heartPrefab; 

    // 回復アイテムの生成位置
    [SerializeField] private Transform[] heartSpawnPoints;  

    // player status参照
    [SerializeField] private PlayerStats playerStats;     

    // 回復アイテム生成済みcheckフラグ
    private bool heartSpawned = false;                     

    private void OnEnable()
    {
        playerStats.OnHPChanged += CheckForHeartSpawn;
    }

    // 指定された位置にコインを生成し、指定された力で発射します。
    public void SpawnCoin(Vector3 pos, Vector3 force)
    {
        GameObject coin = Instantiate(coinPrefab, pos, Quaternion.identity);
        Rigidbody rb = coin.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(force, ForceMode.Impulse);
        }
    }

    // 建物が破壊された時にコインを爆発的に生成
    public void ExplodeCoins()
    {
        const int CoinCount = 5;           
        const float SpawnRadius = 0.5f;    
        const float UpwardForce = 5f;      

        for (int i = 0; i < CoinCount; i++)
        {
            Vector3 spawnPos = transform.position + new Vector3(
                Random.Range(-SpawnRadius, SpawnRadius), 
                0.5f, 
                Random.Range(-SpawnRadius, SpawnRadius)
            );
            
            Vector3 force = new Vector3(
                Random.Range(-1f, 1f), 
                UpwardForce, 
                Random.Range(-1f, 1f)
            );

            SpawnCoin(spawnPos, force);
        }
    }

    // ランダムな位置に回復アイテムを生成
    public void SpawnHeart()
    {
        if (heartSpawnPoints.Length == 0) return;
        
        int idx = Random.Range(0, heartSpawnPoints.Length);
        GameObject heart = Instantiate(heartPrefab, heartSpawnPoints[idx].position, Quaternion.identity);
    }

    /// プレイヤーのHPが低下した時に回復アイテムの生成を判断。
    public void CheckForHeartSpawn(int currentHP)
    {
        const int HeartSpawnThreshold = 50;  

        if (currentHP <= HeartSpawnThreshold && !heartSpawned)
        {
            SpawnHeart();
            heartSpawned = true;
        }
    }
}
