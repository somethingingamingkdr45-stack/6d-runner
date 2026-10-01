using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Setup")]
    public GameObject[] concretePrefabs; 
    public Transform player;
    public WallRunner wallRunner;

    [Header("Dynamic Spawn Settings")]
    public float lookAheadSeconds = 3.5f; 
    public float spawnInterval = 1.5f;    
    public float roadWidthLimit = 35f;

    [Header("Size Scaling (Minimum 15)")]
    public float minSize = 15f;
    public float maxSize = 25f;

    private float timer = 0f;
    
    // Tracks the spawned objects so the spawner can delete them when passed
    private List<GameObject> activeObstacles = new List<GameObject>(); 

    void Update()
    {
        if (player == null || wallRunner == null || concretePrefabs == null || concretePrefabs.Length == 0) return;

        // 1. Spawning
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnObstacleAhead();
            timer = 0f;
        }

        // 2. Automatic Cleanup (Deletes obstacles that fall 20 meters behind David)
        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            if (activeObstacles[i] == null)
            {
                activeObstacles.RemoveAt(i);
                continue;
            }

            Vector3 toObstacle = activeObstacles[i].transform.position - player.position;
            if (Vector3.Dot(toObstacle, player.forward) < -20f)
            {
                Destroy(activeObstacles[i]);
                activeObstacles.RemoveAt(i);
            }
        }
    }

    void SpawnObstacleAhead()
    {
        int randomIndex = Random.Range(0, concretePrefabs.Length);
        GameObject selectedPrefab = concretePrefabs[randomIndex];
        if (selectedPrefab == null) return;

        float dynamicDistance = Mathf.Max(wallRunner.currentSpeed * lookAheadSeconds, 150f);
        float randomLaneX = Random.Range(-roadWidthLimit, roadWidthLimit);

        Vector3 spawnPos = player.position 
                         + (player.forward * dynamicDistance) 
                         + (player.right * randomLaneX);

        GameObject spawned = Instantiate(selectedPrefab, spawnPos, player.rotation);
        
        float chosenScale = Random.Range(minSize, maxSize);
        spawned.transform.localScale = Vector3.one * chosenScale;

        // Add to our tracker list for cleanup
        activeObstacles.Add(spawned);
    }
}