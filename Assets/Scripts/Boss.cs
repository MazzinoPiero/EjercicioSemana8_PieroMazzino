using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public float attackDamage = 25f;
    public float attackSpeed = 2f;
    
    private Transform player;
    private NavMeshAgent agent;
    private float nextAttackTime = 0f;
    private bool canAttack = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        agent = GetComponent<NavMeshAgent>();
        agent.speed = 2f; 
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
    }

    void AttackPlayer()
    {
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
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