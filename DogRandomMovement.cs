using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.SceneManagement;

public class DogRandomMovement : MonoBehaviour
{
    bool canAdvanceTextIndex = true;
    public PlayerMovement player;
    private SpriteRenderer spriteRenderer;
    public GameObject lostDogScreen;
    Vector3 startPos;
    AudioSource music, runMusic, barking;
    public TextMeshProUGUI countdownText;
    float countdown = 30;
    bool escaped = false;
    public bool walkies = false;  // This will be activated by another script
    public float moveSpeed = 2f;
    public float changeDirectionInterval = 2f;
    public float leadMaxLength = 5f; // Maximum length of the lead
    public LineRenderer lineRenderer; // Reference to the LineRenderer component
    public Transform playerHand; // Reference to the player's hand position
    bool canReturnToMusic = true;
    private Vector2 targetPosition;
    private Rigidbody2D rb;
    private Coroutine directionChangeCoroutine;
    private Vector2 lastDirection; // Store the last direction

    private void Start()
    {
        canAdvanceTextIndex = true;
        barking = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPos = transform.position; 
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        ChangeDirection();

        if (lineRenderer != null)
        {
            lineRenderer.enabled = false; // Ensure the line renderer is disabled initially
        }
        music = GameObject.Find("Music").GetComponent<AudioSource>();
        runMusic = GameObject.Find("RunMusic").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (countdown <= 0)
        {
            if (canAdvanceTextIndex)
            {
                EndWalkies();
               
            }
        }
        if (walkies)
        {
            countdownText.gameObject.SetActive(true);
            countdown -= Time.deltaTime;
            
            countdownText.text = "Walkies finished in " + countdown.ToString("0") + " seconds";
            if (directionChangeCoroutine == null) // Check if the coroutine is not already running
            {
                directionChangeCoroutine = StartCoroutine(ChangeDirectionRoutine()); // Start changing direction
            }
            MoveDog();

            // Update line renderer
            if (lineRenderer != null)
            {
                lineRenderer.enabled = true; // Enable the line renderer
                UpdateLineRenderer();
            }

            // Check lead length and escape logic
            if (Vector2.Distance(transform.position, playerHand.position) > leadMaxLength)
            {
                Escape();
            }
        }
        else
        {
            if (directionChangeCoroutine != null) // Stop changing direction if walkies is false
            {
                StopCoroutine(directionChangeCoroutine);
                directionChangeCoroutine = null; // Reset the coroutine reference
            }

            if (lineRenderer != null)
            {
                lineRenderer.enabled = false; // Disable the line renderer when not walkies
            }
        }
    }
    void EndWalkies()
    {
        barking.Stop();
        countdownText.gameObject.SetActive(false);
        
        transform.position = Vector2.MoveTowards(transform.position, startPos, 3 * Time.deltaTime);
        player.isRunning = false;
        if (canReturnToMusic)
        {
            runMusic.Stop();
            music.Play();
            canReturnToMusic = false;
        }
        walkies = false;
        GameManager.Instance.walkiesComplete = true;
        GameManager.Instance.taskIndex++;
        canAdvanceTextIndex = false;

    }
    private void MoveDog()
    {
        // Move towards the target position
        Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.deltaTime);
        rb.MovePosition(newPosition);

        // Flip the sprite based on the movement direction
        if (newPosition.x > rb.position.x)
        {
            spriteRenderer.flipX = true; // Face right
        }
        else if (newPosition.x < rb.position.x)
        {
            spriteRenderer.flipX = false; // Face left
        }
    }

    private IEnumerator ChangeDirectionRoutine()
    {
        while (walkies) // Keep changing direction as long as walkies is true
        {
            ChangeDirection();
            changeDirectionInterval = Random.Range(1, 5);
            yield return new WaitForSeconds(changeDirectionInterval); // Wait for the specified interval
        }
    }

    private void ChangeDirection()
    {
        // Choose a random target position within a defined range
        float xOffset = Random.Range(-25f, 25f); // Change 5f to your desired range
        float yOffset = Random.Range(-25f, 25f); // Change 5f to your desired range
        targetPosition = new Vector2(transform.position.x + xOffset, transform.position.y + yOffset);

        // Calculate the direction to the new target position
        lastDirection = (targetPosition - rb.position).normalized; // Store the last direction
    }
    public void RetryDogWalk()
    {
        music.Play();
        runMusic.Stop();
        SceneManager.LoadScene("ExteriorHouse");   
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            if (escaped)
            {
                barking.Stop();
                runMusic.Stop();
                lostDogScreen.SetActive(true);
            }
            else if(!escaped) // If the dog hits a border, move back in the opposite direction
            targetPosition = rb.position - lastDirection * (targetPosition - rb.position).magnitude; // Calculate the opposite direction
        }
       
    }

    private void UpdateLineRenderer()
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, transform.position); // Set the start position of the line (dog's neck)
            lineRenderer.SetPosition(1, playerHand.position); // Set the end position of the line (player's hand)
        }
    }

    private void Escape()
    {
     
        escaped = true;
        // The dog escapes and runs off
        lineRenderer.enabled = false; // Disable the line renderer
        walkies = false; // Disable walkies
        rb.velocity = new Vector2(Random.Range(-25f, 25f), Random.Range(-25f, 25f)); // Randomly set a velocity to simulate the dog running off
    }
}