using UnityEngine;

public class BossHealth : EnemyHealth
{
    public static BossHealth Instance;

    protected override void Start()
    {
        base.Start();
        Instance = this;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowBossHealthBar();
        }
    }

    protected override void Die()
    {
    
        if (UIManager.Instance != null)
        {
            UIManager.Instance.HideBossHealthBar();
        }

        Instance = null;

        base.Die();
    }
}