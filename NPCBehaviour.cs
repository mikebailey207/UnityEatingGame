using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    public float moveSpeed = 2f;
    float changeDirectionInterval; // Time interval for changing direction
    public float stopDuration = 2f; // Stop duration when hitting an obstacle
    public float npcStopTime = 5f; // Time to stop when bumping into another NPC

    private Vector2 targetDirection;
    private Rigidbody2D rb;
    private bool isStopped = false;

    void Start()
    {
        changeDirectionInterval = Random.Range(2, 3);
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(ChangeDirectionRoutine()); // Start direction change coroutine
    }

    void Update()
    {
        if (!isStopped)
        {
            // Move in the current direction
            rb.velocity = targetDirection * moveSpeed;
        }
        else
        {
            // Stop movement
            rb.velocity = Vector2.zero;
        }
    }

    private IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeDirectionInterval);

            if (!isStopped)
            {
                // Change to a random direction
                float randomAngle = Random.Range(0f, 360f);
                targetDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            
            StartCoroutine(StopMovementForSeconds(stopDuration));
        }
        else if (collision.gameObject.CompareTag("NPC"))
        {
            StartCoroutine(StopMovementForSeconds(npcStopTime));
        }
    }

    private IEnumerator StopMovementForSeconds(float duration)
    {
        isStopped = true;
        yield return new WaitForSeconds(duration);
        isStopped = false;
    }
}
