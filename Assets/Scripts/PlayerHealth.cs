using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    [Header("UI del Jugador")]
    public Image healthBar;

    [Header("Configuración de Game Over")]
    public string gameOverSceneName = "GameOverScene";

    protected override void Start()
    {
        base.Start();
        UpdateHealthBar();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        UpdateHealthBar();
    }

    protected override void Die()
    {

        if (RoundManager.Instance != null && GameDataManager.Instance != null)
        {
            GameDataManager.Instance.SaveGameData(
                RoundManager.Instance.GetTotalGameTime(),
                RoundManager.Instance.GetCurrentRound(),
                RoundManager.Instance.GetTotalEnemiesKilled()
            );
        }


        SceneManager.LoadScene(gameOverSceneName);
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = GetHealthPercent();
        }
    }

    public override void RestoreHealth(float amount)
    {
        base.RestoreHealth(amount);
        UpdateHealthBar();
    }

    public override void SetFullHealth()
    {
        base.SetFullHealth();
        UpdateHealthBar();
    }
}