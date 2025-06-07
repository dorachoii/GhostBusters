using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// ゲームのUI要素を管理します。
/// Observerパターンを活用し、eventsベースの通信でUIを更新します。
/// </summary>

public class UIManager : MonoBehaviour
{
    // Player, Boss Status 参照
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private BossStats bossStats;

    public TextMeshProUGUI playerHPText;    
    public TextMeshProUGUI coinText;        
    public Slider bossHP;                  
    public GameObject gameOverCanvas;       
    public TextMeshProUGUI gameOverText;    

    // eventの購読設定
    private void OnEnable()
    {
        playerStats.OnHPChanged += UpdateHPUI;
        playerStats.OnRingChanged += UpdateRingUI;
        bossStats.OnHPChanged += UpdateBossHPSlider;
        GameManager.Instance.OnGameOver += ShowGameOver;
    }

    // eventの購読解除
    private void OnDisable()
    {
        playerStats.OnHPChanged -= UpdateHPUI;
        playerStats.OnRingChanged -= UpdateRingUI;
        bossStats.OnHPChanged -= UpdateBossHPSlider;
        GameManager.Instance.OnGameOver -= ShowGameOver;
    }

    // Player HP
    private void UpdateHPUI(int newHP)
    {
        playerHPText.text = newHP.ToString();
    }

    // Player Rings Count
    private void UpdateRingUI(int newItem)
    {
        coinText.text = $"{newItem.ToString()} / {playerStats.maxRingCount}";
    }

    // Boss HP
    private void UpdateBossHPSlider(int newHP)
    {
        bossHP.maxValue = bossStats.maxHP;
        bossHP.value = newHP;
    }

    // Game Over
    private void ShowGameOver(string text)
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
            gameOverText.text = text;
        }
    }
}
