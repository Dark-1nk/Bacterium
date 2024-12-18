using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : MonoBehaviour
{
    public float speed = 2.0f;
    public int health = 100;
    public int maxHealth = 100;
    public int attackDamage = 10;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.0f;
    public bool aoeAttack = false; // Attack all in range
    public Slider healthSlider;

    Animator animator;
    Rigidbody2D rb;

    private Transform targetUnit;
    private Transform targetTower;
    private float nextAttackTime = 0f;

    private void Start()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;

        animator.SetBool("isWalking", false);
    }

    private void Update()
    {
        // Update health slider position
        if (healthSlider != null)
        {
            healthSlider.transform.position = transform.position + new Vector3(0, 1.5f, 0);
        }

        bool unitsInRange = AnyPlayerUnitInRange();
        bool towersInRange = AnyTowerInRange();

        if (!unitsInRange && !towersInRange)
        {
            // Move forward if no targets are in range
            rb.velocity = new Vector2(-speed, rb.velocity.y); // Move left
            animator.SetBool("isWalking", true);
        }
        else
        {
            // Stop moving if there are targets in range
            rb.velocity = Vector2.zero;
            animator.SetBool("isWalking", false);
                
            if (Time.time >= nextAttackTime)
            {
                if (unitsInRange)
                {
                    if (aoeAttack)
                        AttackAllInRange();
                    else
                        AttackClosestUnit();
                }
                else if (towersInRange)
                {
                    AttackTower();
                }
            }
        }
    }

    private bool AnyPlayerUnitInRange()
    {
        targetUnit = null;
        float closestDistance = attackRange;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("PlayerUnit"))
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance <= closestDistance)
            {
                closestDistance = distance;
                targetUnit = player.transform;
            }
        }
        return targetUnit != null;
    }

    private bool AnyTowerInRange()
    {
        GameObject playerTower = GameObject.FindGameObjectWithTag("PlayerTower");
        if (playerTower != null)
        {
            float distance = Vector2.Distance(transform.position, playerTower.transform.position);
            if (distance <= attackRange)
            {
                targetTower = playerTower.transform;
                return true;
            }
        }
        return false;
    }

    private void AttackClosestUnit()
    {
        if (targetUnit != null)
        {
            PlayerUnit playerUnit = targetUnit.GetComponent<PlayerUnit>();
            if (playerUnit != null)
            {
                animator.SetTrigger("Attack");
                playerUnit.TakeDamage(attackDamage);
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    private void AttackAllInRange()
    {
        // Gather all player units
        GameObject[] players = GameObject.FindGameObjectsWithTag("PlayerUnit");

        foreach (GameObject player in players)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance <= attackRange)
            {
                PlayerUnit playerUnit = player.GetComponent<PlayerUnit>();
                if (playerUnit != null)
                {
                    animator.SetTrigger("Attack");
                    playerUnit.TakeDamage(attackDamage);
                }
            }
        }

        // Set cooldown
        nextAttackTime = Time.time + attackCooldown;
    }

    private void AttackTower()
    {
        if (targetTower != null)
        {
            TowerHealth towerHealth = targetTower.GetComponent<TowerHealth>();
            if (towerHealth != null)
            {
                animator.SetTrigger("Attack");
                towerHealth.TakeDamage(attackDamage);
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    public void ResetAttackTrigger()
    {
        animator.ResetTrigger("Attack");
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthSlider.value = health;

        bool reachedHalfHealth = false;

        if (health <= 0)
        {
            animator.SetTrigger("Die");
            Destroy(gameObject);
        }
        else if (health <= maxHealth * 0.5 && !reachedHalfHealth)
        {
            animator.SetTrigger("Hit");
            reachedHalfHealth = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
