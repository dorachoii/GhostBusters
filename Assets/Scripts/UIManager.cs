using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private BossStats bossStats;

    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI coinText;
    public Slider bossHP;
    public GameObject gameOverCanvas;

    private void OnEnable()
    {
        playerStats.OnHPChanged += UpdateHPUI;
        playerStats.OnCoinChanged += UpdateCoinUI;
        bossStats.OnHPChanged += UpdateBossHPSlider;
    }

    private void OnDisable()
    {
        playerStats.OnHPChanged -= UpdateHPUI;
        playerStats.OnCoinChanged -= UpdateCoinUI;
    }

    private void UpdateHPUI(int newHP)
    {
        playerHPText.text = newHP.ToString();

        if (newHP <= 0) ShowGameOver();
    }

    private void UpdateCoinUI(int newItem)
    {
        coinText.text = $"{newItem.ToString()} / {playerStats.maxItem}";
    }

    private void UpdateBossHPSlider(int newHP)
    {
        bossHP.maxValue = bossStats.maxHP;
        bossHP.value = newHP;
    }

    private void ShowGameOver()
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }
    }
}
