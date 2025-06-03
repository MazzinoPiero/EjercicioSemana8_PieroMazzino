using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI del Juego")]
    public TextMeshProUGUI gameTimeText;
    public TextMeshProUGUI currentRoundText;
    public TextMeshProUGUI totalKillsText;
    
    [Header("UI del Boss")]
    public GameObject bossHealthPanel;
    public Image bossHealthBar;

    public static UIManager Instance;

    void Awake()
    {
        Instance = this;
        HideBossHealthBar();
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (RoundManager.Instance == null) return;

        if (gameTimeText != null)
            gameTimeText.text = "Tiempo: " + RoundManager.Instance.GetFormattedGameTime();
            
        if (currentRoundText != null)
            currentRoundText.text = "Ronda: " + RoundManager.Instance.GetCurrentRound();
            
        if (totalKillsText != null)
            totalKillsText.text = "Bajas: " + RoundManager.Instance.GetTotalEnemiesKilled();

        if (BossHealth.Instance != null && bossHealthBar != null)
        {
            bossHealthBar.fillAmount = BossHealth.Instance.GetHealthPercent();
        }
    }

    public void ShowBossHealthBar()
    {
        if (bossHealthPanel != null)
            bossHealthPanel.SetActive(true);
    }

    public void HideBossHealthBar()
    {
        if (bossHealthPanel != null)
            bossHealthPanel.SetActive(false);
    }
}