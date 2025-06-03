using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs = new List<GameObject>();
    public float spawnInterval = 2f;
    public int maxEnemiesPerWave = 5;
    
    private bool isSpawning = false;
    private int currentWaveSize = 1;

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            InvokeRepeating("SpawnWave", 0f, spawnInterval);
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
        CancelInvoke("SpawnWave");
    }

    void SpawnWave()
    {
        for (int i = 0; i < currentWaveSize; i++)
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

        if (RoundManager.Instance != null)
        {
            RoundManager.Instance.EnemySpawned();
        }
    }
}