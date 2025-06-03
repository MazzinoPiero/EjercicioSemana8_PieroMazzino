using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public int currentRound = 1;
    public int enemiesRequiredPerRound = 5;
    public int enemiesIncrease = 2;
    
    private int enemiesKilledThisRound = 0;
    private int totalEnemiesKilled = 0;
    private int aliveEnemies = 0;
    private int currentRequirement;
    
    private float gameStartTime;
    private float totalGameTime;

    public static RoundManager Instance;

    void Awake()
    {
        Instance = this;
        gameStartTime = Time.time;
    }

    void Start()
    {
        currentRequirement = enemiesRequiredPerRound;
        StartRound();
    }

    void Update()
    {
        totalGameTime = Time.time - gameStartTime;

        if (enemiesKilledThisRound >= currentRequirement && aliveEnemies <= 0)
        {
            if (BossSpawner.Instance == null || !BossSpawner.Instance.IsBossActive())
            {
                CompleteRound();
            }
        }
    }

    void StartRound()
    {
        enemiesKilledThisRound = 0;
        currentRequirement = enemiesRequiredPerRound + (enemiesIncrease * (currentRound - 1));
        
        // Iniciar spawning de enemigos
        EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.StartSpawning();
        }
    }

    void CompleteRound()
    {
        currentRound++;

        EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.StopSpawning();
        }
        
        Invoke("StartRound", 3f);
    }

    public void EnemySpawned()
    {
        aliveEnemies++;
    }

    public void EnemyKilled()
    {
        aliveEnemies--;
        enemiesKilledThisRound++;
        totalEnemiesKilled++;
    }

    public bool IsRoundReadyForBoss()
    {
        return enemiesKilledThisRound >= currentRequirement && aliveEnemies <= 0;
    }

    // Getters
    public int GetCurrentRound() { return currentRound; }
    public int GetTotalEnemiesKilled() { return totalEnemiesKilled; }
    public float GetTotalGameTime() { return totalGameTime; }
    
    public string GetFormattedGameTime()
    {
        int minutes = Mathf.FloorToInt(totalGameTime / 60f);
        int seconds = Mathf.FloorToInt(totalGameTime % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}