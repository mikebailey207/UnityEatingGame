using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnOffset = 2f;
    public GameObject badgerPrefab; // Badger woman prefab
    public GameObject octopusPrefab; // Octopus prefab
    public DogRandomMovement dogMovement; // Reference to DogRandomMovement script
    public Transform[] spawnPoints; // Define spawn points
    public float initialSpawnInterval = 3f; // Starting spawn interval in seconds
    public float spawnIntervalReduction = 0.05f; // Rate at which spawn interval decreases per day
    public float initialEnemySpeed = 2f; // Starting speed of enemies
    public float speedIncreasePerDay = 0.1f; // Speed increase per day

    private float spawnInterval; // Current spawn interval
    private float enemySpeed; // Current enemy speed
    private bool spawningEnemies; // Whether enemies are currently spawning

    void Start()
    {
        // Set initial spawn interval and enemy speed
        spawnInterval = initialSpawnInterval;
        enemySpeed = initialEnemySpeed;
    }

    void Update()
    {
        // Check if walkies is active
        if (GameManager.Instance.day >= 4 && dogMovement.walkies)
        {
            if (!spawningEnemies)
            {
                spawningEnemies = true;
                StartCoroutine(SpawnEnemies());
            }
        }
        else
        {
            spawningEnemies = false;
            StopAllCoroutines(); // Stop spawning when walkies ends
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (dogMovement.walkies) // Continue spawning as long as walkies is active
        {
            if (GameManager.Instance.day >= 4 && GameManager.Instance.day < 8)
            {
                // Spawn only badgers
                SpawnBadger();
            }
            else if (GameManager.Instance.day >= 8 && GameManager.Instance.day < 12)
            {
                // Spawn only octopuses
                SpawnOctopus();
            }
            else if (GameManager.Instance.day >= 12)
            {
                // Spawn both badgers and octopuses
                SpawnBadger();
                SpawnOctopus();
            }

            yield return new WaitForSeconds(spawnInterval);

            // Adjust spawn interval and enemy speed based on the day
            spawnInterval = Mathf.Max(1f, initialSpawnInterval - (GameManager.Instance.day - 4) * spawnIntervalReduction);
            enemySpeed = initialEnemySpeed + (GameManager.Instance.day - 4) * speedIncreasePerDay;
        }
    }

    void SpawnBadger()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        GameObject badger = Instantiate(badgerPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
        EnemyMovement badgerScript = badger.GetComponent<EnemyMovement>();
        badgerScript.target = dogMovement.transform;
        badgerScript.moveSpeed = enemySpeed;
    }

    void SpawnOctopus()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        GameObject octopus = Instantiate(octopusPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
        MalevolentOctopus octopusScript = octopus.GetComponent<MalevolentOctopus>();
        octopusScript.target = dogMovement.transform;
        //octopusScript.moveSpeed = enemySpeed; // Uncomment if octopuses also use enemy speed
    }
}