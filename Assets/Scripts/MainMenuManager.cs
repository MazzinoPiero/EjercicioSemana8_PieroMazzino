using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameJolt.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button startGameButton;
    public Button showRankingsButton;
    public Button showTrophiesButton;
    public string gameSceneName = "GameScene";

    void Start()
    {
        if (startGameButton != null)
            startGameButton.onClick.AddListener(StartGame);

        if (showRankingsButton != null)
            showRankingsButton.onClick.AddListener(() =>
            {
                GameJoltUI.Instance.ShowLeaderboards();
            });

        if (showTrophiesButton != null)
            showTrophiesButton.onClick.AddListener(() =>
            {
                GameJoltUI.Instance.ShowTrophies();
            });
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }
}