using UnityEngine;
using System;

/// <summary>
/// playerとbossのHPを監視してゲームオーバー状態を制御します。
/// </summary>

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance { get; private set; }

     // Player, Boss Status 参照
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private BossStats bossStats;
    
    // event
    public event Action<string> OnGameOver;  

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // eventの購読
        playerStats.OnHPChanged += CheckPlayerGameOver;
        bossStats.OnHPChanged += CheckBossGameOver;
    }

    // PlayerHP Check
    private void CheckPlayerGameOver(int newHP)
    {
        if (newHP <= 0)
        {
            HandleGameOver("Game Over!");
        }
    }

    // BossHP Check
    private void CheckBossGameOver(int newHP)
    {
        if (newHP <= 0)
        {
            HandleGameOver("Game Clear!");
        }
    }

    private void HandleGameOver(string message)
    {
        OnGameOver?.Invoke(message);
    }

    private void OnDestroy()
    {
        // eventの購読解除
        playerStats.OnHPChanged -= CheckPlayerGameOver;
        bossStats.OnHPChanged -= CheckBossGameOver;
    }
}
