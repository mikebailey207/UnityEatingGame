using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehaviour : MonoBehaviour
{
    bool canSpawnYet = false;
    private bool hasSpawnedBird = false;
    public GameObject bird;
    private SpriteRenderer spriteRenderer;
    private float originalAlpha;
    private int originalSortingOrder;

    private HashSet<Collider2D> collidersInTrigger = new HashSet<Collider2D>();

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalAlpha = spriteRenderer.color.a;
        originalSortingOrder = spriteRenderer.sortingOrder;

        if (!GetComponent<Collider2D>() || !GetComponent<Collider2D>().isTrigger)
        {
            Debug.LogWarning("Tree object needs a trigger Collider2D!");
        }
    }
    private void Start()
    {
        Invoke("CanSpawnNow", 2);
    }
    private void Update()
    {
        bool isAbove = false;

        foreach (var collider in collidersInTrigger)
        {
            if (collider == null) continue;

            float otherY = collider.transform.position.y;
            float treeY = transform.position.y;

            if (otherY > treeY)
            {
                isAbove = true;
                SpriteRenderer otherRenderer = collider.GetComponent<SpriteRenderer>();
                if (otherRenderer != null)
                {
                    SetTransparency(otherRenderer, 0.5f);
                }
            }
        }

        if (isAbove)
        {
            SetTransparency(spriteRenderer, 0.5f);
        }
        else
        {
            SetTransparency(spriteRenderer, originalAlpha);
        }
    }
    void CanSpawnNow()
    {
        canSpawnYet = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasSpawnedBird && canSpawnYet)
        {
            hasSpawnedBird = true;

            // Instantiate the bird at the current position
            GameObject birdInstance = Instantiate(bird, transform.position, Quaternion.identity);

            // Get the Rigidbody2D and SpriteRenderer components from the instantiated bird
            Rigidbody2D birdRb = birdInstance.GetComponent<Rigidbody2D>();
            SpriteRenderer birdSprite = birdInstance.GetComponent<SpriteRenderer>();

            if (birdRb != null && birdSprite != null)
            {
                // Generate a random direction
                Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

                // Apply a force to the bird's Rigidbody2D to move it in the random direction
                float forceMagnitude = Random.Range(2f, 5f); // Adjust force range as needed
                birdRb.AddForce(randomDirection * forceMagnitude, ForceMode2D.Impulse);

                // Flip the bird based on the x-direction
                birdSprite.flipX = randomDirection.x < 0;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!collidersInTrigger.Contains(other))
        {
            collidersInTrigger.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (collidersInTrigger.Contains(other))
        {
            SpriteRenderer otherRenderer = other.GetComponent<SpriteRenderer>();
            if (otherRenderer != null)
            {
                SetTransparency(otherRenderer, 1.0f);
            }

            collidersInTrigger.Remove(other);
        }
    }

    private void SetTransparency(SpriteRenderer targetRenderer, float alpha)
    {
        Color color = targetRenderer.color;
        color.a = alpha;
        targetRenderer.color = color;
    }
}