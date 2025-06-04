using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs = new List<GameObject>();
    public float spawnInterval = 2f;
    public int maxEnemiesPerWave = 5;
    
    [Header("Control de Enemigos por Ronda")]
    public int extraEnemiesMargin = 3; 
    
    private bool isSpawning = false;
    private int currentWaveSize = 1;
    private int enemiesSpawnedThisRound = 0;
    private int maxEnemiesThisRound = 0;

    public void StartSpawning()
    {
        if (!isSpawning)
        {

            if (RoundManager.Instance != null)
            {
                int enemiesRequired = RoundManager.Instance.GetEnemiesRequiredThisRound();
                maxEnemiesThisRound = enemiesRequired + Mathf.Max(1, extraEnemiesMargin);
            }
            
            enemiesSpawnedThisRound = 0;
            isSpawning = true;
            InvokeRepeating("SpawnWave", 0f, spawnInterval);
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
        CancelInvoke("SpawnWave");
        currentWaveSize = 1; 
    }

    void SpawnWave()
    {
        if (enemiesSpawnedThisRound >= maxEnemiesThisRound)
        {
            return;
        }

        int enemiesToSpawn = Mathf.Min(currentWaveSize, maxEnemiesThisRound - enemiesSpawnedThisRound);
        
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
        }

        if (currentWaveSize < maxEnemiesPerWave)
        {
            currentWaveSize++;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Count == 0) return;

        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        Vector3 spawnPos = transform.position + Random.insideUnitSphere * 2f;
        spawnPos.y = transform.position.y;

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, transform.rotation);

        enemiesSpawnedThisRound++;

        if (RoundManager.Instance != null)
        {
            RoundManager.Instance.EnemySpawned();
        }
    }
}