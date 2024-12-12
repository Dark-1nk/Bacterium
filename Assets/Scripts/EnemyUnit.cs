using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : MonoBehaviour
{
    public float speed = 2.0f;
    public int health = 100;
    public int attackDamage = 10;
    public float attackRange = 1.5f; // Range within which the unit can attack
    public float attackCooldown = 1.0f;
    public bool aoeAttack = false; // If true, attacks all units in range; otherwise, attacks the closest
    public Slider healthSlider;
    Animator animator;
    private bool isAttacking = false;

    private Transform target; // Closest target if aoeAttack is false
    private float nextAttackTime = 0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }

    private void Update()
    {
        // Update the health slider position above the unit
        healthSlider.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1, 0));

        if (isAttacking)
        {
            return;
        }

        transform.position += speed * Time.deltaTime * Vector3.right;


        if (aoeAttack)
        {
            if (AnyEnemyInRange())
            {
                isAttacking = true;
                if (Time.time >= nextAttackTime)
                {
                    AttackAllInRange();
                }
            }
            else
            {
                isAttacking = false;
            }
        }
        else if (!aoeAttack)
        {
            FindClosestPlayerUnit();
            if (target != null)
            {
                if (Time.time >= nextAttackTime)
                {
                    Attack();
                }
            }
            else
            {
                isAttacking = false;
            }
        }
    }

    private void FindClosestPlayerUnit()
    {
        float closestDistance = attackRange;
        target = null; // Reset target

        foreach (GameObject playerUnit in GameObject.FindGameObjectsWithTag("PlayerUnit"))
        {
            float distance = Vector2.Distance(transform.position, playerUnit.transform.position);
            if (distance <= closestDistance)
            {
                closestDistance = distance;
                target = playerUnit.transform;
                isAttacking = true;
            }
        }
    }

    private void Attack()
    {
        if (target != null)
        {
            PlayerUnit player = target.GetComponent<PlayerUnit>();
            if (player != null)
            {
                animator.SetTrigger("Attack");
                player.TakeDamage(attackDamage);
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    private bool AnyEnemyInRange()
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

    private void AttackAllInRange()
    {
        foreach (GameObject playerUnit in GameObject.FindGameObjectsWithTag("PlayerUnit"))
        {
            float distance = Vector2.Distance(transform.position, playerUnit.transform.position);
            if (distance <= attackRange)
            {
                PlayerUnit player = playerUnit.GetComponent<PlayerUnit>();
                if (player != null)
                {
                    animator.SetTrigger("Attack");
                    player.TakeDamage(attackDamage);
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
