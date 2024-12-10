using UnityEngine;
using UnityEngine.UI;

public class PlayerUnit : MonoBehaviour
{
    public float speed = 2.0f;
    public int health = 100;
    public int attackDamage = 10;
    public float attackRange = 1.0f;
    public float attackCooldown = 1.0f;
    public Slider healthSlider;
    public Animator animator;

    private Transform target;
    private float nextAttackTime = 0f;

    private void Start()
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }

    private void Update()
    {
        healthSlider.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1, 0));

        if (target == null)
        {
            transform.position += speed * Time.deltaTime * Vector3.right;
        }
        else if (Time.time >= nextAttackTime)
        {
            Attack();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Parasite"))
        {
            target = other.transform;
        }
        else if (other.CompareTag("EnemyTower"))
        {
            target = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && target == other.transform)
        {
            target = null;
        }
    }

    private void Attack()
    {
        if (target != null && target.CompareTag("Enemy") || target.CompareTag("Parasite"))
        {
            EnemyUnit enemy = target.GetComponent<EnemyUnit>();
            if (enemy != null)
            {
                animator.SetTrigger("Attack");
                enemy.TakeDamage(attackDamage);
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else if (target != null && target.CompareTag("EnemyTower"))
        {
            TowerHealth enemyTower = target.GetComponent<TowerHealth>();
            if (enemyTower != null)
            {
                animator.SetTrigger("Attack");
                enemyTower.TakeDamage(attackDamage);
                nextAttackTime += Time.time + attackCooldown;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthSlider.value = health;

        if (health <= 0)
        {
            if (healthSlider != null)
            {
                Destroy(healthSlider.gameObject);
            }

            animator.SetTrigger("Die");
            Destroy(gameObject);
        }
    }
}
