using UnityEngine;
using TMPro;


public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI coinText;


    private void OnEnable()
    {
        playerStats.OnHPChanged += UpdateHPUI;
        playerStats.OnCoinChanged += UpdateCoinUI;
    }

    private void OnDisable()
    {
        playerStats.OnHPChanged -= UpdateHPUI;
        playerStats.OnCoinChanged -= UpdateCoinUI;
    }

    private void UpdateHPUI(int newHP)
    {
        hpText.text = newHP.ToString();
    }

    private void UpdateCoinUI(int newItem)
    {
        coinText.text = $"{newItem.ToString()} / {playerStats.maxItem}";
    }
}
