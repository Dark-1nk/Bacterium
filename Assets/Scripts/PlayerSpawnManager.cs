using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class Unit
    {
        public GameObject prefab; // Unit prefab
        public int price; // Cost to spawn the unit
    }

    public Unit[] units; // Array of units available to spawn
    public Transform spawnPoint; // Spawn location for player units
    public CurrencySystem currencySystem; // Reference to the currency system

    public Button[] unitButtons; // UI buttons for each unit type

    void Start()
    {
        // Set up button listeners
        for (int i = 0; i < unitButtons.Length; i++)
        {
            int index = i; // Capture the index for the listener
            unitButtons[i].onClick.AddListener(() => TrySpawnUnit(index));
        }
    }

    public void TrySpawnUnit(int unitIndex)
    {
        if (unitIndex < 0 || unitIndex >= units.Length)
        {
            Debug.LogWarning("Invalid unit index.");
            return;
        }

        Unit unit = units[unitIndex];

        // Check if the player has enough currency
        if (currencySystem != null && currencySystem.currentCurrency >= unit.price)
        {
            // Deduct the currency and spawn the unit
            currencySystem.AddCurrency(-unit.price);
            Instantiate(unit.prefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Not enough currency to spawn this unit.");
        }
    }
}
