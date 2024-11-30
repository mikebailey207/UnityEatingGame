using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;


public class PlayerMovement : MonoBehaviour
{
    private Vector2 lastNonZeroMovement = Vector2.right;
    public GameObject firstSaffronText;
    public CinemachineVirtualCamera firstDayCam; 
    private Animator animator;
    bool canStartFirstWalkies = true;
    bool metDog;
    public GameObject dogInstructions;
    private bool isDashing = false; // Tracks if the player is currently dashing
    public float dashCooldown = .5f; // Cooldown time in seconds
    public float dashDuration = 0.3f; // Duration of the dash
    public float dashSpeed = 20;
    private float lastDashTime = -1f; // Tracks the last time the player dashed
    bool dogInstructionsOn = false;

    public AudioClip[] walkiesClips; // Array to store the 5 bark clips
    public AudioSource walkiesSource; // AudioSource for barking
    private bool isWalkies = false;
    public AudioSource barking;
    bool canChangeMusic = true;
    private float stopDistance = 0.3f;
    GameObject dog;
    public bool walkingDog = false;
    AudioSource music, runMusic;
    bool canSwitchMusic = true;
    DialogueManager dialogue;
   // public CinemachineConfiner2D confiner;
  //  public CinemachineVirtualCamera cam1, cam2;
    bool startedGame = false;
     float moveSpeed = 2f; // Speed of the player movement
    public float runSpeedMultiplier = 2f; // Multiplier for running speed
    public float maxSpeed = 10f; // Maximum speed while running
    public Slider speedMeter; // Reference to the speed meter slider

    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private Vector2 movement; // Store the movement direction
    private bool facingRight = true; // Track which direction the player is facing
    public bool isRunning = false; // Track if the player is in running mode
    private float currentSpeed = 5f; // Current running speed
    Quaternion startRot;

    bool canTurnOffFirstDay = true;

    void Start()
    {
       
        if (SceneManager.GetActiveScene().name == "TownEntrace" && GameManager.Instance.day == 2)
        {
            FirstDaySaffron();
        }
      
        startRot = transform.rotation;
        dialogue = FindObjectOfType<DialogueManager>();
        music = GameObject.Find("Music").GetComponent<AudioSource>();
        runMusic = GameObject.Find("RunMusic").GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
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
        if (GameManager.Instance.day >= 3)
        {
            Invoke("TurnOffOtherDays", 1);
          //  if (firstDayCam != null) firstDayCam.gameObject.SetActive(false);
        }
    }

    void Update()
    {
     //   if (Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.gotTrainers && !isDashing)// && Time.time >= lastDashTime + dashCooldown)
     //   {
       //     StartCoroutine(Dash());
     //   }
    
           // if(GameManager.Instance.day == 2 && canTurnOffFirstDay)
           // {
           // TurnOffFirstDay();

           // }
       
       
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = 4;
            animator.speed = 2f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 2f;
            animator.speed = 1;
        }
        if(Input.GetKeyDown(KeyCode.Space) && canStartFirstWalkies)
        {
            WalkTheDog();
            canStartFirstWalkies = false;
        }
       
