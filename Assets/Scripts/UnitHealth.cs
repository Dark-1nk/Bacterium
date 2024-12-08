using UnityEngine;
using UnityEngine.UI;

public class UnitHealth : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;
    public Animator animator;
    CurrencySystem currencySystem;
    EnemyUnit enemyUnit;
    public int killReward;
    public Slider healthBar;


    void Start()
    {
        currentHealth = maxHealth;
        currencySystem = FindObjectOfType<CurrencySystem>();
        enemyUnit = GetComponent<EnemyUnit>();

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} Health: {currentHealth}");

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        if (enemyUnit != null)
        {
            currencySystem.EnemyKilled(killReward);
            animator.SetTrigger("Die");
            Destroy(gameObject);
            Debug.Log($"Enemy {gameObject.name} has been destroyed!");
        }

        else
        {
            animator.SetTrigger("Die");
            Destroy(gameObject);
            Debug.Log($"Unit {gameObject.name} has been destroyed!");
        }
    }
}
