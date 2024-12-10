using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform spawnPoint; // Location to spawn enemies
    public GameObject[] enemyPrefabs; // Array of enemy prefabs
    public float minSpawnInterval = 2f; // Minimum time between spawns
    public float maxSpawnInterval = 5f; // Maximum time between spawns

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private System.Collections.IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Wait for a random interval before spawning the next enemy
            float spawnDelay = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnDelay);

            // Randomly pick an enemy type from the array
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
        }
    }
}
