using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;
public class CornMouthController : MonoBehaviour
{
    public GameObject bard;
    public GameObject waterParticles;
    public TextMeshProUGUI fartReminderText;
    bool bouncing = true;
    bool canDie = true;

    public GameObject otherCanvas1, otherCanvas2;
    public AudioSource explodeSound, music, uhohSound, scratchSound;
    public CinemachineVirtualCamera zoomCam;
    public GameObject explodeParticles;
    public GameObject playerSprite, mouthSprite;
    public GameObject disguise;
    public Transform particleSpawnPoint;
    public GameObject loseScreen;

    public GameObject warningScreen;
    public AudioSource gagSound;
    bool canPlayGagSound = true;
    public CinemachineVirtualCamera cam;
    CinemachineBasicMultiChannelPerlin noise;

    public Countdown c;
    public AudioSource drinkSound, fartSound;
    public Slider staminaSlider; // Stamina slider
    public Slider fullnessSlider;
    public Slider burpSlider;
    private int bitesTaken = 0;
    private int biteCost = 10;
    float fullnessAmount = 0;
    private bool drinkingWater = false;

    private AudioSource thirstySound;

    public AudioSource boingSound, chompSound;
    public GameObject cornParticles;
   
    GameObject cornParent = null;
    public Rigidbody2D cornRB = null;
    public float moveForce = 500f; // Adjust force as needed
    public Rigidbody2D rb;

    public GameObject cornPrefab; // Reference to the corn prefab
    private int cornPiecesRemaining = 46; // Initial number of corn pieces per corn object

    void Start()
    {
        thirstySound = GameObject.Find("ThirstySound").GetComponent<AudioSource>();
        noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        rb = GetComponent<Rigidbody2D>();
        staminaSlider.value = 100; // Start with full stamina
        fullnessSlider.value = 0; // Fullness starts at 0
        burpSlider.value = 0;
        UpdateNoise(0, 0);
        rb.gravityScale = 1;
        thirstySound.enabled = true;
        if (GameManager.Instance.isFinale) bard.SetActive(false);
        if (GameManager.Instance.talking) rb.gravityScale = 0;
    }

