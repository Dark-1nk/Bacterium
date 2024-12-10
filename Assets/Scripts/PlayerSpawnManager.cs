using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public Transform spawnPoint; // Position to spawn units
    public GameObject[] unitPrefabs; // Array of player unit prefabs
    public int[] unitCosts; // Costs for each unit
    private CurrencyManager currencyManager; // Reference to CurrencyManager

    private void Start()
    {
        currencyManager = FindObjectOfType<CurrencyManager>();
        if (currencyManager == null)
        {
            Debug.LogError("CurrencyManager not found in the scene!");
        }
    }

    public void SpawnUnit(int unitIndex)
    {
        if (unitIndex < 0 || unitIndex >= unitPrefabs.Length)
        {
            Debug.LogError("Invalid unit index!");
            return;
        }

        if (currencyManager != null && currencyManager.GetCurrency() >= unitCosts[unitIndex])
        {
            Instantiate(unitPrefabs[unitIndex], spawnPoint.position, Quaternion.identity);
            currencyManager.SpendCurrency(unitCosts[unitIndex]);
        }
        else
        {
            Debug.Log("Not enough currency to spawn this unit.");
        }
    }
}
