using UnityEngine;
using System.Collections.Generic;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("Configuración de Objetos Recolectables")]
    public List<GameObject> collectiblePrefabs = new List<GameObject>();
    public Transform spawnPoint;
    
    [Header("Control de Spawneo")]
    public bool spawnEveryRound = true; 
    public List<int> specificRounds = new List<int>(); 
    
    private List<int> usedPrefabIndices = new List<int>();
    private int lastRoundSpawned = 0;
    
    public static CollectibleSpawner Instance;

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

    void Start()
    {
        if (spawnPoint == null)
            spawnPoint = transform;
    }

    void Update()
    {
        CheckForSpawn();
    }

    void CheckForSpawn()
    {
        if (RoundManager.Instance == null) return;
        
        int currentRound = RoundManager.Instance.GetCurrentRound();

        if (usedPrefabIndices.Count >= collectiblePrefabs.Count)
        {
            return;
        }

        if (currentRound > lastRoundSpawned && currentRound >= 2)
        {
            bool shouldSpawn = false;
            
            if (spawnEveryRound)
            {
                shouldSpawn = true;
            }
            else
            {
                shouldSpawn = specificRounds.Contains(currentRound);
            }
            
            if (shouldSpawn)
            {
                SpawnCollectible();
                lastRoundSpawned = currentRound;
                Debug.Log($"Objeto spawneado al inicio de la ronda {currentRound}");
            }
        }
    }

    void SpawnCollectible()
    {
        if (collectiblePrefabs.Count == 0)
        {
            Debug.LogWarning("No hay prefabs de objetos recolectables asignados!");
            return;
        }

        if (usedPrefabIndices.Count >= collectiblePrefabs.Count)
        {
            Debug.Log("Todos los objetos recolectables han sido spawneados. No se generarán más.");
            return;
        }

        List<int> availableIndices = new List<int>();
        for (int i = 0; i < collectiblePrefabs.Count; i++)
        {
            if (!usedPrefabIndices.Contains(i))
            {
                availableIndices.Add(i);
            }
        }
        
        if (availableIndices.Count == 0)
        {
            Debug.LogWarning("No hay objetos recolectables disponibles para spawnear!");
            return;
        }

        int randomIndex = availableIndices[Random.Range(0, availableIndices.Count)];
        usedPrefabIndices.Add(randomIndex);

        GameObject collectible = Instantiate(collectiblePrefabs[randomIndex], spawnPoint.position, spawnPoint.rotation);
        
        Debug.Log($"Objeto recolectable spawneado: {collectible.name} al inicio de la ronda {RoundManager.Instance.GetCurrentRound()}");
        Debug.Log($"Objetos restantes por spawnear: {GetRemainingCollectibles()}");
    }

    public void ForceSpawn()
    {
        SpawnCollectible();
    }

    public void ResetUsedPrefabs()
    {
        usedPrefabIndices.Clear();
        Debug.Log("Lista de objetos recolectables usados reiniciada.");
    }

    public int GetRemainingCollectibles()
    {
        return collectiblePrefabs.Count - usedPrefabIndices.Count;
    }
    
    public bool HasMoreCollectibles()
    {
        return usedPrefabIndices.Count < collectiblePrefabs.Count;
    }
}