    void Update()
    {
        if (!GameManager.Instance.talking)
        {
            rb.gravityScale = 1;
            if (fullnessAmount >= 80)
            {
                UpdateNoise(1, 1);
                warningScreen.SetActive(true);
                PlayGagSound();
            }
            else
            {
                UpdateNoise(0, 0);
                warningScreen.SetActive(false);
                canPlayGagSound = true;
            }
            if (c.countdown >= 0)
            {
                if (fullnessAmount > 0) fullnessAmount -= Time.deltaTime * 2 * GameManager.Instance.fullNessReduceMultiplier;
                if (fullnessAmount >= 100)
                {
                    c.foodsEaten = 0;
                    if (!GameManager.Instance.isFinale)
                    {
                        zoomCam.Priority = 20;
                        music.Stop();
                        c.countingDown = false;
                        otherCanvas1.SetActive(false);
                        otherCanvas2.SetActive(false);
                        if (canDie)
                        {
                            scratchSound.Play();
                            uhohSound.Play();
                            Invoke("ExplodeVoid", 2.3f);

                            canDie = false;
                        }
                    }
                    else if(GameManager.Instance.isFinale)
                    {
                        /// ADD A RETRY FINALE SCREEN HERE
                        GameManager.Instance.finaleMusic.Stop();
                        GameManager.Instance.StopBardEatingCoroutine();
                        GameManager.Instance.StopBardEatingCoroutine();
                        GameManager.Instance.bardEatenCount = 0;
                        GameManager.Instance.finalePlayerCounter = 0;
                        GameManager.Instance.bardDeaths++;
                        SceneManager.LoadScene("OutsidePub");
                    }
                }
                fullnessSlider.value = fullnessAmount;
                // Check for mouse click
                if (Input.GetMouseButtonDown(0))
                {
                    staminaSlider.value -= biteCost - GameManager.Instance.biteCostMultiplier;
                    rb.velocity = Vector2.zero;
                    PlayBoingSound();
                    MoveTowardsMouse();
                }
                if (Input.GetMouseButtonDown(1))
                {
                    waterParticles.SetActive(true);
                    drinkSound.Play();
                    drinkingWater = true;
                }
                if (Input.GetMouseButtonUp(1))
                {
                    waterParticles.SetActive(false);
                    drinkingWater = false;
                    drinkSound.Stop();
                }

                if (drinkingWater)
                {
                    DrinkWater();
                }
                if (Input.GetKeyDown(KeyCode.Space) && burpSlider.value >= 50)
                {
                    GameManager.Instance.fartSound.Play();
                    Burp();
                }

            }
            if(bouncing) BounceOnScreenEdges();
            // Flip the object based on movement direction
            FlipObject();
        }
    }
    void ChooseFartSentence()
    {
        int i = Random.Range(0, GameManager.Instance.fartReminderSentences.Length - 1);
        fartReminderText.text = GameManager.Instance.fartReminderSentences[i];
    }
    void ExplodeVoid()
    {
        StartCoroutine(Explode());
    }
    public IEnumerator Explode()
    {
        bouncing = false;
        disguise.transform.parent = null;
        explodeSound.Play();
        Instantiate(explodeParticles, particleSpawnPoint.position, Quaternion.identity);
        playerSprite.GetComponent<SpriteRenderer>().enabled = false;
        mouthSprite.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1);
        disguise.GetComponent<Rigidbody2D>().isKinematic = false;
        ChooseFartSentence();
        disguise.GetComponent<Rigidbody2D>().gravityScale = 10;
        yield return new WaitForSeconds(2);
        loseScreen.SetActive(true);
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene("CornChallenge");
    }
    public void GiveUp()
    {
        SceneManager.LoadScene("TownEntrace");
    }
    void UpdateNoise(float amplitude, float frequency)
    {
        if (noise != null)
        {
            noise.m_AmplitudeGain = amplitude;
            noise.m_FrequencyGain = frequency;
        }
    }
    void PlayGagSound()
    {
        if (canPlayGagSound)
        {
            gagSound.Play();
            canPlayGagSound = false;
        }
    }
    void BounceOnScreenEdges()
    {
        // Define screen boundaries
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        float objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        float objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;

        // Get the current position of the object
        Vector3 pos = transform.position;

        // Check horizontal boundaries
        if (pos.x + objectWidth > screenBounds.x)
        {
            pos.x = screenBounds.x - objectWidth; // Clamp position within bounds
            rb.velocity = new Vector2(-Mathf.Abs(rb.velocity.x), rb.velocity.y); // Reverse X velocity if moving right
            PlayBoingSound();
        }
        else if (pos.x - objectWidth < -screenBounds.x)
        {
            pos.x = -screenBounds.x + objectWidth; // Clamp position within bounds
            rb.velocity = new Vector2(Mathf.Abs(rb.velocity.x), rb.velocity.y); // Reverse X velocity if moving left
            PlayBoingSound();
        }

        // Check vertical boundaries
        if (pos.y + objectHeight > screenBounds.y)
        {
            pos.y = screenBounds.y - objectHeight; // Clamp position within bounds
            rb.velocity = new Vector2(rb.velocity.x, -Mathf.Abs(rb.velocity.y)); // Reverse Y velocity if moving up
            PlayBoingSound();
        }
        else if (pos.y - objectHeight < -screenBounds.y)
        {
            pos.y = -screenBounds.y + objectHeight; // Clamp position within bounds
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Abs(rb.velocity.y)); // Reverse Y velocity if moving down
            PlayBoingSound();
        }

        // Apply the adjusted position
        transform.position = pos;
    }
    void MoveTowardsMouse()
    {
        // Convert mouse position to world coordinates
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate direction from mouth to mouse
        Vector2 direction = (mousePosition - rb.position).normalized;

        // Apply force towards mouse position
        rb.AddForce(direction * moveForce);
    }
    void PlayBoingSound()
    {
        boingSound.pitch = Random.Range(1.5f, 2.1f);
        boingSound.Play();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Corn"))
        {
            if (staminaSlider.value >= biteCost && !drinkingWater && c.countingDown && !GameManager.Instance.talking)
            {
                cornParent = collision.gameObject.transform.parent.gameObject;
                Destroy(collision.gameObject);
                cornPiecesRemaining--;
                fullnessAmount = Mathf.Clamp(fullnessSlider.value + 2f - GameManager.Instance.biteCost/30, 0, 110); // Increase fullness
                burpSlider.value = Mathf.Clamp(burpSlider.value + 2.5f + GameManager.Instance.burpAddMultiplier/30, 0, 100); // Increase burp meter
                Instantiate(cornParticles, transform.position, cornParent.transform.rotation);
                chompSound.Play();

                // Check if all pieces are eaten
                if (cornPiecesRemaining <= 0)
                {
                    cornRB = cornParent.AddComponent<Rigidbody2D>();

                    cornRB.gravityScale = 1;

                    cornParent.GetComponent<CornController>().enabled = false;


                    cornParent = null;
                    InstantiateNewCorn();
                }
            }
            else if (staminaSlider.value >= biteCost && !drinkingWater && !c.countingDown && !GameManager.Instance.talking 
                && GameManager.Instance.isFinale)
            {
                cornParent = collision.gameObject.transform.parent.gameObject;
                Destroy(collision.gameObject);
                cornPiecesRemaining--;
                fullnessAmount = Mathf.Clamp(fullnessSlider.value + 2f - GameManager.Instance.biteCost / 30, 0, 110); // Increase fullness
                burpSlider.value = Mathf.Clamp(burpSlider.value + 2.5f + GameManager.Instance.burpAddMultiplier / 30, 0, 100); // Increase burp meter
                Instantiate(cornParticles, transform.position, cornParent.transform.rotation);
                chompSound.Play();

                // Check if all pieces are eaten
                if (cornPiecesRemaining <= 0)
                {
                    cornRB = cornParent.AddComponent<Rigidbody2D>();

                    cornRB.gravityScale = 1;

                    cornParent.GetComponent<CornController>().enabled = false;


                    cornParent = null;
                    InstantiateNewCorn();
                }
            }
            else
            {
                thirstySound.Play();
            }
        }
    }
    public void DrinkWater()
    {
        if (staminaSlider.value < 100)
        {
            staminaSlider.value += Time.deltaTime * 50 * GameManager.Instance.drinkRefreshMultiplier; // Adjust the rate as needed
        }
    }
    void InstantiateNewCorn()
    {
        c.foodsEaten++;
        Instantiate(cornPrefab, Vector3.zero, Quaternion.identity);
        cornParent = cornPrefab;
        cornPiecesRemaining = 46; // Reset the counter for the new corn object
        if (GameManager.Instance.isFinale) GameManager.Instance.finalePlayerCounter++;
    }
    private void Burp()
    {
        burpSlider.value -= 50; // Decrease burp meter
        fullnessAmount -= 10f + GameManager.Instance.burpAddMultiplier; // Decrease fullness meter

    }

    void FlipObject()
    {
        // Get the horizontal velocity of the mouth
        float horizontalVelocity = rb.velocity.x;

        // Flip the corn parent based on direction of movement
        if (horizontalVelocity < 0 && cornParent != null) // Moving left
        { 
           transform.localScale = new Vector3(-0.08087645f, 0.08087645f, 0.08087645f); // Flip to left
        }
        else if (horizontalVelocity > 0 && cornParent != null) // Moving right
        {
            transform.localScale = new Vector3(0.08087645f, 0.08087645f, 0.08087645f); // Flip to right
        }
    }
}