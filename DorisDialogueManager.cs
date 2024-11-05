using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cinemachine;


public class DorisDialogueManager : MonoBehaviour
{
    public AudioSource lieSound;
    public CinemachineVirtualCamera cam2;
    public AudioSource[] brotherSounds;
    public TextMeshProUGUI startScreenText;
    public GameObject startDayScreen;
    public AudioSource rockMusic, brotherAudio;
    public AudioSource sliderSound;
    int dayTracker = 0;
    public TextMeshProUGUI taskText;
    public TextMeshProUGUI testText;
    public GameObject gameOverScreen;
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;
    public GameObject meterUI; // UI for the mini-game
    public Slider slider;
    public Image meterFill; // Fill image for the meter

    [TextArea(3, 10)]
    public string[] dialogueLines;
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;

    private float meterValue = 0f; // Current value of the meter
    private bool miniGameActive = false; // Is the mini-game active?
    AudioSource music;
    private void Start()
    {
        music = GameObject.Find("Music").GetComponent<AudioSource>();
        dialogueText.gameObject.SetActive(false);
        currentLineIndex = 0;
        if (GameManager.Instance.day >=2)
        {
            rockMusic.Stop();
            if(GameManager.Instance.houseVisits <=0)
            {
                brotherAudio.Stop();
                music.Stop();
                startDayScreen.SetActive(true);
                startScreenText.text = "Day " + GameManager.Instance.day;
                Invoke("TurnOffStartScreen", 5);
            }
        }

        // Check the current day and house visits
        if (GameManager.Instance.day == 1)
        {
            if (GameManager.Instance.houseVisits == 0)
            {
                rockMusic.Stop();
                cam2.Priority = 20;
                Invoke("WaitToStartDialogue", 3.2f);
            }
        }
        else if (GameManager.Instance.day == 2)
        {
            
                // Display the task text for day 2
                taskText.gameObject.SetActive(true);
                taskText.text = GameManager.Instance.taskTexts[GameManager.Instance.taskIndex];
           
        }
       
        // Increment house visits after the condition checks
        GameManager.Instance.houseVisits++;
    }
    private void Update()
    {
       
        if(miniGameActive)
        {
            sliderSound.pitch = meterValue / 50;
        }
            testText.text = "Day " + GameManager.Instance.day + " - " + "Visit " + GameManager.Instance.houseVisits;

            if (isDialogueActive && Input.GetMouseButtonDown(0))
            {
                if (miniGameActive)
                {
                    // Check if the meter value is above 80% when clicked
                    if (meterValue > 80f)
                    {
                        StartCoroutine(EndMiniGame(true)); // Player succeeded in the mini-game
                    }
                    else
                    {
                        StartCoroutine(EndMiniGame(false)); // Player failed the mini-game
                    }
                }
                else
                {
                    currentLineIndex++;
                    DisplayLine();
                }
            
        }
    }
    void WaitToStartDialogue()
    {
        cam2.Priority = 0;
        rockMusic.Play();
        StartDialogue();

    }
    void TurnOffStartScreen()
    {
        startDayScreen.SetActive(false);
        music.Play();
        brotherAudio.Play();
    }
    public void StartDialogue()
    {
        dialogueText.gameObject.SetActive(true);
        if (dialogueLines.Length == 0)
        {
            Debug.LogWarning("Dialogue lines are empty!");
            return;
        }

        currentLineIndex = 0;
        isDialogueActive = true;

        ShowDialogueUI(true);
        DisplayLine();
    }
    void ChooseBrotherSound()
    {
        int i = Random.Range(0, 2);
        brotherSounds[i].Play();
    }
    private void DisplayLine()
    {
       
        if (currentLineIndex < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLineIndex];
            
                // Check for the specific dialogue line to start the mini-game
                if (currentLineIndex == 4) // Assuming the mini-game starts on the 5th line (index 4)
            {
                StartMiniGame();
            }
            if (currentLineIndex == 1 || currentLineIndex == 3 || currentLineIndex == 6 ||
               currentLineIndex == 8 || currentLineIndex == 10 || currentLineIndex == 12)
            {
                ChooseBrotherSound();
            }
        }
        else
        {
            
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        taskText.gameObject.SetActive(true);
        taskText.text = GameManager.Instance.taskTexts[GameManager.Instance.taskIndex];
        GameManager.Instance.taskIndex++;
        ShowDialogueUI(false);
        isDialogueActive = false;
        GameManager.Instance.metDoris = true;
        GameManager.Instance.dailyTasksCompleted = true;
    }

    private void StartMiniGame()
    {
        sliderSound.Play();
        meterUI.SetActive(true); // Show the meter UI
        meterValue = 0f; // Reset meter value
        miniGameActive = true; // Mark mini-game as active
        StartCoroutine(MiniGameCoroutine()); // Start the mini-game coroutine
    }

    private IEnumerator MiniGameCoroutine()
    {
        float timer = 0f; // Create a timer variable
        while (miniGameActive)
        {
            timer += Time.deltaTime; // Increment timer by delta time
            meterValue = Mathf.PingPong(timer * 150f, 100); // Update meter value

            // Update the fill image based on meterValue
            slider.value = meterValue;

            Debug.Log($"Meter Value: {meterValue}"); // Log current meter value
            yield return null; // Wait for the next frame
        }
    }

   

    private IEnumerator EndMiniGame(bool success)
    {
        miniGameActive = false; // Stop the mini-game
        meterUI.SetActive(false); // Hide the meter UI

        // Check if the mini-game was successful
        if (success)
        {
            lieSound.Play();
            //dialogueText.text = "Yes my dear, fantastic, brilliant, first class.";
            currentLineIndex ++;
            sliderSound.Stop();
        }
        else
        {
            rockMusic.Stop();
            
            dialogueText.text = "BAD. I GOT FIRED FOR BREAKING THE TOILET!";
            yield return new WaitForSeconds(3f); // Wait before showing the game over screen
            ShowGameOverScreen(); // Call the game over logic
        }

        // Move to the next dialogue line
        DisplayLine(); // Display the next line
    }

    private void ShowGameOverScreen()
    {
        GameManager.Instance.houseVisits = 0;
        // Implement your game over logic here, e.g., showing a game over UI
        Debug.Log("Game Over!");
        gameOverScreen.SetActive(true);
    }

    private void ShowDialogueUI(bool show)
    {
        dialogueUI.SetActive(show);
    }
}