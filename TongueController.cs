using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TongueController : MonoBehaviour
{
    public GameObject sushiMusic;
    bool canIncreaseTaskIndex = true;
    public bool gameActive = true;
    public TextMeshProUGUI loseText;
    public GameObject loseScreen;
    public AudioSource music;
    public int lives = 3;

    public TextMeshProUGUI sushisCaughtText, highScoreText, livesText;
    public int sushisCaught = 0;
    int highScore;
    public Transform mouth;
    public Transform tongueTip;
    public LineRenderer lineRenderer;
    public float maxStretchLength = 3f;
    public Collider2D mouthCollider; // Add a reference to the mouth's collider

    private bool isStretching = false;
    private Vector2 stretchEnd;
    private GameObject caughtSushi = null;
    public AudioSource pickupSound;


    private void Start()
    {
        gameActive = true;
        highScore = PlayerPrefs.GetInt("SushiHighScore");
        if (GameManager.Instance.isFinale)
        {
            livesText.gameObject.SetActive(false);
            sushiMusic.SetActive(false);
        }
    }
    void Update()
    {
        if (gameActive)
        {
            if(sushisCaught >= 50 && GameManager.Instance.isFinale)
            {
                SceneManager.LoadScene("ChiliChallenge");
            }
            if (lives <= 0 && !GameManager.Instance.isFinale) GameOver();
            livesText.text = "Lives: " + lives;
            highScoreText.text = "High: " + highScore;
            if (sushisCaught >= highScore)
            {
                highScore = sushisCaught;
                PlayerPrefs.SetInt("SushiHighScore", sushisCaught);
            }
            sushisCaughtText.text = "Sushis: " + sushisCaught;
            // Check if mouse click starts on the mouth
            if (Input.GetMouseButtonDown(0))
            {

                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (mouthCollider.OverlapPoint(mousePos)) // Check if click is on the mouth
                {
                    isStretching = true;
                    //  pickupSound.Play();
                }
            }

            // Only allow tongue stretching if the initial click was on the mouth
            if (Input.GetMouseButton(0) && isStretching)
            {

                stretchEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                stretchEnd = Vector2.ClampMagnitude(stretchEnd - (Vector2)mouth.position, maxStretchLength) + (Vector2)mouth.position;
                tongueTip.position = stretchEnd;
                DrawTongue();

                // If sushi is caught, make it follow the tongue tip
                if (caughtSushi != null)
                {
                    caughtSushi.transform.position = tongueTip.position;
                }
            }

            // Check if the tongue touches the sushi
            if (caughtSushi != null && Vector2.Distance(tongueTip.position, caughtSushi.transform.position) < 0.1f)
            {
                // Start retracting immediately once the sushi is caught
                RetractTongue();
            }

            // Reset tongue stretch when the mouse button is released
            if (Input.GetMouseButtonUp(0))
            {
                isStretching = false;
                RetractTongue();
            }
        }
    }
    public void GameOver()
    {
        gameActive = false;
        //     Time.timeScale = 0;
        loseScreen.SetActive(true);
        music.Stop();
        loseText.text = "You ate " + sushisCaught + " sushis and won " + "£" + 
            (25 + (sushisCaught * 10)) + "!" + "You gained " + sushisCaught + " myspace followers!";
        if(!GameManager.Instance.gotSushiTshirt)
        {
            GameManager.Instance.tick6.SetActive(true);
            GameManager.Instance.tShirts++;
            GameManager.Instance.gotSushiTshirt = true;
        }
       //SceneManager.LoadScene("SushiChallenge");
    }
    public void BackToTown()
    {

        GameManager.Instance.sushiVisitsCount++;

        float reputationToAdd = Mathf.Min(sushisCaught, 50);
        GameManager.Instance.reputation += reputationToAdd;

        GameManager.Instance.money += 25 + sushisCaught * 10;
        GameManager.Instance.dailyTasksCompleted = true;
        if (canIncreaseTaskIndex)
        {
            GameManager.Instance.taskIndex++;
            canIncreaseTaskIndex = false;
        }
        SceneManager.LoadScene("OutsidePub");
      //  Time.timeScale = 1;
    }
    void DrawTongue()
    {
        lineRenderer.SetPosition(0, mouth.position);
        lineRenderer.SetPosition(1, stretchEnd);
    }

    void RetractTongue()
    {
        StartCoroutine(RetractCoroutine());
    }

    IEnumerator RetractCoroutine()
    {
        float elapsed = 0f;
        float retractTime = 0.1f; // Time to retract
        Vector2 start = stretchEnd;

        // While the tongue is retracting, move it towards the mouth
        while (elapsed < retractTime)
        {
            stretchEnd = Vector2.Lerp(start, mouth.position, elapsed / retractTime);
            tongueTip.position = stretchEnd;
            DrawTongue();

            // If sushi is caught, make it follow the tongue tip
            if (caughtSushi != null)
            {
                caughtSushi.transform.position = tongueTip.position;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Once the tongue finishes retracting, check if sushi is near the mouth
        if (caughtSushi != null && Vector2.Distance(mouth.position, caughtSushi.transform.position) < 0.1f)
        {
            Destroy(caughtSushi);  // "Eat" the sushi
            caughtSushi = null;
        }
        isStretching = false;
        lineRenderer.SetPosition(1, mouth.position); // Reset tongue
    }

    // This method is called when the sushi collides with the tongue
    public void CatchSushi(GameObject sushi)
    {
        if (caughtSushi == null) // Only catch sushi if none is already caught
        {
            sushisCaught++;
            caughtSushi = sushi; // Set the caught sushi
            if (GameManager.Instance.isFinale) GameManager.Instance.finalePlayerCounter++;
        }
    }

    // Optional: This function can be called if you want to manually detect sushi on the tongue
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sushi") && caughtSushi == null)  // Check if sushi is not already caught
        {
            CatchSushi(other.gameObject); // Catch the sushi if it touches the tongue
        }
    }
}