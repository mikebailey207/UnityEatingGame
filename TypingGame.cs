using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;
public class TypingGame : MonoBehaviour
{
    public GameObject player;
    Vector3 originalPosition;
    bool isFlipped = false;
    public ParticleSystem waterParticles;
    public Animator playerBiteAnim;
    bool canDie = true;

    public TextMeshProUGUI fartReminderText;
    public GameObject otherCanvas1, otherCanvas2;
    public AudioSource explodeSound, music, uhohSound, scratchSound;
    public CinemachineVirtualCamera zoomCam;
    public GameObject explodeParticles, eatParticles;
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
    public Slider fullnessSlider, burpSlider, staminaSlider;
    int biteCost = 15; // Cost of each bite
    float fullnessAmount = 0;
    bool drinkingWater = false;
    public AudioSource failSound, correctSound, chompSound, fartSound, drinkSound, thirstySound;
    public GameObject pizzaObject;
    public GameObject[] pizzaBits;
    int bitesTaken =0;
    public TextMeshProUGUI nextLetterText;
    public GameObject[] yummyLetters; // Array for each letter GameObject (Y, U, M, M, Y)
    private string correctSequence = "YUMMY";
    private int currentIndex = 0; // Track the current letter index in the sequence
    public GameObject faxSound;
    private void Start()
    {
        originalPosition = player.transform.position;
        faxSound.SetActive(false);
        noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        staminaSlider.value = 100; // Start with full stamina
        fullnessSlider.value = 0; // Fullness starts at 0
        burpSlider.value = 0; // Bu

        UpdateNoise(0, 0);
    }
    private void Update()
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
        noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        nextLetterText.text = "Type the letter " + correctSequence[currentIndex] + "!";
        if (fullnessAmount > 0) fullnessAmount -= Time.deltaTime * 2;
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
                
                GameManager.Instance.bardEatenCount = 0;
                GameManager.Instance.finalePlayerCounter = 0;
                GameManager.Instance.music.Stop();
                GameManager.Instance.finaleMusic.Stop();
                GameManager.Instance.StopBardEatingCoroutine();
                GameManager.Instance.bardDeaths++;
                SceneManager.LoadScene("OutsidePub");

            }
           
        }
        fullnessSlider.value = fullnessAmount;
        if (Input.GetMouseButtonDown(0) && burpSlider.value >= 50)
        {
            GameManager.Instance.fartSound.Play();
            Burp();
        }
        if (Input.GetMouseButtonDown(1))
        {
            drinkSound.Play();
            waterParticles.Play();
            drinkingWater = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            drinkingWater = false;
            waterParticles.Stop();
            drinkSound.Stop();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            drinkSound.Play();
            drinkingWater = true;
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            drinkingWater = false;
            drinkSound.Stop();
        }
        if (drinkingWater)
        {
            DrinkWater();
           // FlipPlayer();
        }
       /* else if (isFlipped)
        {
            ResetPlayerPosition(); // Reset player position and flip status when not drinking
        }*/
        if (Input.anyKeyDown)
        {
            if (!drinkingWater)
            {

                string keyPressed = Input.inputString.ToUpper();

                if (!string.IsNullOrEmpty(keyPressed))
                {
                    // Check if the pressed key matches the current letter in the sequence
                    if (keyPressed[0] == correctSequence[currentIndex])
                    {
                        correctSound.Play();
                        // Trigger the "Bounce" animation for the current letter
                        Animator letterAnimator = yummyLetters[currentIndex].GetComponent<Animator>();
                        if (letterAnimator != null)
                        {
                            letterAnimator.SetTrigger("Bounce");
                        }

                        currentIndex++;

                        // Check if the entire sequence is complete
                        if (currentIndex >= correctSequence.Length)
                        {
                            TakeBite(); // Call the TakeBite method
                            ResetSequence();
                        }
                    }
                    else
                    {
                        failSound.Play();
                        // Incorrect key pressed, lose a life and reset sequence
                        Debug.Log("LOSE LIFE");
                        fullnessAmount += biteCost - GameManager.Instance.biteCostMultiplier*2;
                       // staminaSlider.value = 0;
                        burpSlider.value = 0;
                        ResetSequence();
                    }
                }
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
        pizzaObject.SetActive(false);
        yield return new WaitForSeconds(2);
        loseScreen.SetActive(true);
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene("TypingGame");
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
    private void ResetSequence()
    {
 
        currentIndex = 0;
    }

    private void Burp()
    {
        burpSlider.value -= 50; // Decrease burp meter
        fullnessAmount -= 10f + GameManager.Instance.burpAddMultiplier +5; // Decrease fullness meter
    }
    private void FlipPlayer()
    {
     //   if (!isFlipped)
      //  {
            // Flip the player and move to the new position
    //        playerSprite.transform.localScale = new Vector3(-playerSprite.transform.localScale.x, playerSprite.transform.localScale.y, playerSprite.transform.localScale.z);
  //     playerSprite.transform.position = new Vector3(originalPosition.x - 2, originalPosition.y, originalPosition.z);
           

            waterParticles.Play();
     //       isFlipped = true;
      //  }
    }
    private void ResetPlayerPosition()
    {
        // Reset the player's position and flip status
      //  playerSprite.transform.localScale = new Vector3(-playerSprite.transform.localScale.x, playerSprite.transform.localScale.y, playerSprite.transform.localScale.z);
      /*  if (!GameManager.Instance.gotPizzaTshirt) playerSprite.transform.position = new Vector3(originalPosition.x, originalPosition.y + 4, originalPosition.z);
 //      else if (GameManager.Instance.gotPizzaTshirt) player.transform.position = new Vector3(originalPosition.x + 2, originalPosition.y, originalPosition.z);*/


        waterParticles.Stop();
     //   isFlipped = false;
    }
    void TakeBite()
    {
        if (staminaSlider.value < biteCost)
        {
            thirstySound.Play();
            
            Debug.Log("Not enough stamina to take a bite!");
            return; 
        }
        else if (staminaSlider.value >= biteCost - GameManager.Instance.biteCostMultiplier * 3 && !drinkingWater)
        {
            if (bitesTaken < pizzaBits.Length)
            {
                staminaSlider.value -= biteCost - GameManager.Instance.biteCostMultiplier * 3;
                chompSound.Play();
                pizzaBits[bitesTaken].SetActive(false);
               
                fullnessAmount = Mathf.Clamp(fullnessSlider.value + biteCost - GameManager.Instance.biteCostMultiplier * 2, 0, 110); // Increase fullness
                burpSlider.value = Mathf.Clamp(burpSlider.value + 25 + GameManager.Instance.burpAddMultiplier, 0, 100); // Increase burp meter
                playerBiteAnim.SetTrigger("Bite");
                GameObject particle = Instantiate(eatParticles, mouthSprite.gameObject.transform.position, Quaternion.identity);
                particle.transform.localScale = new Vector3(2, 2, 2);

                // Only turn on the next slice if there is one left
                if (bitesTaken + 1 < pizzaBits.Length)
                {
                    pizzaBits[bitesTaken + 1].SetActive(true);
                }

                bitesTaken++;

                // Check if it's the fourth bite
                if (bitesTaken >= 4)
                {
                    c.foodsEaten++;
                    // Reset the pizza object immediately after the fourth bite
                    pizzaObject.SetActive(false);
                    bitesTaken = 0; // Reset the counter
                    pizzaBits[bitesTaken].SetActive(true);
                    pizzaObject.SetActive(true);
                    if (GameManager.Instance.isFinale) GameManager.Instance.finalePlayerCounter++;
                }
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
}
