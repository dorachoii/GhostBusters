using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private BossStats bossStats;
    
    public event Action<string> OnGameOver;  // 게임오버 이벤트

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        playerStats.OnHPChanged += CheckPlayerGameOver;
        bossStats.OnHPChanged += CheckBossGameOver;
    }

    private void CheckPlayerGameOver(int newHP)
    {
        if (newHP <= 0)
        {
            HandleGameOver("Game Over!");
        }
    }

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
        playerStats.OnHPChanged -= CheckPlayerGameOver;
        bossStats.OnHPChanged -= CheckBossGameOver;
    }
}
