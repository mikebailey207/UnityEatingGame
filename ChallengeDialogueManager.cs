using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class DayDialogue
{
    public string[] sentences; // Array of sentences for this visit
}

public class ChallengeDialogueManager : MonoBehaviour
{
   
    public GameObject pizzaObject;
    public Animator staminaAnim, fullnessAnim, burpAnim;

    
    public GameObject firstCorn;
   // public bool wantGameScriptOn = false;
    public GameObject restaurantOwner;

    bool canStartDialogue = false;
    public AudioSource rockMusic, challengeMusic;
    public GameObject nameScreen;

    public GameObject mainGameCanvas;
    public GameObject instructionsNextButton;

    [Header("Dialogue Settings")]
    public TextMeshProUGUI dialogueText; // TMP Text for dialogue display
    public List<DayDialogue> visitSpecificDialogues; // List of dialogues for each visit
    private int currentVisitCount; // Tracks the current visit count
    private int currentDialogueIndex = 0; // Tracks the current sentence in the dialogue

    [Header("Instruction Settings")]
    public GameObject instructionsUI; // UI containing instructions and buttons
    public TextMeshProUGUI instructionsTextText; // TMP Text for instructions
    public string[] instructions; // Array of instruction steps
    private int currentInstructionIndex = 0; // Tracks the current instruction

    [Header("Challenge Logic")]
    public MonoBehaviour challengeScript; // Specific eating challenge script
    private bool challengeStarted = false; // Tracks if the challenge has started
    private bool showingDialogue = true; // Tracks if dialogue is currently being shown

    [Header("GameManager Variables")]
    public Countdown countdown; // Countdown timer for challenge start
    public string challengeType; // Used to determine the visit count variable (e.g., "hotdogs", "nachos", etc.)

