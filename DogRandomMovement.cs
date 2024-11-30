using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.SceneManagement;
using Cinemachine;
public class DogRandomMovement : MonoBehaviour
{
    public AudioSource squeakSound;

    public GameObject taskPopup;
    public TextMeshProUGUI taskTextText;

    public GameObject blueCircle;
    bool canDie = true;
    public CinemachineVirtualCamera walkiesCam;
    bool canAdvanceTextIndex = true;
    public PlayerMovement player;
    private SpriteRenderer spriteRenderer;
    public GameObject lostDogScreen, killedByBadgerScreen, killedByBulletScreen, killedByOctopusScreen, killedBySniperScreen;
    Vector3 startPos;
    AudioSource music, runMusic, barking;
    public TextMeshProUGUI countdownText;
    float countdown;
    bool escaped = false;
    public bool walkies = false;  // This will be activated by another script
    public float moveSpeed = 1f;
    public float changeDirectionInterval = 2f;
    public float leadMaxLength = 10f; // Maximum length of the lead
    public LineRenderer lineRenderer; // Reference to the LineRenderer component
    public Transform playerHand; // Reference to the player's hand position
    bool canReturnToMusic = true;
    private Vector2 targetPosition;
    private Rigidbody2D rb;
    private Coroutine directionChangeCoroutine;
    private Vector2 lastDirection; // Store the last direction
    float speed = 15;
    float startMoney;
   public bool hereBoy = false;

    private void Start()
    {
        countdown = GameManager.Instance.walkiesCountdownStart;
        taskPopup.SetActive(false);
       if(GameManager.Instance.gotTrainers) ApplyStartPosition();
        if (GameManager.Instance.reputation > 599.5f)
        {
            Destroy(blueCircle.gameObject);
            Destroy(gameObject);
        }
        startMoney = GameManager.Instance.money;
        GameManager.Instance.moneyCollectedThisSession = 0;
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
        if(Input.GetMouseButtonDown(1) && !GameManager.Instance.walkiesComplete && GameManager.Instance.gotTrainers)
        {
            squeakSound.Play();
            hereBoy = true;
           
        }
        if(hereBoy)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.gameObject.transform.position, 10 * Time.deltaTime);
        }
        if (countdown <= 0)
        {
            if (canAdvanceTextIndex)
            {
                GameManager.Instance.walkingDog = false;
                EndWalkies();

            }
        }
        if (walkies)
        {
            GameManager.Instance.walkingDog = true;
            if (Vector2.Distance(transform.position, playerHand.position) > GameManager.Instance.leadMaxLength)
            {
                Escape();
            }
            blueCircle.SetActive(true);
            walkiesCam.Priority = 100;
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
            // Check if the dog is to the left or right of the player
            if (transform.position.x > player.transform.position.x)
            {
                // Dog is to the right of the player, flip it to face left
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                // Dog is to the left of the player, flip it to face right
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            // Check lead length and escape logic

        }
        else
        {
           blueCircle.SetActive(false);
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
    void ApplyStartPosition()
    {
        // Define maximum attempts to prevent infinite loops
        const int maxAttempts = 100;
        int attempt = 0;

        // Define the spawn area bounds
        float minX = -30, maxX = -2;
        float minY = -10, maxY = 17;

        // Define the check radius
        float checkRadius = 2f; // Adjust based on your dog's collider size

        bool positionValid = false;

        while (!positionValid && attempt < maxAttempts)
        {
            // Generate a random position
            float posX = Random.Range(minX, maxX);
            float posY = Random.Range(minY, maxY);
            Vector3 potentialPosition = new Vector3(posX, posY, 0);

            // Check for any collision
            Collider2D hit = Physics2D.OverlapCircle(potentialPosition, checkRadius);
            if (hit == null)
            {
                // No collision detected
                positionValid = true;
                transform.position = potentialPosition;
            }

            attempt++;
            Debug.Log(attempt);
        }

        if (!positionValid)
        {
            Debug.LogWarning("Failed to find a valid spawn position for the dog.");
        }
    }
    void EndWalkies()
    {
       
        walkies = false;
        canDie = false;
        walkiesCam.Priority = 0;
        Debug.Log("EndWalkies");
        barking.Stop();
        countdownText.gameObject.SetActive(false);

        transform.position = Vector2.MoveTowards(transform.position, startPos, 3 * Time.deltaTime);
        player.isRunning = false;
        player.walkingDog = false;
        if (canReturnToMusic)
        {
            runMusic.Stop();
            music.Play();
            canReturnToMusic = false;
        }
        
        GameManager.Instance.walkiesComplete = true;
        GameManager.Instance.taskIndex++;
        canAdvanceTextIndex = false;
        taskTextText.text = GameManager.Instance.taskTexts[GameManager.Instance.taskIndex] + "!";
        taskPopup.SetActive(true);
        GameManager.Instance.walkiesCountdownStart += 2;

    }
    private void MoveDog()
    {
        // Move towards the target position

        transform.position = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.deltaTime);
        //  Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.deltaTime);
        //   rb.MovePosition(newPosition);

        // Flip the sprite based on the movement direction
        //   if (newPosition.x > rb.position.x)
        if (transform.position.x > rb.position.x)
        {
            spriteRenderer.flipX = false; // Face right
        }
        // else if (newPosition.x < rb.position.x)
        {
            spriteRenderer.flipX = true;     // Face left
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
        float xOffset = Random.Range(-speed, speed); // Change 5f to your desired range
        float yOffset = Random.Range(-speed, speed); // Change 5f to your desired range
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
      /*  if (collision.gameObject.CompareTag("Bullet"))
        {
            if (canDie)
            {
                barking.Stop();
                runMusic.Stop();
                GameManager.Instance.money = startMoney;
                killedByBulletScreen.SetActive(true);
                canDie = false;
            }
        }*/
        if (collision.gameObject.CompareTag("SniperBullet"))
        {
            if (canDie)
            {
                barking.Stop();
                runMusic.Stop();
                GameManager.Instance.money = startMoney;
                killedBySniperScreen.SetActive(true);
                canDie = false;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            if (escaped)
            {
                if (canDie)
                {
                    barking.Stop();
                    runMusic.Stop();
                    GameManager.Instance.money = startMoney;

                    lostDogScreen.SetActive(true);
                    canDie = false;
                }
            }
            else if (!escaped) // If the dog hits a border, move back in the opposite direction
                               //  ChangeDirection();
                targetPosition = rb.position - lastDirection * (targetPosition - rb.position).magnitude; // Calculate the opposite direction
        }
        if (collision.gameObject.CompareTag("Badger"))
        {
            if (canDie && walkies)
            {
                barking.Stop();
                runMusic.Stop();
                GameManager.Instance.money = startMoney;
                killedByBadgerScreen.SetActive(true);
                canDie = false;
            }
        }
        if (collision.gameObject.CompareTag("Octopus"))
        {
            if (canDie && walkies)
            {
                barking.Stop();
                runMusic.Stop();
                GameManager.Instance.money = startMoney;
                killedByOctopusScreen.SetActive(true);
                canDie = false;
            }
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