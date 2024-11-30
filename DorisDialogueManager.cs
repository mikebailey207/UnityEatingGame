using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cinemachine;


public class DorisDialogueManager : MonoBehaviour
{
    public GameObject newTaskText;
    public TextMeshProUGUI newTaskTextText;
    public AudioSource moneySound;
    bool canContinue = true;
    public GameObject dorisScreen, doris, doris2, doris3, dalmations, littleMermaid;
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
    public GameObject gameOverScreen, gameOverScreenAfterDay1, gameSavedText;
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;
    public GameObject meterUI; // UI for the mini-game
    public Slider slider;
    public Image meterFill; // Fill image for the meter

    [TextArea(3, 10)]
    public string[] dialogueLinesMorning4;
    [TextArea(3, 10)]
    private bool isMorning4DialogueActive = false;

    [TextArea(3, 10)]
    public string[] dialogueLinesMorning8;
    [TextArea(3, 10)]
    private bool isMorning8DialogueActive = false;

    [TextArea(3, 10)]
    public string[] dialogueLinesPost350;
    [TextArea(3, 10)]
    private bool isPost350DialogueActive = false;

    public string[] dialogueLines;

    [TextArea(3, 10)]
    public string[] dialogueLinesAfterDay2;
    private int currentLineIndex = 0;
 //   int index2 = 0;
    private bool isDialogueActive = false;

