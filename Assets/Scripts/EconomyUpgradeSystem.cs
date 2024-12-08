using UnityEngine;
using UnityEngine.UI;

public class EconomyUpgradeSystem : MonoBehaviour
{
    public int currentLevel = 1; // Current upgrade level
    public float upgradeCostMultiplier = 1.5f; // Multiplier for upgrade cost growth
    public int baseUpgradeCost = 50; // Cost of the first upgrade
    public Text levelText; // UI Text to display the current level
    public Text costText; // UI Text to display the upgrade cost
    public CurrencySystem currencyManager; // Reference to the currency manager
    public Button upgradeButton; // Reference to the upgrade button

    private int currentUpgradeCost; // Current upgrade cost

    void Start()
    {
        currentUpgradeCost = baseUpgradeCost;
        UpdateUI();
        currencyManager = GetComponent<CurrencySystem>();
    }

    void UpdateUI()
    {
        levelText.text = "Level: " + currentLevel;
        costText.text = "Cost: " + currentUpgradeCost;

        // Enable or disable the upgrade button based on currency
        upgradeButton.interactable = currencyManager.currentCurrency >= currentUpgradeCost;
    }

    void Update()
    {
        // Update button interactability dynamically
        UpdateUI();
    }

    public void UpgradeLevel()
    {
        if (currencyManager.currentCurrency >= currentUpgradeCost)
        {
            // Deduct the cost
            currencyManager.AddCurrency(-currentUpgradeCost);

            // Upgrade level and income rate
            currentLevel++;
            currencyManager.currencyRate += 25f * currentLevel; 
            currentUpgradeCost = Mathf.CeilToInt(baseUpgradeCost * Mathf.Pow(upgradeCostMultiplier, currentLevel - 1));

            // Update the UI
            UpdateUI();
        }
    }
}
