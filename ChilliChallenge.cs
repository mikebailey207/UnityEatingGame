using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;

public class ChilliChallenge : MonoBehaviour
{
    public AudioSource beepSound;
    public GameObject waterParticles;
    public GameObject kathyHead;
    public GameObject finaleWinScreen;
    public TextMeshProUGUI winText;
    public GameObject winScreen;
    bool dead = false;
    public GameObject chiliMusic;

    bool canDie = true;

    public GameObject otherCanvas1, otherCanvas2;
    public AudioSource explodeSound, music, uhohSound, scratchSound;
    public CinemachineVirtualCamera zoomCam;
    public GameObject explodeParticles;
    public GameObject playerSprite, mouthSprite;
    public GameObject disguise;
    public Transform particleSpawnPoint;
    public GameObject loseScreen;

    int chilisEaten = 0;
    bool canEndScene = true;
    bool canPlayGagSound = true;

    int biteCost = 8; // Cost of each bite
    float fullnessAmount = 0;
    private bool drinkingWater = false;
    public Slider staminaSlider; // Stamina slider
    public Slider fullnessSlider; // Fullness slider
    public Slider burpSlider;
    public GameObject warningScreen;


    public Image redImage;
    public CinemachineVirtualCamera virtualCamera; // Reference to the virtual camera
    private CinemachineBasicMultiChannelPerlin noiseModule; // Reference to the noise module
    public float amplitudeIncreaseRate = 0.1f; // The rate at which amplitude increases
    public float frequencyIncreaseRate = 0.05f; // The rate at which frequency increases
    public float maxAmplitude = 2f; // Max amplitude of the noise
    public float maxFrequency = 2f; // Max
    public ParticleSystem sweat;
    public GameObject chilliParticles;



    float scovilleAmount;
    public TextMeshProUGUI scovillesText;
    public bool scovilleChallenge = false;
    public LineRenderer lineRenderer;  // Reference to LineRenderer
    private GameObject mouthStartObject;
    private bool isMouthDashing = false;
    public AudioSource drumRoll, technoMusic, groanSound, chompSound, drinkSound, thirstySound, gagSound;
    public Slider courageSlider; // UI slider for displaying courage level
    public GameObject targetCirclePrefab; // Prefab for the shrinking target circle
    public GameObject mouth, closedMouth; // Reference to the mouth GameObject
    public float courageGain = 0.02f; // Amount of courage gained per button press
    public float courageDecayRate = 0.01f; // Rate at which courage decays over time
    public float targetShrinkSpeed = 1.0f; // Initial shrink speed for target circles
    public int chewsToEatChilli = 5; // Number of successful clicks to eat a chilli
    public float movementSpeed = 1.0f; // Speed at which the target circle moves
    private float courage = 0f;
    private int chewsLeft;
    private GameObject activeTargetCircle;
    private bool isChewingPhase = false;
    private Vector3 mouthStartPosition;
    private Vector3 movementDirection;

