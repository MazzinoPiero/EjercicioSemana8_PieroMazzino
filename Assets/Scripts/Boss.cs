using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Boss : MonoBehaviour
{
    [Header("Ataque Básico")]
    public float attackDamage = 25f;
    public float attackSpeed = 2f;
    
    [Header("Sistema de Progresión")]
    [SerializeField] private int currentBossNumber = 0; 
    
    [Header("Habilidad 1: Disparo Simple")]
    public bool enableSingleShot = false;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 8f;
    public float singleShotInterval = 3f;
    
    [Header("Habilidad 2: Disparo Doble")]
    public bool enableDoubleShot = false;
    public float doubleShotInterval = 5f;
    public float timeBetweenBurstShots = 0.3f;
    
    [Header("Habilidad 3: Boost de Velocidad")]
    public bool enableSpeedBoost = false;
    public float speedBoostDuration = 4f;
    public float speedBoostInterval = 8f;
    public float speedMultiplier = 2f;
    
    private Transform player;
    private NavMeshAgent agent;
    private float nextAttackTime = 0f;
    private bool canAttack = false;
    
    private float nextSingleShotTime = 0f;
    private float nextDoubleShotTime = 0f;
    private float nextSpeedBoostTime = 0f;
    private float normalSpeed;
    private bool isSpeedBoosted = false;
    
    private Coroutine doubleShotCoroutine;
    private Coroutine speedBoostCoroutine;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        agent = GetComponent<NavMeshAgent>();
        agent.speed = 2f;
        normalSpeed = agent.speed;

        if (firePoint == null)
            firePoint = transform;

        currentBossNumber = GetCurrentBossNumber();
        ConfigureAbilitiesBasedOnAppearance();
        
        Debug.Log("Boss #" + currentBossNumber + " - Habilidades configuradas");
    }

    void Update()
    {
        if (player == null || agent == null) return;

        agent.SetDestination(player.position);
        
        if (canAttack && Time.time >= nextAttackTime)
        {
            AttackPlayer();
            nextAttackTime = Time.time + attackSpeed;
        }
        
        HandleSingleShot();
        HandleDoubleShot();
        HandleSpeedBoost();
    }

    void AttackPlayer()
    {
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }
    
    int GetCurrentBossNumber()
    {
        if (RoundManager.Instance == null) return 1;
 
        int currentRound = RoundManager.Instance.GetCurrentRound();
        int bossNumber = (currentRound / 3); // 3, 6, 9, etc.

        return Mathf.Max(1, bossNumber);
    }
    
    void ConfigureAbilitiesBasedOnAppearance()
    {

        enableSingleShot = false;
        enableDoubleShot = false;
        enableSpeedBoost = false;

        switch (currentBossNumber)
        {
            case 1:

                Debug.Log("Boss #1: Solo ataque básico");
                break;
                
            case 2:

                enableSingleShot = true;
                Debug.Log("Boss #2: Single Shot activado");
                break;
                
            case 3:

                enableSingleShot = true;
                enableDoubleShot = true;
                Debug.Log("Boss #3: Single Shot + Double Shot activados");
                break;
                
            default:

                if (currentBossNumber >= 4)
                {
                    enableSingleShot = true;
                    enableDoubleShot = true;
                    enableSpeedBoost = true;
                    Debug.Log("Boss #" + currentBossNumber + ": Todas las habilidades activadas");
                }
                break;
        }
    }
    
    void HandleSingleShot()
    {
        if (!enableSingleShot || bulletPrefab == null) return;
        
        if (Time.time >= nextSingleShotTime)
        {
            ShootAtPlayer();
            nextSingleShotTime = Time.time + singleShotInterval;
        }
    }
    
    void HandleDoubleShot()
    {
        if (!enableDoubleShot || bulletPrefab == null) return;
        
        if (Time.time >= nextDoubleShotTime && doubleShotCoroutine == null)
        {
            doubleShotCoroutine = StartCoroutine(DoubleShotSequence());
            nextDoubleShotTime = Time.time + doubleShotInterval;
        }
    }
    
    void HandleSpeedBoost()
    {
        if (!enableSpeedBoost) return;
        
        if (Time.time >= nextSpeedBoostTime && speedBoostCoroutine == null)
        {
            speedBoostCoroutine = StartCoroutine(SpeedBoostSequence());
            nextSpeedBoostTime = Time.time + speedBoostInterval;
        }
    }
    
    void ShootAtPlayer()
    {
        if (player == null) return;
        
        Vector3 direction = (player.position - firePoint.position).normalized;
        direction.y = 0; 

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.targetTag = "Player"; 
        }

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.linearVelocity = direction * bulletSpeed;
        }
    }
    
    IEnumerator DoubleShotSequence()
    {
        ShootAtPlayer();

        yield return new WaitForSeconds(timeBetweenBurstShots);

        ShootAtPlayer();

        doubleShotCoroutine = null;
    }
    
    IEnumerator SpeedBoostSequence()
    {
        isSpeedBoosted = true;
        agent.speed = normalSpeed * speedMultiplier;
        
        yield return new WaitForSeconds(speedBoostDuration);

        isSpeedBoosted = false;
        agent.speed = normalSpeed;

        speedBoostCoroutine = null;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canAttack = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canAttack = false;
        }
    }
    
    public void ActivateSingleShot()
    {
        enableSingleShot = true;
    }
    
    public void DeactivateSingleShot()
    {
        enableSingleShot = false;
    }
    
    public void ActivateDoubleShot()
    {
        enableDoubleShot = true;
    }
    
    public void DeactivateDoubleShot()
    {
        enableDoubleShot = false;
    }
    
    public void ActivateSpeedBoost()
    {
        enableSpeedBoost = true;
    }
    
    public void DeactivateSpeedBoost()
    {
        enableSpeedBoost = false;

        if (speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine);
            speedBoostCoroutine = null;
            isSpeedBoosted = false;
            agent.speed = normalSpeed;
        }
    }

    public bool IsSpeedBoosted() { return isSpeedBoosted; }
    public float GetCurrentSpeed() { return agent.speed; }
    public int GetBossNumber() { return currentBossNumber; }
    
    public void SpecialAttack1()
    {
        // TODO: Implementar ataque especial 1
    }
    
    public void SpecialAttack2()
    {
        // TODO: Implementar ataque especial 2
    }
    
    public void DefensiveAbility()
    {
        // TODO: Implementar habilidad defensiva
    }
}