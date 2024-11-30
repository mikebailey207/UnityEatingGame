using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleWithColliders : MonoBehaviour
{
    LineRenderer tentacleRenderer;
    public GameObject dog; // Reference to the dog object
  //  public GameObject gameOverScreen; // Reference to the Game Over UI or logic
    private List<BoxCollider2D> colliders = new List<BoxCollider2D>();

    void Start()
    {
        if (tentacleRenderer == null)
        {
            Debug.LogError("Tentacle Renderer not assigned!");
            return;
        }
        tentacleRenderer = GetComponent<LineRenderer>();
        // Initialize colliders for each segment
        for (int i = 0; i < tentacleRenderer.positionCount - 1; i++)
        {
            var collider = gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            colliders.Add(collider);
        }
    }

    void Update()
    {
        UpdateColliders();
    }

    void UpdateColliders()
    {
        if (colliders.Count == 0) return;

        for (int i = 0; i < colliders.Count; i++)
        {
            // Get positions for this segment
            Vector3 start = tentacleRenderer.GetPosition(i);
            Vector3 end = tentacleRenderer.GetPosition(i + 1);

            // Calculate midpoint and rotation
            Vector3 midPoint = (start + end) / 2f;
            float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;

            // Update collider
            var collider = colliders[i];
            collider.transform.position = midPoint;
            collider.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Adjust size to match the segment
            float length = Vector3.Distance(start, end);
            collider.size = new Vector2(length, 1f); // Adjust thickness as needed
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == dog)
        {
            Debug.Log("Game Over! Dog touched a tentacle.");
            // Trigger Game Over logic
          
        }
    }
}
