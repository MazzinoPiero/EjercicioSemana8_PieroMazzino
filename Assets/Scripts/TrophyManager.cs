using UnityEngine;
using GameJolt.API;

public class TrophyManager : MonoBehaviour
{
    [Header("Códigos de Trofeos de GameJolt")]
    [SerializeField] private int surviveFirstRoundTrophyID = 269785;
    [SerializeField] private int firstKillTrophyID = 269786;
    [SerializeField] private int killBossTrophyID = 269787;
    [SerializeField] private int collectItemTrophyID = 269788;
    [SerializeField] private int createSpecialObjectTrophyID = 269789;

    [Header("Estado de Trofeos (Solo para debug)")]
    [SerializeField] private bool surviveFirstRoundUnlocked = false;
    [SerializeField] private bool firstKillUnlocked = false;
    [SerializeField] private bool killBossUnlocked = false;
    [SerializeField] private bool collectItemUnlocked = false;
    [SerializeField] private bool createSpecialObjectUnlocked = false;

    public static TrophyManager Instance;

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

    void Start()
    {
        // Suscribirse a eventos si están disponibles
        SubscribeToEvents();
    }

    void Update()
    {
        CheckTrophies();
    }

    void SubscribeToEvents()
    {
        // Los eventos se manejan mediante llamadas directas desde los otros scripts
        // No necesitamos suscripciones adicionales
    }

    void CheckTrophies()
    {
        CheckSurviveFirstRound();
        CheckFirstKill();
        CheckCollectItem();
        // Los otros se verifican via eventos
    }

    void CheckSurviveFirstRound()
    {
        if (surviveFirstRoundUnlocked) return;

        if (RoundManager.Instance != null)
        {
            int currentRound = RoundManager.Instance.GetCurrentRound();
            if (currentRound >= 2)
            {
                UnlockTrophy(surviveFirstRoundTrophyID, "¡Sobreviviste la primera ronda!");
                surviveFirstRoundUnlocked = true;
            }
        }
    }

    void CheckFirstKill()
    {
        if (firstKillUnlocked) return;

        if (RoundManager.Instance != null)
        {
            int totalKills = RoundManager.Instance.GetTotalEnemiesKilled();
            if (totalKills >= 1)
            {
                UnlockTrophy(firstKillTrophyID, "¡Primera baja conseguida!");
                firstKillUnlocked = true;
            }
        }
    }

    void CheckCollectItem()
    {
        if (collectItemUnlocked) return;

        PlayerCollector collector = FindFirstObjectByType<PlayerCollector>();
        if (collector != null)
        {
            int collectedItems = collector.GetCollectedItemsCount();
            if (collectedItems >= 1)
            {
                UnlockTrophy(collectItemTrophyID, "¡Primer objeto recolectado!");
                collectItemUnlocked = true;
            }
        }
    }

    // Método público para ser llamado cuando muere un boss
    public void OnBossKilled()
    {
        if (killBossUnlocked) return;

        UnlockTrophy(killBossTrophyID, "¡Boss derrotado!");
        killBossUnlocked = true;
    }

    // Método público para ser llamado cuando se activa el objeto especial
    public void OnSpecialObjectActivated()
    {
        if (createSpecialObjectUnlocked) return;

        UnlockTrophy(createSpecialObjectTrophyID, "¡Objeto especial creado!");
        createSpecialObjectUnlocked = true;
    }

    void UnlockTrophy(int trophyID, string message)
    {
        // Verificar si el usuario está logueado
        if (!GameJolt.API.GameJoltAPI.Instance.CurrentUser.IsAuthenticated)
        {
            Debug.LogWarning("Usuario no autenticado en GameJolt. No se puede desbloquear trofeo.");
            return;
        }

        // Desbloquear trofeo usando el método simple
        Trophies.Unlock(trophyID, (bool success) =>
        {
            if (success)
            {
                Debug.Log($"Trofeo desbloqueado: {message}");
                ShowTrophyNotification(message);
            }
            else
            {
                Debug.LogError($"Error al desbloquear trofeo: {trophyID}");
            }
        });
    }

    void ShowTrophyNotification(string message)
    {
        // Implementa aquí tu sistema de notificaciones si lo deseas
        // Por ejemplo, podrías tener un UI Canvas que muestre el mensaje temporalmente
        Debug.Log($"TROFEO DESBLOQUEADO: {message}");
    }

    // Método para resetear trofeos (útil para testing)
    public void ResetTrophies()
    {
        surviveFirstRoundUnlocked = false;
        firstKillUnlocked = false;
        killBossUnlocked = false;
        collectItemUnlocked = false;
        createSpecialObjectUnlocked = false;
    }

    void OnDestroy()
    {
        // Cleanup si es necesario
        // Como usamos llamadas directas, no hay eventos de los que desuscribirse
    }
}