    int finaleTarget = 50;
    void Start()
    {
        if (GameManager.Instance.isFinale)
        {
            chiliMusic.SetActive(false);
            kathyHead.SetActive(false);
        }
        
        if (virtualCamera != null)
        {
            noiseModule = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        staminaSlider.value = 100;
    
        courageSlider.value = courage;
        courageSlider.gameObject.SetActive(true);
        if (!scovilleChallenge) StartCourageBuildingPhase();
        else StartChewingPhase();
        mouthStartPosition = mouth.transform.position; // Store the original position of the mouth
        mouthStartObject = new GameObject("MouthStartPoint"); // Create the start point object
        mouthStartObject.transform.position = mouthStartPosition;
        scovillesText.text = "Schofields: " + scovilleAmount.ToString("0");
    }

    void Update()
    {
        if (GameManager.Instance.finalePlayerCounter >= 170 && GameManager.Instance.isFinale)
        {
            GameManager.Instance.StopBardEatingCoroutine();
            GameManager.Instance.finaleMusic.Stop();
            GameManager.Instance.finaleSlider.gameObject.SetActive(false);
            GameManager.Instance.finaleSliderBard.gameObject.SetActive(false);
            GameManager.Instance.tshirtsText.gameObject.SetActive(false);
            GameManager.Instance.finalePlayerCounterText.gameObject.SetActive(false);
            GameManager.Instance.finaleBardCounterText.gameObject.SetActive(false);
            finaleWinScreen.SetActive(true);
            GameManager.Instance.StopBardEatingCoroutine();
            GameManager.Instance.finaleMusic.Stop();
          
            StopAllCoroutines();
            //SceneManager.LoadScene("EndSceneTown");
            // THIS IS THE WIN GAME CODE!#
            Debug.Log("WE HAVE REACHED THE END GAME MUTHAFUCKAS!");
        }
        fullnessSlider.value = fullnessAmount;
        if (fullnessAmount >= 80)
        {
          
            warningScreen.SetActive(true);
            PlayGagSound();
        }
        else
        {
           
            warningScreen.SetActive(false);
            canPlayGagSound = true;
        }
        if (fullnessAmount >= 100)
        {
            if (!GameManager.Instance.isFinale)
            {
                dead = true;
                zoomCam.Priority = 20;
                music.Stop();
              
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
            else if (GameManager.Instance.isFinale)
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
        if (!isChewingPhase)
        {
            courage -= courageDecayRate * Time.deltaTime;
            courage = Mathf.Clamp01(courage);
            courageSlider.value = courage;

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.A))
            {
                courage += courageGain;
                courage = Mathf.Clamp01(courage);
                courageSlider.value = courage;

                if (courage >= 1f)
                {
                    StartChewingPhase();
                }
            }
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

    public void LoadFinalScene()
    {
        GameManager.Instance.talking = true;
        SceneManager.LoadScene("EndSceneTown");
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
        playerSprite.SetActive(false);
        yield return new WaitForSeconds(1);
        disguise.GetComponent<Rigidbody2D>().isKinematic = false;
        disguise.GetComponent<Rigidbody2D>().gravityScale = 10;
      
        yield return new WaitForSeconds(2);
        winScreen.SetActive(true);
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene("ChiliChallenge");
    }
    public void GiveUp()
    {
        SceneManager.LoadScene("TownEntrace");
    }
    void PlayGagSound()
    {
        if (canPlayGagSound)
        {
            gagSound.Play();
            canPlayGagSound = false;
        }
    }
    private void Burp()
    {
        burpSlider.value -= 50; // Decrease burp meter
        fullnessAmount -= 10f + GameManager.Instance.burpAddMultiplier; // Decrease fullness meter
    }
    void StartCourageBuildingPhase()
    {
        technoMusic.Stop();
        drumRoll.Play();
        isChewingPhase = false;
        courage = 0f;
        courageSlider.value = courage;
        courageSlider.gameObject.SetActive(true);
        chewsLeft = chewsToEatChilli;
    }

    void StartChewingPhase()
    {
        technoMusic.Play();
        drumRoll.Stop();
        isChewingPhase = true;
        courageSlider.gameObject.SetActive(false);
        chewsLeft = chewsToEatChilli;
        SpawnTargetCircle();
    }

    void SpawnTargetCircle()
    {
        if (activeTargetCircle != null) Destroy(activeTargetCircle);

        float circleRadius = targetCirclePrefab.transform.localScale.x / 2f;
        float minX = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x + circleRadius;
        float maxX = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x - circleRadius;
        float minY = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y + circleRadius;
        float maxY = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y - circleRadius;

        Vector2 randomPosition = new Vector2(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY)
        );

        // Determine a random movement direction (in 2D space)
        movementDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;

        activeTargetCircle = Instantiate(targetCirclePrefab, randomPosition, Quaternion.identity);
        StartCoroutine(ShrinkAndAwaitClick());
    }

    IEnumerator ShrinkAndAwaitClick()
    {
       
            if (activeTargetCircle == null) yield break; // Ensure activeTargetCircle is not null before proceeding

            Transform targetTransform = activeTargetCircle.transform;
            Vector3 initialScale = targetTransform.localScale;

            while (targetTransform.localScale.x > 0)
            {
                if (activeTargetCircle == null) yield break; // Check if the target has been destroyed during the loop

                targetTransform.localScale -= new Vector3(targetShrinkSpeed, targetShrinkSpeed, 0) * Time.deltaTime;

                // Move the circle in the random direction
                targetTransform.position += movementDirection * movementSpeed * Time.deltaTime;

                if (Input.GetMouseButtonDown(0) && !drinkingWater)
                {

                    if (staminaSlider.value < 90) thirstySound.Play();
                    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (activeTargetCircle != null && activeTargetCircle.GetComponent<Collider2D>().OverlapPoint(mousePosition)
                        && staminaSlider.value >= 90)
                    {
                        Destroy(activeTargetCircle);
                        groanSound.pitch = Random.Range(0.7f, 1.3f);
                        groanSound.Play();
                        chompSound.Play();

                        // Trigger mouth dash only if not already dashing
                        if (!isMouthDashing)
                        {
                            StartCoroutine(MouthDash(targetTransform.position));
                        }

                    if (!scovilleChallenge)
                    {
                        chewsLeft--;

                        if (chewsLeft <= 0)
                        {
                            EatChilli();
                        }
                        else
                        {
                            SpawnTargetCircle();
                        }
                    }
                    else
                    {
                        
                            groanSound.pitch = Random.Range(0.7f, 1.3f);
                            groanSound.Play();
                            chompSound.Play();
                            Instantiate(chilliParticles, activeTargetCircle.transform.position, Quaternion.identity);
                            EatChilliScoville();
                            SpawnTargetCircle();
                        
                    }

                        yield break; // Exit the coroutine after processing the click
                    }
                }

                yield return null;
            }
            if (!GameManager.Instance.isFinale && !dead) GameOver(); // If the loop ends, the game is over
            else if(GameManager.Instance.isFinale && GameManager.Instance.finalePlayerCounter >= 120)
            {
                SpawnTargetCircle();
                GameManager.Instance.finalePlayerCounter--;

            }
        
    }

    IEnumerator MouthDash(Vector3 targetPosition)
    {
        isMouthDashing = true;  // Mark that the dash has started

        float dashSpeed = 50f;
        float pauseDuration = 0.1f;
        float threshold = 0.1f; // Tolerance for precision issues

        // Initialize and configure the LineRenderer
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, mouthStartObject.transform.position); // Set the start position
        lineRenderer.SetPosition(1, mouth.transform.position); // Initially set the end point to the mouth's start position

        // Move towards the target position
        while (Vector3.Distance(mouth.transform.position, targetPosition) > threshold)
        {
            mouth.transform.position = Vector3.MoveTowards(mouth.transform.position, targetPosition, dashSpeed * Time.deltaTime);

            // Update the LineRenderer position
            lineRenderer.SetPosition(1, mouth.transform.position);

            yield return null;
        }

        // Ensure mouth reaches target position exactly
        mouth.transform.position = targetPosition;

        closedMouth.SetActive(true);  // Show closed mouth animation
        yield return new WaitForSeconds(pauseDuration);
        closedMouth.SetActive(false); // Hide closed mouth animation

        // Move back to the starting position
        while (Vector3.Distance(mouth.transform.position, mouthStartPosition) > threshold)
        {
            mouth.transform.position = Vector3.MoveTowards(mouth.transform.position, mouthStartPosition, dashSpeed * Time.deltaTime);

            // Update the LineRenderer position
            lineRenderer.SetPosition(1, mouth.transform.position);

            yield return null;
        }

        // Ensure mouth reaches starting position exactly
        mouth.transform.position = mouthStartPosition;

        // Remove the LineRenderer once the dash is finished
        lineRenderer.positionCount = 0;

        isMouthDashing = false;  // Mark that the dash has finished
    }
    void EatChilliScoville()
    {
       if(GameManager.Instance.isFinale) GameManager.Instance.finalePlayerCounter++;
        staminaSlider.value = 0;
       // Modify the emission rate of the sweat particles
        var emission = sweat.emission;
        emission.rateOverTime = emission.rateOverTime.constant + 1;

        // Increase target shrink speed and movement speed
        targetShrinkSpeed += 0.01f;
        movementSpeed += 0.02f;

        // Randomize the heat of the pepper and add it to the scoville count
        float pepperHeat = Random.Range(7000, 10000);
        scovilleAmount += pepperHeat;

        // Modify the camera noise properties
        noiseModule.m_AmplitudeGain += amplitudeIncreaseRate;
        noiseModule.m_FrequencyGain += frequencyIncreaseRate;

        // Update the Scoville count display
        scovillesText.text = "Schofields: " + scovilleAmount.ToString("0");

        fullnessAmount += biteCost - GameManager.Instance.biteCostMultiplier; // Increase fullness
        burpSlider.value = Mathf.Clamp(burpSlider.value + 25 + GameManager.Instance.burpAddMultiplier, 0, 100);
        if(GameManager.Instance.isFinale && GameManager.Instance.finalePlayerCounter >= 160)
        {
            beepSound.Play();
            beepSound.pitch += 0.1f;
        }
        // Start the alpha fade effect for the red image
        StartCoroutine(FadeRedImage());
    }
    IEnumerator FadeRedImage()
    {
        float duration = 0.5f; // Time to fade back to 0
        float targetAlpha = 0.6f; // Target alpha when biting
        Color startColor = redImage.color; // Store the current color of the image

        // Fade the image to the target alpha (0.6) over the duration
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(startColor.a, targetAlpha, elapsedTime / duration);
            redImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Set the alpha to exactly 0.6f at the end of the fade
        redImage.color = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);

        // Wait a brief moment at 0.6f before fading back to 0
        yield return new WaitForSeconds(0.1f); // Adjust the wait time as needed

        // Fade the image alpha back to 0
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(targetAlpha, 0f, elapsedTime / duration);
            redImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Set the alpha to exactly 0 at the end of the fade
        redImage.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
    }

