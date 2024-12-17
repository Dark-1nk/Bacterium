using UnityEngine;
using UnityEngine.UI;

public class PlayerUnit : MonoBehaviour
{
    public float speed = 2.0f;
    public int health = 100;
    public int attackDamage = 10;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.0f;
    public bool aoeAttack = false; // If true, attacks all enemies in range; otherwise, attacks the closest
    public Slider healthSlider;
    Animator animator;
    private Rigidbody2D rb;

    private Transform target;
    private float nextAttackTime = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        healthSlider.maxValue = health;
        healthSlider.value = health;

        animator.SetBool("isWalking", true); // Walking is the default animation
    }

    private void Update()
    {
        // Update health slider position
        if (healthSlider != null)
        {
            healthSlider.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1, 0));
        }

        // Check for enemies in range
        bool enemiesInRange = AnyEnemyInRange();

        if (!enemiesInRange)
        {
            // Move forward if no enemies are in range
            rb.velocity = new Vector2(speed, rb.velocity.y);
            animator.SetBool("isWalking", true);
        }
        else
        {
            // Stop moving and attack if enemies are in range
            rb.velocity = Vector2.zero;

            if (Time.time >= nextAttackTime)
            {
                animator.SetBool("isWalking", false);
                if (aoeAttack)
                {
                    AttackAllInRange();
                }
                else
                {
                    AttackClosestEnemy();
                }
            }
        }
    }

    private bool AnyEnemyInRange()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("EnemyUnit"))
        {
            if (Vector2.Distance(transform.position, enemy.transform.position) <= attackRange)
            {
                return true;
            }
        }
        return false;
    }

    private void AttackClosestEnemy()
    {
        GameObject closestEnemy = null;
        float closestDistance = attackRange;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("EnemyUnit"))
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            EnemyUnit enemy = closestEnemy.GetComponent<EnemyUnit>();
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
        foreach (GameObject enemyUnit in GameObject.FindGameObjectsWithTag("EnemyUnit"))
        {
            float distance = Vector2.Distance(transform.position, enemyUnit.transform.position);
            if (distance <= attackRange)
            {
                EnemyUnit enemy = enemyUnit.GetComponent<EnemyUnit>();
                if (enemy != null)
                {
                    animator.SetTrigger("Attack");
                    enemy.TakeDamage(attackDamage);
                }
            }
        }

        nextAttackTime = Time.time + attackCooldown;
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
