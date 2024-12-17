using UnityEngine;
using UnityEngine.UI;

public class PlayerUnit : MonoBehaviour
{
    public float speed = 2.0f;
    public int health;
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
            healthSlider.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1, 0));
        }

        bool unitsInRange = AnyEnemyInRange();
        bool towersInRange = AnyTowerInRange();

        if (!unitsInRange && !towersInRange)
        {
            // Move forward if no targets are in range
            rb.velocity = new Vector2(speed, rb.velocity.y);
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
                        AttackClosestEnemy();
                }
                else if (towersInRange)
                {
                    AttackTower();
                }
            }
        }
    }

    private bool AnyEnemyInRange()
    {
        targetUnit = null;
        float closestDistance = attackRange;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("EnemyUnit"))
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= closestDistance)
            {
                closestDistance = distance;
                targetUnit = enemy.transform;
            }
        }
        return targetUnit != null;
    }

    private bool AnyTowerInRange()
    {
        GameObject enemyTower = GameObject.FindGameObjectWithTag("EnemyTower");
        if (enemyTower != null)
        {
            float distance = Vector2.Distance(transform.position, enemyTower.transform.position);
            if (distance <= attackRange)
            {
                targetTower = enemyTower.transform;
                return true;
            }
        }
        return false;
    }

    private void AttackClosestEnemy()
    {
        if (targetUnit != null)
        {
            EnemyUnit enemy = targetUnit.GetComponent<EnemyUnit>();
            if (enemy != null)
            {
                animator.SetTrigger("Attack");
                enemy.TakeDamage(attackDamage);
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    private void AttackAllInRange()
    {
        // Gather all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyUnit");

        foreach (GameObject enemy in enemies)
        {
            // Check if enemy is within attack range
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= attackRange)
            {
                EnemyUnit enemyUnit = enemy.GetComponent<EnemyUnit>();
                if (enemyUnit != null)
                {
                    animator.SetTrigger("Attack");
                    enemyUnit.TakeDamage(attackDamage);
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

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthSlider.value = health;

        if (health <= 0)
        {
            animator.SetTrigger("Die");
            Destroy(gameObject);
        }
        else if (health <= maxHealth * 0.5) 
        {
            animator.SetTrigger("Hit");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
