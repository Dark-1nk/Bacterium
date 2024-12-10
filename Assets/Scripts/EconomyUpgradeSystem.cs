using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    public int upgradeCost = 50;
    public int passiveGainIncrease = 1;
    public Button upgradeButton;
    private CurrencyManager currencyManager;

    private void Start()
    {
        currencyManager = FindObjectOfType<CurrencyManager>();
        upgradeButton.onClick.AddListener(UpgradeEconomy);
    }

    private void UpgradeEconomy()
    {
        if (currencyManager.GetCurrency() >= upgradeCost)
        {
            currencyManager.SpendCurrency(upgradeCost);
            currencyManager.passiveGain += passiveGainIncrease;
            Debug.Log("Economy upgraded!");
        }
        else
        {
            Debug.Log("Not enough currency to upgrade.");
        }
    }
}
