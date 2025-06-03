using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI finalTimeText;
    public TextMeshProUGUI finalRoundText;
    public TextMeshProUGUI finalKillsText;
    public Button retryButton;
    public Button menuButton;
    
    public string gameSceneName = "GameScene";
    public string menuSceneName = "MenuScene";

    void Start()
    {
        DisplayStats();
        
        if (retryButton != null)
            retryButton.onClick.AddListener(RestartGame);
            
        if (menuButton != null)
            menuButton.onClick.AddListener(GoToMenu);
    }

    void DisplayStats()
    {
        if (GameDataManager.Instance != null)
        {
            if (finalTimeText != null)
                finalTimeText.text = "Tiempo: " + GameDataManager.Instance.GetFormattedGameTime();
                
            if (finalRoundText != null)
                finalRoundText.text = "Ronda: " + GameDataManager.Instance.GetFinalRound();
                
            if (finalKillsText != null)
                finalKillsText.text = "Bajas: " + GameDataManager.Instance.GetTotalKills();
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}