    void EatChilli()
    {
        courageDecayRate += 0.05f;
        targetShrinkSpeed += 0.1f;
        movementSpeed += 0.2f;
        StartCourageBuildingPhase();
        if (GameManager.Instance.isFinale) GameManager.Instance.finalePlayerCounter++;
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        if (canEndScene)
        {
            GameManager.Instance.chilliVisitsCount++;
            GameManager.Instance.dailyTasksCompleted = true;
            GameManager.Instance.taskIndex++;

            float reputationToAdd = Mathf.Min(scovilleAmount/10000, 50);
            GameManager.Instance.reputation += reputationToAdd;
            
            GameManager.Instance.money += 25 + scovilleAmount / 1000;
            canEndScene = false;
            if(!GameManager.Instance.gotChiliTshirt)
            {
                GameManager.Instance.tick5.SetActive(true);
                GameManager.Instance.tShirts++;
                GameManager.Instance.gotChiliTshirt = true;
            }
        }
        winText.text = "You ate " + scovilleAmount.ToString("0") + " schofields and won " + "£" +
            (scovilleAmount/1000).ToString("0") + "!" + "You gained " + (scovilleAmount/10000).ToString("0") + " myspace followers!";
        winScreen.SetActive(true);
        StopAllCoroutines();
//        SceneManager.LoadScene("OutsidePub");
    }
    public void LoadTown()
    {
        SceneManager.LoadScene("OutsidePub");
    }
    public void DrinkWater()
    {
        if (staminaSlider.value < 100)
        {
            staminaSlider.value += Time.deltaTime * 50 * GameManager.Instance.drinkRefreshMultiplier; // Adjust the rate as needed
        }
    }
}
