using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    public float speed = 2f;
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    private float attackTimer;
    public GameObject bountyTargetPrefab;
    private BountyDamage bountyDamage;
    public LayerMask enemyLayer;
    public Animator animator;
    Rigidbody2D rb;

    bool isAttacking;

    void Start()
    {
        bountyDamage = GetComponent<BountyDamage>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        animator.SetFloat("magnitude", rb.velocity.magnitude);

        attackTimer -= Time.deltaTime;

        if (!isAttacking)
        {
            MoveForward(); // Continue moving if not attacking
        }

        CheckAndAttackEnemies();
    }

    void MoveForward()
    {
        // Move the unit forward
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    void StopMoving()
    {
        // Stop the unit's movement
        rb.velocity = Vector2.zero;
    }

    void CheckAndAttackEnemies()
    {
        // Detect enemies within attack range
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        if (enemies.Length > 0)
        {
            StopMoving(); // Stop moving when an enemy is in range
            isAttacking = true;

            if (attackTimer <= 0)
            {
                // Trigger attack animation
                if (animator != null)
                {
                    animator.SetTrigger("Attack");
                }

                // Apply damage
                if (bountyDamage != null)
                {
                    bountyDamage.DealDamage(enemies[0].gameObject); // Attack the first enemy in range
                }

                // Reset the attack cooldown
                attackTimer = attackCooldown;
            }
        }
        else
        {
            // No enemies in range, continue moving
            isAttacking = false;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
