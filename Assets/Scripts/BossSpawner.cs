 using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab;
    public int bossRoundInterval = 3; 

    private bool bossActive = false;
    private int lastBossRound = 0;

    public static BossSpawner Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        CheckForBossSpawn();
    }

    void CheckForBossSpawn()
    {
        if (RoundManager.Instance == null || bossActive) return;

        int currentRound = RoundManager.Instance.GetCurrentRound();

        // ¿Es ronda de boss?
        if (currentRound % bossRoundInterval == 0 && currentRound != lastBossRound)
        {
            if (RoundManager.Instance.IsRoundReadyForBoss())
            {
                SpawnBoss();
                lastBossRound = currentRound;
            }
        }
    }

    void SpawnBoss()
    {
        if (bossPrefab == null) return;

        GameObject boss = Instantiate(bossPrefab, transform.position, transform.rotation);
        bossActive = true;

        if (RoundManager.Instance != null)
        {
            RoundManager.Instance.EnemySpawned();
        }

        BossHealth bossHealth = boss.GetComponent<BossHealth>();
        if (bossHealth != null)
        {
            StartCoroutine(WaitForBossDeath(bossHealth));
        }
    }

    System.Collections.IEnumerator WaitForBossDeath(BossHealth bossHealth)
    {
        while (bossHealth != null && bossHealth.IsAlive())
        {
            yield return new WaitForSeconds(0.5f);
        }

        bossActive = false;
    }

    public bool IsBossActive()
    {
        return bossActive;
    }
}