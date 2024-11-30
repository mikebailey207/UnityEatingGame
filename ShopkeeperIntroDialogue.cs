using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class ShopkeeperIntroDialogue : MonoBehaviour
{
    public GameObject easyButton;
    public TextMeshProUGUI taskTextText;
    public GameObject taskPopup;
    bool finishedBardLastChat = false;
    public GameObject choiceScreen;
    public GameObject taskText;
    private GameObject moneyText;
    AudioSource music;
    public CinemachineVirtualCamera closeUpCam;
    public TMP_Text dialogueText; // Reference to TextMeshPro text UI element
    public string[] dialogueLines, dialogueLines2; // Array of dialogue lines for the NPC
    private int currentLineIndex = 0;
    private bool isDialogueActive;
    bool isDialogue2Active;
    bool headingToShop = false;

    public Transform shop;

    private void Awake()
    {
        if (GameManager.Instance.reputation < GameManager.Instance.bardMyspaceTarget)
        {
            isDialogueActive = true;
            isDialogue2Active = false;
        }
        else
        {
            isDialogue2Active = true;
            isDialogueActive = false;
        }
  
    }
    void Start()
    {
        if(GameManager.Instance.bardDeaths >=1)
        {
            if (easyButton != null)
            {
                easyButton.SetActive(true);
            }
        }
        closeUpCam.Priority = 1000;

        moneyText = GameObject.Find("MoneyText");
        music = GameObject.Find("Music").GetComponent<AudioSource>();
        music.Stop();
        moneyText.SetActive(false);
        taskText.SetActive(false);
       
        if (GameManager.Instance.reputation < GameManager.Instance.bardMyspaceTarget)
        {
            DisplayNextLine(); // Display the first line of dialogue
        }
        else if (GameManager.Instance.reputation >= GameManager.Instance.bardMyspaceTarget)
        {
            DisplayNextLine2();
        }
    }

    void Update()
    {
        if (isDialogueActive && GameManager.Instance.reputation < GameManager.Instance.bardMyspaceTarget && Input.GetMouseButtonDown(0))
        {
            DisplayNextLine();
        }
        // Check if Dialogue 2 should run
        else if (isDialogue2Active && GameManager.Instance.reputation >= GameManager.Instance.bardMyspaceTarget && Input.GetMouseButtonDown(0))
        {
            DisplayNextLine2();
        }
        if(headingToShop)
        {
            transform.position = Vector2.MoveTowards(transform.position, shop.transform.position, 20 * Time.deltaTime);
        }
    }

    void DisplayNextLine()
    {
        
        GameManager.Instance.talking = true;
        dialogueText.gameObject.SetActive(true);
        if (currentLineIndex < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLineIndex];
            currentLineIndex++;
        }
        else
        {
            EndDialogue();
        }
    }
    void DisplayNextLine2()
    {

        GameManager.Instance.talking = true;
        dialogueText.gameObject.SetActive(true);
        if (currentLineIndex < dialogueLines2.Length)
        {
            dialogueText.text = dialogueLines2[currentLineIndex];
            currentLineIndex++;
        }
        else
        {

            choiceScreen.SetActive(true);
        }
    }

    void EndDialogue()
    {
        GetComponent<AudioSource>().Stop();
        music.Play();
        closeUpCam.Priority = -1000;
        GameManager.Instance.talking = false;
        dialogueText.text = ""; // Clear the dialogue
        moneyText.SetActive(true);
        taskText.SetActive(true);
        isDialogueActive = false;
        dialogueText.gameObject.SetActive(false);
        headingToShop = true;
        if (taskPopup != null)
        {
            taskTextText.text = GameManager.Instance.taskTexts[GameManager.Instance.taskIndex] + "!";
            taskPopup.SetActive(true);
        }
      
    }
    public void KeepPlaying()
    {
        GameManager.Instance.moneyText.gameObject.SetActive(true);
        taskText.SetActive(true);
       
        music.Play();
        GetComponent<AudioSource>().Stop();
        closeUpCam.Priority = -1000;
        dialogueText.text = "";
        dialogueText.gameObject.SetActive(false);
        GameManager.Instance.talking = false;
        choiceScreen.SetActive(false);
        finishedBardLastChat = true;
        if (taskPopup != null) taskPopup.SetActive(true);
    }
    public void StartFinale()
    {
        GameManager.Instance.tshirtsText.gameObject.SetActive(false);
        finishedBardLastChat = true;
        music.Play();
        GameManager.Instance.talking = false;
        GameManager.Instance.isFinale = true;
        GameManager.Instance.isFinale = true;
        GameManager.Instance.bardSpeedModifier = 1.5f;
        // BE GOOD TO HAVE SOME KIND OF OTT ESPN LIKE INTRO CUT SCENE HERE, THAT JUST PLAYS OUT AND MOVES TO THE CHALLENGE AT THE END
        GameManager.Instance.finaleBardCounterText.gameObject.SetActive(true);
        GameManager.Instance.finalePlayerCounterText.gameObject.SetActive(true);
        GameManager.Instance.finaleSliderBard.gameObject.SetActive(true);
        GameManager.Instance.finaleSlider.gameObject.SetActive(true);
        SceneManager.LoadScene("FoodChallenge");
   
    }
    public void StartFinaleEasy()
    {
        finishedBardLastChat = true;
        music.Play();
        GameManager.Instance.talking = false;
        GameManager.Instance.isFinale = true;
        GameManager.Instance.bardSpeedModifier = 1;
        // BE GOOD TO HAVE SOME KIND OF OTT ESPN LIKE INTRO CUT SCENE HERE, THAT JUST PLAYS OUT AND MOVES TO THE CHALLENGE AT THE END
        GameManager.Instance.finaleBardCounterText.gameObject.SetActive(true);
        GameManager.Instance.finalePlayerCounterText.gameObject.SetActive(true);
        GameManager.Instance.finaleSliderBard.gameObject.SetActive(true);
        GameManager.Instance.finaleSlider.gameObject.SetActive(true);
        SceneManager.LoadScene("FoodChallenge");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && finishedBardLastChat)
        {
            choiceScreen.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && finishedBardLastChat)
        {
            choiceScreen.SetActive(false);
        }
    }
}
