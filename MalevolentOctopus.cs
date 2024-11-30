using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalevolentOctopus : MonoBehaviour
{
    AudioSource hitSound, deathSound;
    public int lives = 3; // Number of lives for the octopus
    public float knockbackForce = 1f; // Force to apply when hit
    public LineRenderer[] tentacles; // Assign 8 tentacles in the inspector
    public int segments = 10; // Number of segments per tentacle
    public float segmentLength = 0.5f; // Length of each segment
    public float baseSpeed = 1f; // Base speed for tentacle movement
    public float rushSpeed = 10f; // Speed during the rush
    public float rotationSpeed = 360f; // Rotation speed during the rush
    public float pauseDistance = 15f; // Distance at which octopus pauses
    public float pauseDuration = 2f; // Duration of pause before rushing
    public Transform target; // Reference to the dog (target)

    private Vector3[] tentacleBasePositions; // Base positions for each tentacle
    private float[] movementOffsets; // Unique offsets for each tentacle
    private bool isPausing = false; // Whether the octopus is currently pausing
    private bool isRushing = false; // Whether the octopus is currently rushing
    private float pauseTimer = 0f; // Timer for pause duration

    void Start()
    {
        hitSound = GameObject.Find("HitSound").GetComponent<AudioSource>();
        deathSound = GameObject.Find("DeathSound").GetComponent<AudioSource>();

        GameObject particles = Resources.Load<GameObject>("OctopusParticles"); // Path to prefab inside the Resources folder
        if (particles != null)
        {
            Instantiate(particles, transform.position, Quaternion.identity);
        }
        // Initialize tentacle base positions and unique offsets
        tentacleBasePositions = new Vector3[tentacles.Length];
        movementOffsets = new float[tentacles.Length];

        for (int i = 0; i < tentacles.Length; i++)
        {
            tentacleBasePositions[i] = tentacles[i].transform.localPosition;

            // Give each tentacle a unique offset for timing
            movementOffsets[i] = Random.Range(0f, 10f);
        }
    }

    void Update()
    {
        if (GameManager.Instance.walkiesComplete)
        {
            GameObject particles = Resources.Load<GameObject>("OctopusParticles"); // Path to prefab inside the Resources folder
            if (particles != null)
            {
                Instantiate(particles, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
        if (target != null)
        {
            // Calculate distance to target
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (!isPausing && !isRushing)
            {
                if (distanceToTarget <= pauseDistance)
                {
                    // Start pausing
                    isPausing = true;
                    pauseTimer = pauseDuration;
                }
                else
                {
                    // Move towards the target at base speed
                    Vector3 direction = (target.position - transform.position).normalized;
                    transform.position += direction * Time.deltaTime * baseSpeed;
                }
            }
            else if (isPausing)
            {
                // Count down the pause timer
                pauseTimer -= Time.deltaTime;
                if (pauseTimer <= 0)
                {
                    // End pausing and start rushing
                    isPausing = false;
                    isRushing = true;
                }
            }
            else if (isRushing)
            {
                // Move towards the target at rush speed
                Vector3 direction = (target.position - transform.position).normalized;
                transform.position += direction * Time.deltaTime * rushSpeed;

                // Rotate rapidly
                transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            }
        }

        // Update tentacle movements
        for (int i = 0; i < tentacles.Length; i++)
        {
            MoveTentacle(tentacles[i], tentacleBasePositions[i], movementOffsets[i]);
        }
    }

    void MoveTentacle(LineRenderer tentacle, Vector3 basePosition, float offset)
    {
        tentacle.positionCount = segments + 1;

        // Set the base position relative to the head
        tentacle.SetPosition(0, transform.position + basePosition);

        // Generate random movement for the tentacle segments
        Vector3 previousPosition = tentacle.GetPosition(0);

        for (int j = 1; j <= segments; j++)
        {
            // Create unique random offsets using Perlin noise with time + offset
            float offsetX = Mathf.PerlinNoise(Time.time * baseSpeed + offset + j, j) - 0.5f;
            float offsetY = Mathf.PerlinNoise(j, Time.time * baseSpeed + offset) - 0.5f;

            // Calculate next segment position
            Vector3 nextPosition = previousPosition + new Vector3(offsetX, offsetY, 0) * segmentLength;
            tentacle.SetPosition(j, nextPosition);

            previousPosition = nextPosition;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            hitSound.Play();
            // Calculate knockback direction
            Vector3 knockbackDirection = (transform.position - other.transform.position).normalized;
            transform.position += knockbackDirection * knockbackForce;
            
            // Decrease lives
            lives--;

            // Destroy the bullet
            Destroy(other.gameObject);
            GameObject particles = Resources.Load<GameObject>("OctopusParticles"); // Path to prefab inside the Resources folder
            // Check if lives are depleted
            if (lives <= 0)
            {
                
                if (particles != null)
                {
                    Instantiate(particles, transform.position, Quaternion.identity);
                }
                deathSound.Play();
                Destroy(gameObject); // Destroy the octopus
            }
        }
    }
}