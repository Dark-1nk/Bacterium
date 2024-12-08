using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    public float attackRange = 1f; // Range to detect player units
    public LayerMask playerLayer; // Layer for player units
    public float attackCooldown = 1f; // Time between attacks
    public float moveSpeed = -2f; // Movement speed (negative for moving left)
    BountyDamage bountyDamage;

    private Animator animator;
    private float attackTimer;
    private Rigidbody2D rb;

    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        attackTimer = 0f; // Initialize the attack timer
        bountyDamage = GetComponent<BountyDamage>();
    }

    void Update()
    {
        animator.SetFloat("magnitude", rb.velocity.magnitude);

        attackTimer -= Time.deltaTime;

        if (!isAttacking)
        {
            MoveForward(); // Continue moving if not attacking
        }

        CheckAndAttackPlayerUnits();
    }

    void MoveForward()
    {
        // Move the unit forward (leftward)
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    void StopMoving()
    {
        // Stop the unit's movement
        rb.velocity = Vector2.zero;
    }

    void CheckAndAttackPlayerUnits()
    {
        // Detect player units within attack range
        Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayer);

        if (players.Length > 0)
        {
            StopMoving(); // Stop moving when a player unit is in range
            isAttacking = true;

            if (attackTimer <= 0)
            {
                // Trigger attack animation
                if (animator != null)
                {
                    animator.SetTrigger("Attack");
                }

                // Apply damage to the first player unit in range
                UnitHealth targetPlayer = players[0].GetComponent<UnitHealth>();
                if (targetPlayer != null)
                {
                    bountyDamage.DealDamage(players[0].gameObject);
                }

                // Reset the attack cooldown
                attackTimer = attackCooldown;
            }
        }
        else
        {
            // No player units in range, continue moving
            isAttacking = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the attack range in the editor
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
