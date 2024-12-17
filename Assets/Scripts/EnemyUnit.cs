using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : MonoBehaviour
{
    public float speed = 2.0f;
    public int health = 100;
    public int attackDamage = 10;
    public int towerDamage = 10;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.0f;
    public bool aoeAttack = false; // If true, attacks all player units in range
    public Slider healthSlider;
    Animator animator;
    private Rigidbody2D rb;

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

        // Check for player units in range
        bool playersInRange = AnyPlayerInRange();

        if (!playersInRange)
        {
            // Move forward if no players are in range
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            animator.SetBool("isWalking", true);
        }
        else
        {
            // Stop moving and attack if players are in range
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
                    AttackClosestPlayer();
                }
            }
        }
    }

    private bool AnyPlayerInRange()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("PlayerUnit"))
        {
            if (Vector2.Distance(transform.position, player.transform.position) <= attackRange)
            {
                return true;
            }
        }
        return false;
    }

    private void AttackClosestPlayer()
    {
        GameObject closestPlayer = null;
        float closestDistance = attackRange;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("PlayerUnit"))
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance <= closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player;
            }
        }

        if (closestPlayer != null)
        {
            PlayerUnit player = closestPlayer.GetComponent<PlayerUnit>();
            TowerHealth playerTower = closestPlayer.GetComponent<TowerHealth>();

            if (player != null || playerTower != null)
            {
                animator.SetTrigger("Attack");
                player.TakeDamage(attackDamage);
                playerTower.TakeDamage(towerDamage);
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    private void AttackAllInRange()
    {
        foreach (GameObject playerUnit in GameObject.FindGameObjectsWithTag("PlayerUnit"))
        {
            float distance = Vector2.Distance(transform.position, playerUnit.transform.position);
            if (distance <= attackRange)
            {
                PlayerUnit player = playerUnit.GetComponent<PlayerUnit>();
                TowerHealth playerTower = playerUnit.GetComponent<TowerHealth>();
                if (player != null || playerTower != null)
                {
                    animator.SetTrigger("Attack");
                    player.TakeDamage(attackDamage);
                    playerTower.TakeDamage(towerDamage);
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
