using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiSpawner : MonoBehaviour
{
    public GameObject[] sushiPrefab;
    int index;
    public Transform leftSpawnPoint;   // Assign this above the screen on the left
    public Transform rightSpawnPoint;  // Assign this below the screen on the right
    public float spawnIntervalMin = 0.5f;
    public float spawnIntervalMax = 1.5f;
    public float sushiSpeed = 3f;

    void Start()
    {
        StartCoroutine(SpawnSushiRoutine());
    }

    IEnumerator SpawnSushiRoutine()
    {
        while (true)
        {
            // Wait for a random interval between min and max
            float spawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(spawnInterval);

            // Choose random spawn point
            bool spawnOnLeft = Random.Range(0, 2) == 0;

            if (spawnOnLeft)
            {
                index = Random.Range(0, sushiPrefab.Length);
                // Spawn sushi at the left spawn point
                GameObject sushi = Instantiate(sushiPrefab[index], leftSpawnPoint.position, Quaternion.identity);
                Rigidbody2D rb = sushi.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // Make sure it moves downwards from the left spawn point
                    rb.velocity = new Vector2(0, -sushiSpeed);
                }
            }
            else
            {
                index = Random.Range(0, sushiPrefab.Length);
                // Spawn sushi at the right spawn point
                GameObject sushi = Instantiate(sushiPrefab[index], rightSpawnPoint.position, Quaternion.identity);
                Rigidbody2D rb = sushi.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // Make sure it moves upwards from the right spawn point
                    rb.velocity = new Vector2(0, sushiSpeed);
                }
            }
        }
    }
}