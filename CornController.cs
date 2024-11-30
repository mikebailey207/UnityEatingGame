using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornController : MonoBehaviour
{
    public float moveSpeed = 2f;           // Speed of movement
    private Vector2 randomDirection;       // Direction of movement
    private Vector2 screenBounds;          // Screen bounds for collision
    private float rotationSpeed;           // Random rotation speed for each piece

    void Start()
    {
        // Initialize screen bounds
        Camera mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        // Set the initial direction and rotation speed
        ChangeDirection();
        rotationSpeed = Random.Range(-100f, 100f); // Set a random rotation speed
    }

    void Update()
    {
        // Move the corn in the random direction
        transform.Translate(randomDirection * moveSpeed * Time.deltaTime);

        // Apply the random rotation to the corn (in degrees per second)
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        // Check if the corn goes out of bounds
        CheckBounds();
    }

    void ChangeDirection()
    {
        // Set a new random direction for the movement
        randomDirection = Random.insideUnitCircle.normalized;
    }

    void CheckBounds()
    {
        Vector3 pos = transform.position;

        // Get the object's size (half of the width and height)
        float objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        float objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;

        // Check if corn is out of bounds on the x-axis
        if (pos.x + objectWidth > screenBounds.x || pos.x - objectWidth < -screenBounds.x)
        {
            randomDirection.x = -randomDirection.x;  // Reflect the x direction
            pos.x = Mathf.Clamp(pos.x, -screenBounds.x + objectWidth, screenBounds.x - objectWidth);
        }

        // Check if corn is out of bounds on the y-axis
        if (pos.y + objectHeight > screenBounds.y || pos.y - objectHeight < -screenBounds.y)
        {
            randomDirection.y = -randomDirection.y;  // Reflect the y direction
            pos.y = Mathf.Clamp(pos.y, -screenBounds.y + objectHeight, screenBounds.y - objectHeight);
        }

        // Apply the adjusted position
        transform.position = pos;
    }
}