using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;

public class MountMovement : MonoBehaviour
{
    public GameObject waterParticles;
    public TextMeshProUGUI fartReminderText;
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

    public GameObject particles;
    public SpriteRenderer mouthRenderer;
    public Sprite[] mouthSprites;
    public Countdown c;
    public AudioSource drinkSound, fartSound, thirstySound;
    public AudioSource chompSound;
    public Slider staminaSlider; // Stamina slider
    public Slider fullnessSlider; // Fullness slider
    public Slider burpSlider;
    float fullnessAmount = 0;
    private bool drinkingWater = false;

    float biteCost = 10;
    public Transform[] corners; // An array to hold the corner points (in order)
    private int currentCornerIndex = 0;
    private bool clockwise = true; // Movement direction
    public float moveSpeed = 5f;
    public float minInterval = 0.5f; // Minimum time before direction change
    public float maxInterval = 3f;   // Maximum time before direction change

    private void Start()
    {
        GameManager.Instance.holdingNacho = false;
        noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        staminaSlider.value = 100; // Start with full stamina
     fullnessSlider.value = 0; // Fullness starts at 0
        burpSlider.value = 0;
        // Start the coroutine to change direction at random intervals
        StartCoroutine(DirectionChangeRoutine());
        UpdateNoise(0, 0);
    }

    private void Update()
    {
        if (!GameManager.Instance.talking)
        {
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
                            StopCoroutine(DirectionChangeRoutine());
                            Invoke("ExplodeVoid", 2.3f);
                     
                            canDie = false;
                        }
                    }
                    else if (GameManager.Instance.isFinale)
                    {
                        GameManager.Instance.finaleMusic.Stop();
                        GameManager.Instance.StopBardEatingCoroutine();
                        GameManager.Instance.bardEatenCount = 0;
                        GameManager.Instance.finalePlayerCounter = 0;
                        GameManager.Instance.music.Stop();
                        GameManager.Instance.bardDeaths++;
                        SceneManager.LoadScene("OutsidePub");
                    }
                }
                fullnessSlider.value = fullnessAmount;
            }
            //      moveSpeed += Time.deltaTime/4;
            // Continuously move towards the current target corner
            Vector3 target = corners[currentCornerIndex].position;
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            // Check if the mouth has reached the current corner
            if (Vector3.Distance(transform.position, target) < 0.1f)
            {
                // Move to the next corner based on direction
                if (clockwise)
                {
                    currentCornerIndex = (currentCornerIndex + 1) % corners.Length;
                }
                else
                {
                    currentCornerIndex = (currentCornerIndex - 1 + corners.Length) % corners.Length;
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                waterParticles.SetActive(true);
                MouthAnimation();
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
       
        disguise.transform.parent = null;
        explodeSound.Play();
        Instantiate(explodeParticles, particleSpawnPoint.position, Quaternion.identity);
        playerSprite.GetComponent<SpriteRenderer>().enabled = false;
        mouthSprite.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1);
        disguise.GetComponent<Rigidbody2D>().isKinematic = false;
        disguise.GetComponent<Rigidbody2D>().gravityScale = 10;
        ChooseFartSentence();
        yield return new WaitForSeconds(2);
        loseScreen.SetActive(true);
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene("NachosChallenge");
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
    void MouthAnimation()
    {
        mouthRenderer.sprite = mouthSprites[1];
        
        Invoke("StopMouthAnim", 0.5f);
    }
    void StopMouthAnim()
    {
        mouthRenderer.sprite = mouthSprites[0];
     
    }
    private IEnumerator DirectionChangeRoutine()
    {
        while (true)
        {
            // Wait for a random time between minInterval and maxInterval
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            // Change the movement direction
            ChangeDirection();
        }
    }
    private void FlipMouth()
    {
        // Check direction and flip accordingly
        if (clockwise)
        {
            // Flip mouth for clockwise direction (right)
            transform.localScale = new Vector3(0.16031f, 0.16031f, 0.16031f);  // Reset to normal
        }
        else
        {
            // Flip mouth for counterclockwise direction (left)
            transform.localScale = new Vector3(-0.16031f, 0.16031f, 0.16031f);  // Flip horizontally
        }
    }
    private void ChangeDirection()
    {
       
        // Toggle movement direction
        clockwise = !clockwise;
        FlipMouth();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Nacho"))
        {
            if (staminaSlider.value >= biteCost - GameManager.Instance.biteCostMultiplier && !drinkingWater)
            {
                
                GameManager.Instance.holdingNacho = false;
                Destroy(collision.gameObject);
              
                chompSound.Play();
                staminaSlider.value -= biteCost - GameManager.Instance.biteCostMultiplier;
                fullnessAmount = Mathf.Clamp(fullnessSlider.value + biteCost - GameManager.Instance.biteCostMultiplier, 0, 110); // Increase fullness
                burpSlider.value = Mathf.Clamp(burpSlider.value + 25 + GameManager.Instance.burpAddMultiplier, 0, 100);
                MouthAnimation();
                Instantiate(particles, transform.position, transform.rotation);
                c.foodsEaten++;

                if (GameManager.Instance.isFinale) GameManager.Instance.finalePlayerCounter++;
            }
        else
         {
            thirstySound.Play();
                
          }
        }
    }
    private void Burp()
    {
        burpSlider.value -= 50; // Decrease burp meter
        fullnessAmount -= 10f + GameManager.Instance.burpAddMultiplier; // Decrease fullness meter
    }
    public void DrinkWater()
    {
        if (staminaSlider.value < 100)
        {
            staminaSlider.value += Time.deltaTime * 50 * GameManager.Instance.drinkRefreshMultiplier; // Adjust the rate as needed
        }
    }
}


