using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using Cinemachine;
public class PlayerMovement : MonoBehaviour
{

    public AudioSource barking;
    bool canChangeMusic = true;
    private float stopDistance = 0.3f;
    GameObject dog;
    bool walkingDog = false;
    AudioSource music, runMusic;
    bool canSwitchMusic = true;
    DialogueManager dialogue;
   // public CinemachineConfiner2D confiner;
  //  public CinemachineVirtualCamera cam1, cam2;
    bool startedGame = false;
     float moveSpeed = 1.5f; // Speed of the player movement
    public float runSpeedMultiplier = 2f; // Multiplier for running speed
    public float maxSpeed = 10f; // Maximum speed while running
    public Slider speedMeter; // Reference to the speed meter slider

    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private Vector2 movement; // Store the movement direction
    private bool facingRight = true; // Track which direction the player is facing
    public bool isRunning = false; // Track if the player is in running mode
    private float currentSpeed = 5f; // Current running speed

    void Start()
    {
     
        dialogue = FindObjectOfType<DialogueManager>();
        music = GameObject.Find("Music").GetComponent<AudioSource>();
        runMusic = GameObject.Find("RunMusic").GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        if (speedMeter != null)
        {
            speedMeter.maxValue = maxSpeed; // Set the maximum value for the speed meter
            speedMeter.value = 0; // Initialize the speed meter
        }
        if (SceneManager.GetActiveScene().name == "ExteriorStart")
        {
           
            Invoke("WaitToStartGame", 5);
        }
        else
        {
            WaitToStartGame();
        }
 //       cam2.Priority = 20;

    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = 3;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 1.5f;
        }
        if (startedGame)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("FoodChallenge");
            }
                // Check for running mode toggle
                if (Input.GetKeyDown(KeyCode.Space))
            {

                isRunning = !isRunning; // Toggle running mode
                if(isRunning)
                {
                    music.Stop();
                    runMusic.Play();
                }
                else
                {
                    runMusic.Stop();
                    music.Play();
                }
            }

            if (isRunning)
            {
                HandleMovement();
               // HandleRunning();
               // FlipBasedOnMouse();

            }
            else
            {
                HandleMovement();
            }

            // Flip the player based on mouse position


            // Update speed meter
            if (speedMeter != null)
            {
                speedMeter.value = currentSpeed; // Update the slider value
            }
        }
    }

    void FixedUpdate()
    {
        if (startedGame)
        {
            // Move the player towards the mouse position
            if (isRunning)
            {
                rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
                //MoveTowardsMouse();
            }
            else
            {
                rb.MovePosition(rb.position + movement * moveSpeed *  Time.fixedDeltaTime);
            }
        }
    }
    void WaitToStartGame()
    {
     //   confiner.enabled = true;
        startedGame = true;
        transform.parent = null;
        GetComponent<SpriteRenderer>().enabled = true;
    //    dialogue.StartDialogue();
       
    }
    private void HandleMovement()
    {
        // Get input for movement
        movement.x = Input.GetAxis("Horizontal"); // A/D or Left/Right arrows
        movement.y = Input.GetAxis("Vertical");   // W/S or Up/Down arrows

        // Call the function to flip the player sprite based on movement direction
        if (movement.x < 0 && facingRight) // If moving left and currently facing right
        {
            Flip(); // Flip the player
        }
        else if (movement.x > 0 && !facingRight) // If moving right and currently facing left
        {
            Flip(); // Flip the player
        }
    }

    private void HandleRunning()
    {
       // currentSpeed = 10;
        // Handle running mechanics
        if (Input.GetMouseButtonDown(0) && currentSpeed <= 5)
        {
            // Alternating presses increase the current speed
            currentSpeed += 0.5f;
        }
        else
        {
            // Decrease speed when not pressing A or D
            currentSpeed = Mathf.Max(currentSpeed - Time.deltaTime * 0.5f, 0); // Decrease over time
        }
    }

    private void MoveTowardsMouse()
    {
        // Move the player towards the mouse position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Set Z position to 0

        Vector2 direction = (mousePos - transform.position).normalized;
        float distanceToMouse = Vector2.Distance(transform.position, mousePos); // Calculate the distance to the mouse

        // Check if the player is farther away than the stop distance
        if (distanceToMouse > stopDistance)
        {
            rb.MovePosition(rb.position + direction * currentSpeed * Time.fixedDeltaTime);
        }
    }

    private void FlipBasedOnMouse()
    {
        // Get mouse position and check if it's left or right of the player
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Set Z position to 0

        if (mousePos.x < transform.position.x && facingRight)
        {
            Flip(); // Flip if the mouse is to the left
        }
        else if (mousePos.x > transform.position.x && !facingRight)
        {
            Flip(); // Flip if the mouse is to the right
        }
    }

    private void Flip()
    {
        // Flip the player's local scale on the x-axis
        facingRight = !facingRight; // Toggle the facing direction
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Flip the scale
        transform.localScale = localScale; // Apply the new scale
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Dog"))
        {
           if (!GameManager.Instance.walkiesComplete)
            {
                if (canChangeMusic)
                {
                    barking.Play();
                    isRunning = true;
                    music.Stop();
                    runMusic.Play();
                    Debug.Log("WalkieS?");
                    dog = collision.gameObject;
                    dog.GetComponent<DogRandomMovement>().walkies = true;
                    walkingDog = true;
                    canChangeMusic = false;
                }
            }
          
        }
    }
}
