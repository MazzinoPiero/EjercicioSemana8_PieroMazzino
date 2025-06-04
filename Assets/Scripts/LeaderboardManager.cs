using UnityEngine;
using GameJolt.API;

public class LeaderboardManager : MonoBehaviour
{
    [Header("IDs de Leaderboards de GameJolt")]
    [SerializeField] private int totalRoundsLeaderboardID = 0;
    [SerializeField] private int totalKillsLeaderboardID = 0;

    public static LeaderboardManager Instance;

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

    public void SubmitFinalStats(int finalRound, int totalKills)
    {
        if (!GameJolt.API.GameJoltAPI.Instance.CurrentUser.IsAuthenticated)
            return;

        if (totalRoundsLeaderboardID != 0)
        {
            Scores.Add(finalRound, $"Ronda {finalRound}", totalRoundsLeaderboardID);
        }

        if (totalKillsLeaderboardID != 0)
        {
            Scores.Add(totalKills, $"{totalKills} enemigos", totalKillsLeaderboardID);
        }
    }
}