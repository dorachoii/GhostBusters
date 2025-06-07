using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// ゲームのUI要素を管理し、プレイヤーのHP、コイン数、ボスのHPを更新し、ゲームオーバー状態を処理します。
/// </summary>

public class UIManager : MonoBehaviour
{
    // Player, Boss Status 参照
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private BossStats bossStats;

    public TextMeshProUGUI playerHPText;    // プレイヤーのHP表示
    public TextMeshProUGUI coinText;        // コイン数表示
    public Slider bossHP;                   // ボスのHP表示
    public GameObject gameOverCanvas;        // ゲームオーバー画面
    public TextMeshProUGUI gameOverText;    // ゲームオーバーメッセージ

    // イベントの購読設定
    private void OnEnable()
    {
        playerStats.OnHPChanged += UpdateHPUI;
        playerStats.OnCoinChanged += UpdateCoinUI;
        bossStats.OnHPChanged += UpdateBossHPSlider;
        GameManager.Instance.OnGameOver += ShowGameOver;
    }

    // イベントの購読解除
    private void OnDisable()
    {
        playerStats.OnHPChanged -= UpdateHPUI;
        playerStats.OnCoinChanged -= UpdateCoinUI;
        bossStats.OnHPChanged -= UpdateBossHPSlider;
        GameManager.Instance.OnGameOver -= ShowGameOver;
    }

    // プレイヤーのHPテキストを更新
    private void UpdateHPUI(int newHP)
    {
        playerHPText.text = newHP.ToString();
    }

    // プレイヤーのコインテキストを更新
    private void UpdateCoinUI(int newItem)
    {
        coinText.text = $"{newItem.ToString()} / {playerStats.maxItem}";
    }

    // ボスのHPスライダーを更新
    private void UpdateBossHPSlider(int newHP)
    {
        bossHP.maxValue = bossStats.maxHP;
        bossHP.value = newHP;
    }

    // ゲームオーバー画面を表示
    private void ShowGameOver(string text)
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
            gameOverText.text = text;
        }
    }
}
