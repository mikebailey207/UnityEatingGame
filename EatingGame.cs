using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EatingGame : MonoBehaviour
{
    public Countdown c;

    public AudioClip[] fartSounds;
    public AudioSource drinkSound, fartSound, thirstySound;

    private float slider1IncreaseSpeed = 100f; // Speed for slider1 when increasing
    private float slider1DecreaseSpeed = 50f;  // Speed for slider1 when decreasing
    private float slider2IncreaseSpeed = 100f; // Speed for slider2 when increasing
    private float slider2DecreaseSpeed = 50f;  // Speed for slider2 when decreasing

    public AudioSource chompSound;
    public Slider slider1; // First slider for hot dog
    public Slider slider2; // Second slider for hot dog
    public Slider staminaSlider; // Stamina slider
    public Slider fullnessSlider; // Fullness slider
    public Slider burpSlider; // Burp meter slider
    public Image hotdogImage, sliderHotdogImage, sliderMouthImage; // Image for the hot dog states

    public SpriteRenderer mouthRenderer;
    public Sprite[] hotdogSprites, mouthSprites; // Array of hot dog sprites (0: full, 1: 1 bite, 2: 2 bites, 3: 3 bites)
    private int bitesTaken = 0; // Track the number of bites taken
    private const int biteCost = 15; // Cost of each bite
    float fullnessAmount = 0;
    private float slider1Speed = 100f; // Speed of slider1 movement
    private float slider2Speed = 100f; // Speed of slider2 movement
    private bool drinkingWater = false;
    public float hitMargin = 10;
    public Animator hotdogTextAnim;
    private void Start()
    {
        // Set sliders' initial values
        slider1.value = 1;
        slider2.value = 99;
        staminaSlider.value = 100; // Start with full stamina
        fullnessSlider.value = 0; // Fullness starts at 0
        burpSlider.value = 0; // Burp meter starts at 0
    }

    private void Update()
    {
        
        if (c.countdown >= 0)
        {
            if (fullnessAmount > 0) fullnessAmount -= Time.deltaTime * 2;
            if (fullnessAmount >= 100) SceneManager.LoadScene("FoodChallenge");
            fullnessSlider.value = fullnessAmount;
            //UpdateSliderSpeeds();
            // Move the sliders up and down
            MoveSliders();

            // Check if both sliders are within the margin and the left mouse button is pressed
            if (Input.GetMouseButtonDown(0))
            {
                staminaSlider.value -= biteCost;
                if (Mathf.Abs(slider1.value - slider2.value) <= hitMargin)
                {
                    TakeBite();
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                drinkSound.Play();
                drinkingWater = true;
            }
            if (Input.GetMouseButtonUp(1))
            {
                drinkingWater = false;
                drinkSound.Stop();
            }

            if (drinkingWater)
            {
                DrinkWater();
            }
            if (Input.GetKeyDown(KeyCode.Space) && burpSlider.value >= 50)
            {
                fartSound.Play();
                Burp();
            }
            // Handle burping
            if (Input.GetKey(KeyCode.Space) && burpSlider.value > 0)
            {


            }
        }
    }
    void MouthAnimation()
    {
        mouthRenderer.sprite = mouthSprites[1];
        sliderMouthImage.sprite = mouthSprites[1];
        Invoke("StopMouthAnim", 0.5f);
    }
    void StopMouthAnim()
    {
        mouthRenderer.sprite = mouthSprites[0];
        sliderMouthImage.sprite = mouthSprites[0];
    }
    private void MoveSliders()
    {
        // Move slider1 up and down
        if (slider1.value >= 100)
        {
            // Reverse direction
            slider1Speed = -slider1DecreaseSpeed;
        }
        else if (slider1.value <= 0)
        {
            // Reverse direction
            slider1Speed = slider1IncreaseSpeed;
        }

        // Update slider1 value based on speed
        slider1.value += slider1Speed * Time.deltaTime;

        // Move slider2 up and down
        if (slider2.value >= 100)
        {
            // Reverse direction
            slider2Speed = -slider2DecreaseSpeed;
        }
        else if (slider2.value <= 0)
        {
            // Reverse direction
            slider2Speed = slider2IncreaseSpeed;
        }

        // Update slider2 value based on speed
        slider2.value += slider2Speed * Time.deltaTime;

        // Clamp slider values to ensure they stay within 0 and 100
        slider1.value = Mathf.Clamp(slider1.value, 0, 100);
        slider2.value = Mathf.Clamp(slider2.value, 0, 100);
    }

    private void TakeBite()
    {
        if (staminaSlider.value >= biteCost && !drinkingWater)
        {
           
            chompSound.Play();
            bitesTaken++;
           // Decrease stamina
            fullnessAmount = Mathf.Clamp(fullnessSlider.value + 8, 0, 110); // Increase fullness
            burpSlider.value = Mathf.Clamp(burpSlider.value + 25, 0, 100); // Increase burp meter
            UpdateHotdogImage();

            if (bitesTaken >= hotdogSprites.Length)
            {
                c.hotdogs++;
                // Reset to full hot dog after the last bite
                ResetHotdog();
            }
            StopMouthAnim();
            MouthAnimation();
        }
        else
        {
       
            thirstySound.Play();
            Debug.Log("Not enough stamina to take a bite!");
        }
    }

    private void UpdateHotdogImage()
    {
        if (bitesTaken < hotdogSprites.Length)
        {
           
            hotdogImage.sprite = hotdogSprites[bitesTaken];
            hotdogImage.gameObject.transform.position = new Vector3(hotdogImage.gameObject.transform.position.x,
                hotdogImage.gameObject.transform.position.y + 1.8f, 0);
            sliderHotdogImage.sprite = hotdogSprites[bitesTaken];
        }
    }

    private void ResetHotdog()
    {
        hotdogImage.gameObject.GetComponent<Animator>().enabled = true;
         hotdogImage.gameObject.transform.position = new Vector3(hotdogImage.gameObject.transform.position.x,
         hotdogImage.gameObject.transform.position.y -7.2f, 0);
        hotdogImage.gameObject.SetActive(false);
        hotdogImage.gameObject.SetActive(true);
    
        bitesTaken = 0; // Reset bite count

        // Increase slider speeds
        slider1IncreaseSpeed *= 1.5f; // Increase by 50%
        slider1DecreaseSpeed += 1.5f;
     //   slider2IncreaseSpeed *= 1.5f; // Increase by 50%
        Debug.Log("Slider speeds increased: " + slider1IncreaseSpeed + ", " + slider2IncreaseSpeed);
        hotdogTextAnim.SetTrigger("Add");
        UpdateHotdogImage(); // Update the image to the full hot dog
    }

    private void Burp()
    {
        burpSlider.value -= 50; // Decrease burp meter
        fullnessAmount -= 10f; // Decrease fullness meter
        if (slider1IncreaseSpeed >= 100)
        {
            slider1IncreaseSpeed -= 50f;
          //  slider1DecreaseSpeed -= 50f;// Increase by 50%
          //  slider2IncreaseSpeed -= 50f;
        }
        // Update slider speeds based on fullness
    }

    private void UpdateSliderSpeeds()
    {
        slider1Speed = 100f - (fullnessSlider.value / 100f * 50f); // Adjust slider1 speed based on fullness
        slider2Speed = 100f - (fullnessSlider.value / 100f * 50f); // Adjust slider2 speed based on fullness
    }

    public void DrinkWater()
    {
        if (staminaSlider.value < 100)
        {
            staminaSlider.value += Time.deltaTime * 50; // Adjust the rate as needed
        }
    }
}