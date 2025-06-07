using UnityEngine;

/// <summary>
/// アイテムの生成を管理するクラスです。
/// 建物が破壊された時にコインを生成し、プレイヤーのHPが低下した時に回復アイテムを生成します。
/// </summary>
public class ItemSpawner : MonoBehaviour
{
    // 生成するアイテムのプレハブ
    [SerializeField] private GameObject coinPrefab;   // コインのプレハブ
    [SerializeField] private GameObject heartPrefab;  // 回復アイテムのプレハブ

    // 回復アイテムの生成位置
    [SerializeField] private Transform[] heartSpawnPoints;  // 回復アイテムの生成位置リスト

    // プレイヤーの状態監視用
    [SerializeField] private PlayerStats playerStats;      // プレイヤーのステータス
    private bool heartSpawned = false;                     // 回復アイテムが生成済みかどうか

    private void OnEnable()
    {
        // プレイヤーのHP変更イベントを購読
        playerStats.OnHPChanged += CheckForHeartSpawn;
    }

    /// <summary>
    /// 指定された位置にコインを生成し、指定された力で発射します。
    /// </summary>
    /// <param name="pos">生成位置</param>
    /// <param name="force">発射する力</param>
    public void SpawnCoin(Vector3 pos, Vector3 force)
    {
        GameObject coin = Instantiate(coinPrefab, pos, Quaternion.identity);
        Rigidbody rb = coin.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(force, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// 建物が破壊された時にコインを爆発的に生成します。
    /// </summary>
    public void ExplodeCoins()
    {
        const int CoinCount = 5;           // 生成するコインの数
        const float SpawnRadius = 0.5f;    // 生成範囲の半径
        const float UpwardForce = 5f;      // 上方向の力

        for (int i = 0; i < CoinCount; i++)
        {
            // ランダムな位置にコインを生成
            Vector3 spawnPos = transform.position + new Vector3(
                Random.Range(-SpawnRadius, SpawnRadius), 
                0.5f, 
                Random.Range(-SpawnRadius, SpawnRadius)
            );
            
            // ランダムな方向に力を加える
            Vector3 force = new Vector3(
                Random.Range(-1f, 1f), 
                UpwardForce, 
                Random.Range(-1f, 1f)
            );

            SpawnCoin(spawnPos, force);
        }
    }

    /// <summary>
    /// ランダムな位置に回復アイテムを生成します。
    /// </summary>
    public void SpawnHeart()
    {
        if (heartSpawnPoints.Length == 0) return;
        
        int idx = Random.Range(0, heartSpawnPoints.Length);
        GameObject heart = Instantiate(heartPrefab, heartSpawnPoints[idx].position, Quaternion.identity);
    }

    /// <summary>
    /// プレイヤーのHPが低下した時に回復アイテムの生成を判断します。
    /// </summary>
    /// <param name="currentHP">現在のHP</param>
    public void CheckForHeartSpawn(int currentHP)
    {
        const int HeartSpawnThreshold = 50;  // 回復アイテム生成のHP閾値

        if (currentHP <= HeartSpawnThreshold && !heartSpawned)
        {
            SpawnHeart();
            heartSpawned = true;
        }
    }
}
