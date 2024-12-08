using UnityEngine;
using TMPro;

public class CurrencySystem : MonoBehaviour
{
    public int startingCurrency = 0; // Initial currency amount
    public float currencyRate = 1f; // Amount of currency generated per interval
    public float productionInterval = 1f; // Time interval for currency production
    public TMP_Text currencyText; // UI Text to display the currency amount

    public int currentCurrency;
    private float productionTimer;

    void Start()
    {
        currentCurrency = startingCurrency;
        UpdateCurrencyText();
        productionTimer = productionInterval; // Initialize the production timer
    }

    void Update()
    {
        // Generate currency over time
        productionTimer -= Time.deltaTime;
        if (productionTimer <= 0)
        {
            AddCurrency((int)currencyRate);
            productionTimer = productionInterval; // Reset the timer
        }
    }

    public void AddCurrency(int amount)
    {
        currentCurrency += amount;
        UpdateCurrencyText();
    }

    private void UpdateCurrencyText()
    {
        if (currencyText != null)
        {
            currencyText.text = $"Oxygen: {currentCurrency}";
        }
        else
        {
            Debug.LogWarning("Currency text UI element is not assigned.");
        }
    }

    public void EnemyKilled(int reward)
    {
        AddCurrency(reward);
    }
}
