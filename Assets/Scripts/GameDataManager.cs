using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    private float totalGameTime;
    private int finalRound;
    private int totalKills;

    public static GameDataManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGameData(float gameTime, int round, int kills)
    {
        totalGameTime = gameTime;
        finalRound = round;
        totalKills = kills;
    }

    public float GetTotalGameTime() { return totalGameTime; }
    public int GetFinalRound() { return finalRound; }
    public int GetTotalKills() { return totalKills; }
    
    public string GetFormattedGameTime()
    {
        int minutes = Mathf.FloorToInt(totalGameTime / 60f);
        int seconds = Mathf.FloorToInt(totalGameTime % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}