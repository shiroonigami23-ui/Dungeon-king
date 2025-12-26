using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject monsterPrefab;
    public float spawnRate = 3f;
    public int maxMonsters = 10;
    
    [Header("Range")]
    public float spawnRadius = 5f;

    private float nextSpawnTime;

    void Update()
    {
        // Only spawn if we haven't hit the limit and it's time
        if (Time.time >= nextSpawnTime && GameObject.FindGameObjectsWithTag("Enemy").Length < maxMonsters)
        {
            SpawnMonster();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnMonster()
    {
        // Generate a random position within a circle around the spawner
        Vector2 randomPos = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
        
        Instantiate(monsterPrefab, randomPos, Quaternion.identity);
    }

    // Visualize the spawn area in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}