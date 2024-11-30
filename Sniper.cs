using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour
{
    public GameObject dog;
    public Transform player; // Reference to the player (to lock on to)
    public LineRenderer lineRenderer; // The line representing the sniper's aim
    public GameObject bulletPrefab; // The bullet prefab the sniper fires
    public float rotationSpeed = 2f; // Speed at which the sniper rotates towards the player
    public float shotDelay = 1f; // Delay before the sniper shoots after locking on
    public float bulletSpeed = 10f; // Speed of the sniper's bullet
    public float maxLineLength = 100f; // Length of the line (line will keep extending towards player)
    public float respawnDistance = 20f; // Distance at which the sniper will respawn from the player
    private bool isLockedOn = false; // Whether the sniper has locked on to the player
    private Vector3 directionToPlayer; // Direction to the player
    private Vector3 randomSpawnPosition; // Random respawn position for the sniper

    void Start()
    {
        // Ensure LineRenderer is initialized properly
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        // Initialize the line renderer's start and end positions
        lineRenderer.positionCount = 2; // Two points: start and end
        lineRenderer.SetPosition(0, transform.position); // Start position is the sniper's position
        lineRenderer.SetPosition(1, transform.position); // End position starts at the sniper

        // Set initial line settings
      lineRenderer.startWidth = 0.04f;
        lineRenderer.endWidth = 0.04f;

        // Respawn at a random position far from the player
        RespawnAtRandomPosition();
    }

    void Update()
    {
        if (player != null && dog.GetComponent<DogRandomMovement>().walkies && GameManager.Instance.day >= 160)
        {
            // Calculate the direction to the player
            directionToPlayer = player.position - transform.position;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

            // Gradually rotate the sniper towards the player
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Update the line to point towards the player
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, player.transform.position);

            // Once the sniper has locked on (i.e., direction to player is close enough)
            if (!isLockedOn && directionToPlayer.magnitude <= maxLineLength)
            {
                isLockedOn = true;
                StartCoroutine(FireBulletAfterDelay());
            }
        }
    }

    // Waits for the shot delay and then fires the bullet
    IEnumerator FireBulletAfterDelay()
    {
        // Wait for the delay before shooting
        yield return new WaitForSeconds(shotDelay);

        // Fire the bullet towards the player
        FireBullet();

        // Wait for a couple of seconds after firing before respawning
        yield return new WaitForSeconds(2f);

        // Respawn at a random position far from the player
        RespawnAtRandomPosition();

        // Reset locked-on state and start the cycle again
        isLockedOn = false;
    }

    void FireBullet()
    {
        // Instantiate the bullet at the sniper's position
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Get the direction from the sniper to the player
        Vector3 fireDirection = directionToPlayer.normalized;

        // Get the Rigidbody2D of the bullet and apply velocity towards the player
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = fireDirection * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("Bullet prefab does not have Rigidbody2D component.");
        }
    }

    void RespawnAtRandomPosition()
    {
        // Find a random direction far from the player
        float randomAngle = Random.Range(0f, 360f);
        Vector3 offset = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0f) * respawnDistance;

        // Set the sniper's new position far from the player
        transform.position = player.position + offset;

        // Make sure the sniper points in a random direction when it spawns
        float randomSpawnAngle = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0, 0, randomSpawnAngle); // Point in a random direction
    }
}






