using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : MonoBehaviour
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
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else if (Time.time >= nextAttackTime)
        {
            Attack();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerUnit"))
        {
            target = other.transform;
        }
        else if (other.CompareTag("PlayerTower"))
        {
            target = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerUnit") && target == other.transform)
        {
            target = null;
        }
    }

    private void Attack()
    {
        if (target != null)
        {
            PlayerUnit player = target.GetComponent<PlayerUnit>();
            if (player != null)
            {
                player.TakeDamage(attackDamage);
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else
        {

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

            Destroy(gameObject);
        }
    }
}