    void Start()
    {
        if (!GameManager.Instance.isFinale)
        {
            // Initialize UI
            instructionsUI.SetActive(false);

            if (challengeScript != null)
                challengeScript.enabled = false;

            // Get the current visit count based on the challenge type
            currentVisitCount = GetVisitCountForChallenge();
            GameManager.Instance.talking = true;
            // Handle name screen logic
            if (currentVisitCount == 0)
            {
                Invoke("TurnOffNameScreen", 3);
            }
            else
            {
                TurnOffNameScreen();
            }
            // Check if there are dialogues for the current visit
            if (currentVisitCount < visitSpecificDialogues.Count && visitSpecificDialogues[currentVisitCount].sentences.Length > 0)
            {
                // Start dialogue for the current visit
                dialogueText.text = visitSpecificDialogues[currentVisitCount].sentences[currentDialogueIndex];
            }
            else
            {
                // No dialogue for this visit, skip to instructions
                OnDialogueFinished();

                // Instructions should still appear even if there's no dialogue
                instructionsUI.SetActive(true);
            }
        }
        else
        {
            challengeScript.enabled = true;
            mainGameCanvas.SetActive(true);
            instructionsUI.SetActive(false);
            dialogueText.gameObject.SetActive(false);
            nameScreen.SetActive(false);
            if(firstCorn != null)
            {
                firstCorn.SetActive(true);
            }
            if(GameManager.Instance.currentChallengeType == "chilli")
            {
                restaurantOwner.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    void Update()
    {
        if (!GameManager.Instance.isFinale)
        {

            // Progress dialogue on mouse click if currently showing dialogue
            if (showingDialogue && Input.GetMouseButtonDown(0) && canStartDialogue)
            {
                ShowNextDialogueSentence();
            }
        }
        AnimateInstructions();
    }
    void TurnOffNameScreen()
    {
        nameScreen.SetActive(false);
        rockMusic.Play();
        canStartDialogue = true;
        restaurantOwner.SetActive(true); 

    }

    public void ShowNextDialogueSentence()
    {
        // Check if we are at the last sentence
        if (currentVisitCount >= visitSpecificDialogues.Count || currentDialogueIndex >= visitSpecificDialogues[currentVisitCount].sentences.Length - 1)
        {
            // End of dialogue for this visit, transition to instructions
            OnDialogueFinished();
        }
        else
        {
            // Show the next sentence in the dialogue
            currentDialogueIndex++;
            dialogueText.text = visitSpecificDialogues[currentVisitCount].sentences[currentDialogueIndex];
        }
    }

    public void OnDialogueFinished()
    {
        dialogueText.gameObject.SetActive(false);
        mainGameCanvas.gameObject.SetActive(true);
        // Disable dialogue display
        showingDialogue = false;

        // Enable instructions UI
        instructionsUI.SetActive(true);

        // Reset instructions to the first entry
        currentInstructionIndex = 0;
        if (instructions.Length > 0)
        {
            instructionsTextText.text = instructions[currentInstructionIndex];
        }
        else
        {
            instructionsTextText.text = "No instructions available.";
        }
    }

    void AnimateInstructions()
    {
        if(GameManager.Instance.currentChallengeType == "hotdogs")
        {
         if(currentInstructionIndex == 0 && !challengeStarted)
            {
                staminaAnim.SetBool("Showing", true);
            }
         if(currentInstructionIndex == 1)
            {
                staminaAnim.SetBool("Showing", false);
                fullnessAnim.SetBool("Showing", true);
            }
         if(currentInstructionIndex == 2)
            {
                fullnessAnim.SetBool("Showing", false);
                burpAnim.SetBool("Showing", true);
            }
         if(currentInstructionIndex >= 3)
            {
                burpAnim.SetBool("Showing", false);
            }
                      }
        
    }
    public void ShowNextInstruction()
    {
        Debug.Log("IS THIS BUTTON WORKING (INSTR)?");
        // Progress to the next instruction
        if (currentInstructionIndex < instructions.Length - 1)
        {
            currentInstructionIndex++;
            instructionsTextText.text = instructions[currentInstructionIndex];
        }
        else
        {
            instructionsNextButton.SetActive(false);
            Debug.Log("End of instructions.");
        }
    }
    public void StartChallengeVegan()
    {
      
       
     
        GameManager.Instance.vegan = true;
        Debug.Log("IS THE START CHALLENGE BUTTON WORKING??!");
        GameManager.Instance.talking = false;

        if (countdown != null) countdown.countingDown = true;
        rockMusic.Stop();
        challengeMusic.Play();

        // Start the challenge and hide instructions
        challengeStarted = true;

        if (challengeScript != null)
            challengeScript.enabled = true;

        instructionsUI.SetActive(false);
        if (firstCorn != null)
        {
            firstCorn.SetActive(true);
        }
        if (pizzaObject != null)
        {
            pizzaObject.SetActive(true);
        }
        if (staminaAnim != null)
        {
            staminaAnim.SetBool("Showing", false);
            fullnessAnim.SetBool("Showing", false);
            burpAnim.SetBool("Showing", false);
        }
      
        HandleOwnerVanish();
    }
    
    public void StartChallenge()
    {
     
       
        GameManager.Instance.vegan = false;
        Debug.Log("IS THE START CHALLENGE BUTTON WORKING??!");
        GameManager.Instance.talking = false;
       
       if(countdown != null) countdown.countingDown = true;
        rockMusic.Stop();
        challengeMusic.Play();

        // Start the challenge and hide instructions
        challengeStarted = true;

        if (challengeScript != null)
            challengeScript.enabled = true;

        instructionsUI.SetActive(false);
        if(firstCorn != null)
        {
            firstCorn.SetActive(true);
        }
        if(pizzaObject != null)
        {
            pizzaObject.SetActive(true);
        }
        if (staminaAnim != null)
        {
            staminaAnim.SetBool("Showing", false);
            fullnessAnim.SetBool("Showing", false);
            burpAnim.SetBool("Showing", false);
        }
      
        HandleOwnerVanish();
    }
    void HandleOwnerVanish()
    {
        if(GameManager.Instance.currentChallengeType == "corn")
        {
            restaurantOwner.AddComponent<Rigidbody2D>();

        }
        if(GameManager.Instance.currentChallengeType == "chilli")
        {
            restaurantOwner.AddComponent<Rigidbody2D>();
        
        }
    }

    public bool IsChallengeStarted()
    {
        return challengeStarted;
    }

    private int GetVisitCountForChallenge()
    {
        // Retrieve the visit count from GameManager based on the challenge type
        switch (GameManager.Instance.currentChallengeType.ToLower())
        {
            case "hotdogs":
                return GameManager.Instance.hotdogsVisitCount;
            case "nachos":
                return GameManager.Instance.nachosVisitCount;
            case "pizza":
                return GameManager.Instance.pizzaVisitCount;
            case "corn":
                return GameManager.Instance.cornVisitCount;
            case "sushi":
                return GameManager.Instance.sushiVisitsCount;
            case "chilli":
                return GameManager.Instance.chilliVisitsCount;
            default:
                Debug.LogWarning("Invalid challenge type: " + challengeType);
                return 0;
        }
    }
}