using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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
    private float nextAttackTime = 0f;  // Cooldown timer
    private bool isAttacking = false;  // Attack flag

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

            // Attack if targets are in range and cooldown has passed
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
        if (targetUnit != null && !isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
            EnemyUnit enemy = targetUnit.GetComponent<EnemyUnit>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }
            nextAttackTime = Time.time + attackCooldown;  // Set cooldown
            StartCoroutine(ResetAttackFlag()); // Reset the flag after the attack
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
            if (distance <= attackRange && !isAttacking)
            {
                isAttacking = true;
                animator.SetTrigger("Attack");
                EnemyUnit enemyUnit = enemy.GetComponent<EnemyUnit>();
                if (enemyUnit != null)
                {
                    enemyUnit.TakeDamage(attackDamage);
                }
            }
        }

        // Set cooldown
        nextAttackTime = Time.time + attackCooldown;  // Set cooldown
        StartCoroutine(ResetAttackFlag()); // Reset the flag after the attack
    }

    private void AttackTower()
    {
        if (targetTower != null && !isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
            TowerHealth towerHealth = targetTower.GetComponent<TowerHealth>();
            if (towerHealth != null)
            {
                towerHealth.TakeDamage(attackDamage);
            }
            nextAttackTime = Time.time + attackCooldown;  // Set cooldown
            StartCoroutine(ResetAttackFlag()); // Reset the flag after the attack
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
        else
        {
            animator.SetTrigger("Hit");
        }
    }

    private IEnumerator ResetAttackFlag()
    {
        // Wait for the attack cooldown to finish
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
