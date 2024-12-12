using UnityEngine;
using UnityEngine.UI;

public class PlayerUnit : MonoBehaviour
{
    public float speed = 2.0f;
    public int health = 100;
    public int attackDamage = 10;
    public float attackRange = 1.5f; // Range within which the unit can attack
    public float attackCooldown = 1.0f;
    public bool aoeAttack = false; // If true, attacks all enemies in range; otherwise, attacks the closest
    public Slider healthSlider;
    Animator animator;

    private Transform target; // Closest target if aoeAttack is false
    private float nextAttackTime = 0f;

    private void Start()
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }

    private void Update()
    {
        // Update the health slider position above the unit
        healthSlider.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1, 0));

        if (!aoeAttack)
        {
            // Find the closest enemy within range
            FindClosestEnemy();
        }

        // Move or attack based on the presence of a target
        if (target == null && !aoeAttack)
        {
            // No target: move forward
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else if (Time.time >= nextAttackTime)
        {
            // Perform attack
            if (aoeAttack)
            {
                AttackAllInRange();
            }
            else
            {
                Attack();
            }
        }
    }

    private void FindClosestEnemy()
    {
        float closestDistance = attackRange;
        target = null; // Reset target

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= closestDistance)
            {
                closestDistance = distance;
                target = enemy.transform;
            }
        }
    }

    private void Attack()
    {
        if (target != null)
        {
            EnemyUnit enemy = target.GetComponent<EnemyUnit>();
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

        nextAttackTime = Time.time + attackCooldown; // Set cooldown
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
