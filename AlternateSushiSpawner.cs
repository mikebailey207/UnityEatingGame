using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateSushiSpawner : MonoBehaviour
{
    public TongueController tongue;
    public GameObject[] sushiPrefab;
    int index;
    public Transform leftSpawnPoint;   // Assign this above the screen on the left
    public Transform rightSpawnPoint;  // Assign this below the screen on the right
    public float sushiSpeed = 3f;

    void Start()
    {
        StartCoroutine(SpawnSushiRoutine());
    }
    private void Update()
    {
        if(tongue.gameActive == false)
        {
            StopAllCoroutines();
        }
    }
    IEnumerator SpawnSushiRoutine()
    {
       
            while (true)
            {
                // Randomly choose one of the three patterns
                int patternChoice = Random.Range(0, 3);

                switch (patternChoice)
                {
                    case 0:
                        // Pattern 1: Left, Right, Left, Right, etc.
                        yield return StartCoroutine(SpawnPattern1());
                        break;

                    case 1:
                        // Pattern 2: Left, Left, Right, Right, etc.
                        yield return StartCoroutine(SpawnPattern2());
                        break;

                    case 2:
                        // Pattern 3: Left, Left, Left, Left, Right, Right, Right, Right, etc.
                        yield return StartCoroutine(SpawnPattern3());
                        break;
                }

                // No additional wait here, the spawning interval is now controlled within the patterns
            }
      
    }

    IEnumerator SpawnPattern1()
    {
        // Alternating Left and Right spawn (every 0.58 seconds)
        SpawnSushi(true);  // Spawn on Left
        yield return new WaitForSeconds(0.58f);

        SpawnSushi(false);  // Spawn on Right
        yield return new WaitForSeconds(0.58f);
    }

    IEnumerator SpawnPattern2()
    {
        // Two Left, Two Right pattern (every 0.58 seconds)
        SpawnSushi(true);  // Spawn on Left
        yield return new WaitForSeconds(0.58f);

        SpawnSushi(true);  // Spawn on Left
        yield return new WaitForSeconds(0.58f);

        SpawnSushi(false);  // Spawn on Right
        yield return new WaitForSeconds(0.58f);

        SpawnSushi(false);  // Spawn on Right
        yield return new WaitForSeconds(0.58f);
    }

    IEnumerator SpawnPattern3()
    {
        // Four Left, Four Right pattern (every 0.58 seconds)
        for (int i = 0; i < 4; i++)
        {
            SpawnSushi(true);  // Spawn on Left
            yield return new WaitForSeconds(0.58f);
        }

        for (int i = 0; i < 4; i++)
        {
            SpawnSushi(false);  // Spawn on Right
            yield return new WaitForSeconds(0.58f);
        }
    }

    void SpawnSushi(bool spawnOnLeft)
    {
        index = Random.Range(0, sushiPrefab.Length);
        GameObject sushi;

        if (spawnOnLeft)
        {
            sushi = Instantiate(sushiPrefab[index], leftSpawnPoint.position, Quaternion.identity);
            Rigidbody2D rb = sushi.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = new Vector2(0, -sushiSpeed);  // Move downwards from the left spawn point
            }
        }
        else
        {
            sushi = Instantiate(sushiPrefab[index], rightSpawnPoint.position, Quaternion.identity);
            Rigidbody2D rb = sushi.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = new Vector2(0, sushiSpeed);  // Move upwards from the right spawn point
            }
        }
    }
}