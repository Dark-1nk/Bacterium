using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] phaseOneUnitPrefabs; // Array to store multiple unit prefabs
    public GameObject[] phaseTwoUnitPrefabs; // Array to store multiple unit prefabs
    public GameObject[] phaseThreeUnitPrefabs; // Array to store multiple unit prefabs
    public Transform spawnPoint; // Location to spawn units
    public float minSpawnInterval = 1f; // Minimum time between spawns
    public float maxSpawnInterval = 5f; // Maximum time between spawns
    private float spawnTimer;
    TowerHealth tower;

    void Start()
    {
        // Initialize the spawn timer with a random interval
        tower = FindObjectOfType<TowerHealth>();
        spawnTimer = GetRandomInterval();
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            SpawnUnit();
            spawnTimer = GetRandomInterval(); // Reset the timer with a new random interval
        }
    }

    private void SpawnUnit()
    {
        if (tower.currentHealth <= tower.maxHealth )
        {
            if (phaseOneUnitPrefabs.Length > 0)
            {
                // Randomly select a unit prefab from the array
                int randomIndex = Random.Range(0, phaseOneUnitPrefabs.Length);
                GameObject unitPrefab = phaseOneUnitPrefabs[randomIndex];

                // Instantiate the selected unit at the spawn point
                Instantiate(unitPrefab, spawnPoint.position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("No unit prefabs assigned to the spawner.");
            }
        }
        if (tower.currentHealth < tower.maxHealth * 0.6)
        {
            if (phaseTwoUnitPrefabs.Length > 0)
            {
                // Randomly select a unit prefab from the array
                int randomIndex = Random.Range(0, phaseTwoUnitPrefabs.Length);
                GameObject unitPrefab = phaseTwoUnitPrefabs[randomIndex];

                // Instantiate the selected unit at the spawn point
                Instantiate(unitPrefab, spawnPoint.position, Quaternion.identity);
            }

            else
            {
                Debug.LogWarning("No unit prefabs assigned to the spawner.");
            }
        }
        if (tower.currentHealth < tower.maxHealth * 0.3)
        {
            if (phaseThreeUnitPrefabs.Length > 0)
            {
                // Randomly select a unit prefab from the array
                int randomIndex = Random.Range(0, phaseThreeUnitPrefabs.Length);
                GameObject unitPrefab = phaseThreeUnitPrefabs[randomIndex];

                // Instantiate the selected unit at the spawn point
                Instantiate(unitPrefab, spawnPoint.position, Quaternion.identity);
            }

            else
            {
                Debug.LogWarning("No unit prefabs assigned to the spawner.");
            }
        }
    }

    private float GetRandomInterval()
    {
        // Generate a random time interval between minSpawnInterval and maxSpawnInterval
        return Random.Range(minSpawnInterval, maxSpawnInterval);
    }
}