        if (startedGame)
        {

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

    void FirstDaySaffron()
    {
        if (firstDayCam != null)
        {
            StartCoroutine(FirsrtSaffronEnum());
        }

    }
    IEnumerator FirsrtSaffronEnum()
    {
        firstSaffronText.SetActive(true);
        GameManager.Instance.talking = true;
        yield return new WaitForSeconds(2);
        GameManager.Instance.talking = false;
        yield return new WaitForSeconds(9);



        TurnOffFirstDay();
    }
    void TurnOffFirstDay()
    {
        firstDayCam.Priority = 0;
        firstSaffronText.SetActive(false);
        GameManager.Instance.talking = false;
        canTurnOffFirstDay = false;
    }
    void TurnOffOtherDays()
    {
        firstDayCam.Priority = 0;

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
        if (!GameManager.Instance.talking)
        {
            // Get input for movement
            movement.x = Input.GetAxis("Horizontal"); // A/D or Left/Right arrows
            movement.y = Input.GetAxis("Vertical");   // W/S or Up/Down arrows

            // Update animation based on movement
            

            // Call the function to flip the player sprite based on movement direction
            if (movement.x < 0 && facingRight) // If moving left and currently facing right
            {
                Flip(); // Flip the player
            }
            else if (movement.x > 0 && !facingRight) // If moving right and currently facing left
            {
                Flip(); // Flip the player
            }
            if (movement != Vector2.zero) // Check if there is movement
            {
                animator.SetBool("IsRunning", true); // Set the running animation
            }
            else
            {
                animator.SetBool("IsRunning", false); // Set the idle animation
            }
            if (isWalkies)
            {
                // Rotate the player towards the dog
            }
            else
            {
                transform.rotation = startRot;
            }
        }
        else return;
    }


    private IEnumerator Dash()
    {
        if (!isDashing && Time.time >= lastDashTime + dashCooldown)
        {
            lastDashTime = Time.time;
            isDashing = true;

            Vector2 dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            if (dashDirection == Vector2.zero)
            {
                dashDirection = lastNonZeroMovement;
            }
            else
            {
                lastNonZeroMovement = dashDirection;
            }

            float originalSpeed = moveSpeed;
            moveSpeed = 15;

            float elapsedTime = 0;
            while (elapsedTime < dashDuration)
            {
                rb.velocity = dashDirection * moveSpeed;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            rb.velocity = Vector2.zero;
            moveSpeed = originalSpeed;
            isDashing = false;
        }
    }
    void RotatePlayerTowardsDog()
    {
        Vector3 directionToDog = dog.transform.position - transform.position;
        directionToDog.z = 0;

        float angle = Mathf.Atan2(directionToDog.y, directionToDog.x) * Mathf.Rad2Deg;

        // Smooth rotation
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 500);
        
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
        if (collision.gameObject.CompareTag("Dog") && GameManager.Instance.reputation < 350)
        {
            dog = collision.gameObject;
            
            Rigidbody2D dogRigidbody = dog.GetComponent<Rigidbody2D>();
            Rigidbody2D playerRigidbody = GetComponent<Rigidbody2D>(); // Assumes this script is on the player object

            // Freeze the dog and player completely
           

            if (GameManager.Instance.day == 2 && !metDog && !dogInstructionsOn)
            {
                if (dogRigidbody != null)
                {
                    dogRigidbody.velocity = Vector2.zero;
                    dogRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                }
                if (playerRigidbody != null)
                {
                    playerRigidbody.velocity = Vector2.zero;
                    playerRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                }
                // Show instructions only if they haven't been shown already
                GameManager.Instance.talking = true;
                dogInstructions.SetActive(true);
                dogInstructionsOn = true;
                metDog = true;
                GetComponent<Animator>().SetBool("IsRunning", false);

                // Ensure this function doesn't proceed to WalkTheDog
                return;
            }
            else if (!GameManager.Instance.talking) // Only proceed if not in a talking state
            {
                WalkTheDog();
            }
        }
    }
    void WalkTheDog()
    {
        if (!GameManager.Instance.walkiesComplete)
        {
        //    dog.GetComponent<DogRandomMovement>().hereBoy = false;
            Rigidbody2D dogRigidbody = dog.GetComponent<Rigidbody2D>();
            Rigidbody2D playerRigidbody = GetComponent<Rigidbody2D>();
           
            if (dogRigidbody != null)
            {
                dogRigidbody.constraints = RigidbodyConstraints2D.None;
                dogRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }

            if (playerRigidbody != null)
            {
                playerRigidbody.constraints = RigidbodyConstraints2D.None;
                playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            if (canChangeMusic)
            {
                //Time.timeScale = 1;
                dogInstructions.SetActive(false);
               
                GameManager.Instance.talking = false;
                barking.Play();
                isRunning = true;
                music.Stop();
                runMusic.Play();
                Debug.Log("WalkieS?");
                
                dog.GetComponent<DogRandomMovement>().walkies = true;
                walkingDog = true;
                canChangeMusic = false;

                if (!isWalkies)
                {
                    StartCoroutine(PlayBarkSounds());
                }
            }
        }
    }
    private IEnumerator PlayBarkSounds()
    {
        isWalkies = true;

        while (walkingDog) // Continue as long as the dog is walking
        {
            // Play a random bark clip
            walkiesSource.clip = walkiesClips[Random.Range(0, walkiesClips.Length)];
            walkiesSource.Play();

            // Wait for a random interval between 2 and 4 seconds
            yield return new WaitForSeconds(Random.Range(2f, 4f));
        }

        isWalkies = false;
    }
}