    private float meterValue = 0f; // Current value of the meter
    private bool miniGameActive = false; // Is the mini-game active?
    AudioSource music;
    private void Start()
    {
     
        music = GameObject.Find("Music").GetComponent<AudioSource>();
        dialogueText.gameObject.SetActive(false);
        currentLineIndex = 0;
        if(GameManager.Instance.day == 4)
        {
           // Day4Morning();
        }
        if (GameManager.Instance.day >=2)
        {
            //GameManager.Instance.SaveGame();
            
            rockMusic.Stop();
            dorisScreen.SetActive(false);
            if(GameManager.Instance.houseVisits <=0)
            {
                // THIS IS THE START OF TH DAY!!!
                GameManager.Instance.canStartDay = false;
                brotherAudio.Stop();
                music.Stop();
                startDayScreen.SetActive(true);
                startScreenText.text = "Day " + GameManager.Instance.day;
                Invoke("TurnOffStartScreen", 5);
            }
            else if (GameManager.Instance.houseVisits > 0)
            {
                if (GameManager.Instance.reputation <599.5f)
                {
                    doris.SetActive(true);
                    if (GameManager.Instance.dailyTasksCompleted && !GameManager.Instance.paidDoris)
                    {
                        StartMiniGame();
                        dialogueUI.SetActive(true);
                        dialogueText.text = "YOU: HOW WAS WORK? IT WAS.......";

                        GameManager.Instance.talking = true;
                        // GameManager.Instance.money -= 50;
                        // GameManager.Instance.paidDoris = true;
                    }
                }
                else if(GameManager.Instance.reputation >= 599.5f)
                {
                    doris.SetActive(true);
                    if (GameManager.Instance.dailyTasksCompleted && !GameManager.Instance.paidDoris)
                    {
                        if (GameManager.Instance.canShowWimbledonDialogue)
                        {
                            Post350Homecoming();
                        }
                        else if(!GameManager.Instance.canShowWimbledonDialogue)
                        {
                            StartMiniGame();
                            dialogueUI.SetActive(true);
                            dialogueText.text = "YOU: HOW WAS WORK? IT WAS.......";

                            GameManager.Instance.talking = true;
                        }
                    }
                }
            }
        }

        // Check the current day and house visits
        if (GameManager.Instance.day == 1)
        {
            if (GameManager.Instance.houseVisits == 0)
            {
                GameManager.Instance.talking = true;
                rockMusic.Stop();
                cam2.Priority = 20;
                Invoke("TurnOffDorisScreen", 3f);
            }
        }
        else if (GameManager.Instance.day >= 2)
        {
            
                // Display the task text for day 2
                taskText.gameObject.SetActive(true);
                taskText.text = GameManager.Instance.taskTexts[GameManager.Instance.taskIndex];
           
        }
       
        // Increment house visits after the condition checks
        GameManager.Instance.houseVisits++;
        GameManager.Instance.beenShopping = false;
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
                if (canContinue)
                {
                    currentLineIndex++;
                    DisplayLine();
                }
                }
            
                
        }
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.day >= 2 && GameManager.Instance.dailyTasksCompleted)
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
        }
        if (isMorning4DialogueActive && Input.GetMouseButtonDown(0))
        {
            currentLineIndex++;
            DisplayLineMorning4();
        }
        if (isMorning8DialogueActive && Input.GetMouseButtonDown(0))
        {
            currentLineIndex++;
            DisplayLineMorning8();
        }
        if (isPost350DialogueActive && Input.GetMouseButtonDown(0))
        {
            currentLineIndex++;
            DisplayLinePost350();
        }
    }
    void TurnOffDorisScreen()
    {
        doris.SetActive(true);
        dorisScreen.SetActive(false);
        rockMusic.Stop();
        cam2.Priority = 20;
        Invoke("WaitToStartDialogue", 3f);
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
        gameSavedText.SetActive(false);
       
        GameManager.Instance.canStartDay = true;
        newTaskTextText.text = GameManager.Instance.taskTexts[GameManager.Instance.taskIndex] + "!";
        newTaskText.SetActive(true);
        if (GameManager.Instance.day == 4)
        {
            newTaskText.SetActive(false);
            GameManager.Instance.canStartDay = false;
            Day4Morning();
        }
        if (GameManager.Instance.day == 8)
        {
            newTaskText.SetActive(false);
            GameManager.Instance.canStartDay = false;
            Day8Morning();
        }
        
    }

    public void Post350Homecoming()
    {
        taskText.gameObject.SetActive(false);
        GameManager.Instance.moneyText.gameObject.SetActive(false);
        GameObject player = GameObject.Find("Player");
        player.GetComponent<SceneChanger>().interactText.gameObject.SetActive(false);
       
        music.Stop();
        rockMusic.Play();
        GameManager.Instance.talking = true;
        dialogueUI.SetActive(true);
        //THIS IS WHERE IT HAS TO START A NEW DIALOGUE, THAT GETS PROGRESSED ON CLICKS, AND USES THE TEXT DROM dialogueLinesMorning4

        isPost350DialogueActive = true;
        currentLineIndex = 0; // Reset index for new dialogue
        ShowDialogueUI(true);
        DisplayLinePost350();
    }
    public void Day4Morning()
    {
        taskText.gameObject.SetActive(false);
        GameManager.Instance.moneyText.gameObject.SetActive(false);
        GameObject player = GameObject.Find("Player");
        player.GetComponent<SceneChanger>().interactText.gameObject.SetActive(false);
        doris2.SetActive(true);
        dalmations.SetActive(true);
        music.Stop();
        rockMusic.Play();
        GameManager.Instance.talking = true;
        dialogueUI.SetActive(true);
        //THIS IS WHERE IT HAS TO START A NEW DIALOGUE, THAT GETS PROGRESSED ON CLICKS, AND USES THE TEXT DROM dialogueLinesMorning4

        isMorning4DialogueActive = true;
        currentLineIndex = 0; // Reset index for new dialogue
        ShowDialogueUI(true);
        DisplayLineMorning4();
    }
    public void Day8Morning()
    {
        taskText.gameObject.SetActive(false);
        GameManager.Instance.moneyText.gameObject.SetActive(false);
        GameObject player = GameObject.Find("Player");
        player.GetComponent<SceneChanger>().interactText.gameObject.SetActive(false);
        doris3.SetActive(true);
        littleMermaid.SetActive(true);
        music.Stop();
        rockMusic.Play();
        GameManager.Instance.talking = true;
        dialogueUI.SetActive(true);
        //THIS IS WHERE IT HAS TO START A NEW DIALOGUE, THAT GETS PROGRESSED ON CLICKS, AND USES THE TEXT DROM dialogueLinesMorning4

        isMorning8DialogueActive = true;
        currentLineIndex = 0; // Reset index for new dialogue
        ShowDialogueUI(true);
        DisplayLineMorning8();
    }
    private void DisplayLineMorning4()
    {
        if (currentLineIndex < dialogueLinesMorning4.Length)
        {
            dialogueText.text = dialogueLinesMorning4[currentLineIndex];
            if (currentLineIndex == 2 || currentLineIndex == 4 || currentLineIndex == 6||
               currentLineIndex == 8 || currentLineIndex == 10 || currentLineIndex == 12)
            {
                ChooseBrotherSound();
            }
        }
        else
        {
            EndDialogueMorning4();
        }
    }
    private void DisplayLinePost350()
    {
        if (currentLineIndex < dialogueLinesPost350.Length)
        {
            dialogueText.text = dialogueLinesPost350[currentLineIndex];
            if (currentLineIndex == 2 || currentLineIndex == 4 || currentLineIndex == 6 ||
               currentLineIndex == 8 || currentLineIndex == 10 || currentLineIndex == 12)
            {
                ChooseBrotherSound();
            }
        }
        else
        {
            EndDialoguePost350();
        }
    }
    private void DisplayLineMorning8()
    {
        if (currentLineIndex < dialogueLinesMorning8.Length)
        {
            dialogueText.text = dialogueLinesMorning8[currentLineIndex];
            if (currentLineIndex == 2 || currentLineIndex == 3 || currentLineIndex == 6 ||
               currentLineIndex == 8|| currentLineIndex == 10)
            {
                ChooseBrotherSound();
            }
        }
        else
        {
            EndDialogueMorning8();
        }
    }
    private void EndDialogueMorning4()
    {
        GameManager.Instance.moneyText.gameObject.SetActive(true);
        isMorning4DialogueActive = false;
        ShowDialogueUI(false);
        GameManager.Instance.talking = false;
        taskText.gameObject.SetActive(true);
        taskText.text = GameManager.Instance.taskTexts[GameManager.Instance.taskIndex];
        rockMusic.Stop();
        music.Play();
        taskText.gameObject.SetActive(true);
        newTaskTextText.text = GameManager.Instance.taskTexts[GameManager.Instance.taskIndex] + "!";
        newTaskText.SetActive(true);
        GameManager.Instance.canStartDay = true;
       // GetComponent<SceneChanger>().interactText.gameObject.SetActive(true);

    }
    private void EndDialogueMorning8()
    {
        GameManager.Instance.moneyText.gameObject.SetActive(true);
        isMorning8DialogueActive = false;
        ShowDialogueUI(false);
        GameManager.Instance.talking = false;
        taskText.gameObject.SetActive(true);
        taskText.text = GameManager.Instance.taskTexts[GameManager.Instance.taskIndex];
        rockMusic.Stop();
        music.Play();
        taskText.gameObject.SetActive(true);
        newTaskTextText.text = GameManager.Instance.taskTexts[GameManager.Instance.taskIndex] + "!";
        newTaskText.SetActive(true);
        GameManager.Instance.canStartDay = true;
      //  GetComponent<SceneChanger>().interactText.gameObject.SetActive(true);

    }
    private void EndDialoguePost350()
    {
        GameManager.Instance.moneyText.gameObject.SetActive(true);
        isPost350DialogueActive = false;
        ShowDialogueUI(false);
        GameManager.Instance.talking = false;
        taskText.gameObject.SetActive(true);
        taskText.text = GameManager.Instance.taskTexts[GameManager.Instance.taskIndex];
        rockMusic.Stop();
        music.Play();
        taskText.gameObject.SetActive(true);
        GameManager.Instance.canStartDay = true;
        newTaskTextText.text = GameManager.Instance.taskTexts[GameManager.Instance.taskIndex] + "!";
        newTaskText.SetActive(true);
        //GetComponent<SceneChanger>().interactText.gameObject.SetActive(true);
        GameManager.Instance.canShowWimbledonDialogue = false;

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
        // brotherSounds[i].Play();
        brotherSounds[1].Play();
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
               currentLineIndex == 8 || currentLineIndex == 10 || currentLineIndex == 12 || currentLineIndex == 14)
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
        GameManager.Instance.money -= 24.99f;
        moneySound.Play();
        GameManager.Instance.talking = false;
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
            if (GameManager.Instance.day == 1)
            {
                lieSound.Play();
                //dialogueText.text = "Yes my dear, fantastic, brilliant, first class.";

                
                sliderSound.Stop();
                if (GameManager.Instance.day == 1)
                    currentLineIndex++;
            }
            else if (GameManager.Instance.day >= 2)
            {
                lieSound.Play();
                dialogueText.text = dialogueLinesAfterDay2[GameManager.Instance.index2];
                
               
                sliderSound.Stop();
                yield return new WaitForSeconds(4);
                gameSavedText.gameObject.SetActive(false);
                GameManager.Instance.talking = false;
                GameManager.Instance.paidDoris = true;
                GameManager.Instance.money -= 24.99f;
                moneySound.Play();
                TurnOffDialogue2();
                GameManager.Instance.SaveGame();
                gameSavedText.SetActive(true);
            }
        }
        else
        {
            if (GameManager.Instance.day == 1)
            {
                canContinue = false;
                rockMusic.Stop();

                dialogueText.text = "..BAD. I GOT FIRED FOR eating a stapler.";
                sliderSound.Stop();
                GameManager.Instance.fartSound.Play();
                yield return new WaitForSeconds(3f); // Wait before showing the game over screen
                ShowGameOverScreen(); // Call the game over logic
            }
            else if (GameManager.Instance.day >= 2)
            {

                dialogueText.text = "..I LOST MY JOB AND I NOW JUST SCOFF ALL DAY";
                rockMusic.Stop();
                music.Stop();
                sliderSound.Stop();
                GameManager.Instance.fartSound.Play();
                yield return new WaitForSeconds(3f);
                ShowGameOverScreenAfterDay1();
            }
        }
        if (GameManager.Instance.day == 1)
            // Move to the next dialogue line
            DisplayLine(); // Display the next line
    }
    void TurnOffDialogue2()
    {
        dialogueUI.SetActive(false);
       GameManager.Instance.index2++;
    }
    private void ShowGameOverScreen()
    {
        GameManager.Instance.houseVisits = 0;
   
        // Implement your game over logic here, e.g., showing a game over UI
        Debug.Log("Game Over!");
        gameOverScreen.SetActive(true);
    }
    private void ShowGameOverScreenAfterDay1()
    {
    //    GameManager.Instance.houseVisits = 0;

        // Implement your game over logic here, e.g., showing a game over UI
        Debug.Log("Game Over!");
        gameOverScreenAfterDay1.SetActive(true);
    }
    public void RetryAfterDay1()
    {
        music.Play();
        dialogueText.text = "YOU: HOW WAS WORK? IT WAS.......";
        gameOverScreenAfterDay1.SetActive(false);
        StartMiniGame();
    }


    private void ShowDialogueUI(bool show)
    {
        dialogueUI.SetActive(show);
    }
}