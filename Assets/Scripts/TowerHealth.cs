using UnityEngine;
using UnityEngine.UI;

public class TowerHealth : MonoBehaviour
{
    public int health = 500;
    public Slider healthSlider;

    private void Start()
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthSlider.value = health;

        if (health <= 0)
        {
            Debug.Log($"{gameObject.name} has been destroyed!");
            Destroy(gameObject);
        }
    }
}
