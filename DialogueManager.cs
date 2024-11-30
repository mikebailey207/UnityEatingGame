using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class DialogueManager : MonoBehaviour
{
    bool canPressS = true;
    bool canPressA = true;
    bool canPressW = true;
    bool canPressD = true;

    public AudioSource explodeSound;
    bool canExplodeLetters = false;
    public GameObject letterA, letterW, letterS, letterD;
    public CinemachineVirtualCamera cam1, cam2;//, cam3;  

    public GameObject dialogueUI;       // UI panel or box for the dialogue
    public TextMeshProUGUI dialogueText;           // Text component to display the dialogue

    [TextArea(3, 10)]
    public string[] dialogueLines;      // Array to hold dialogue lines, editable in the Inspector

    private int currentLineIndex = 0;   // Track the current line in the dialogue
    private bool isDialogueActive = false;


    private void Start()
    {
        StartCoroutine(GoThroughCameras());
        Invoke("StartDialogue", 5);
    }
    
    IEnumerator GoThroughCameras()
    {
        cam1.Priority = 20;
        yield return new WaitForSeconds(2);
        cam1.Priority = 0; 
    }
    public void StartDialogue()
    {
        GameManager.Instance.talking = true;
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

    private void DisplayLine()
    {
        if (currentLineIndex < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLineIndex];
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        GameManager.Instance.talking = false;
        ShowDialogueUI(false);
        isDialogueActive = false;
        cam1.Priority = 50;
        canExplodeLetters = true;
        letterA.SetActive(true);
        letterD.SetActive(true);
        letterS.SetActive(true);
        letterW.SetActive(true);
    }

    private void Update()
    {
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            currentLineIndex++;
            DisplayLine();
        }
        if(canExplodeLetters)
        {
            if(Input.GetKeyDown(KeyCode.W) && canPressW)
            {
                GameObject particles = Resources.Load<GameObject>("BadgerWomanParticles"); // Path to your prefab inside the Resources folder
             
               
                if (particles != null)
                {
                    Instantiate(particles, letterW.transform.position, Quaternion.identity);
                    explodeSound.Play();
                    letterW.SetActive(false);
                    canPressW = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.S) && canPressS)
            {
                GameObject particles = Resources.Load<GameObject>("BadgerWomanParticles"); // Path to your prefab inside the Resources folder


                if (particles != null)
                {
                    Instantiate(particles, letterS.transform.position, Quaternion.identity);
                    explodeSound.Play();
                    letterS.SetActive(false);
                    canPressS = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.A) && canPressA)
            {
                GameObject particles = Resources.Load<GameObject>("BadgerWomanParticles"); // Path to your prefab inside the Resources folder


                if (particles != null)
                {
                    Instantiate(particles, letterA.transform.position, Quaternion.identity);
                    explodeSound.Play();
                    letterA.SetActive(false);
                    canPressA = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.D) && canPressD)
            {
                GameObject particles = Resources.Load<GameObject>("BadgerWomanParticles"); // Path to your prefab inside the Resources folder


                if (particles != null)
                {
                    Instantiate(particles, letterD.transform.position, Quaternion.identity);
                    explodeSound.Play();
                    letterD.SetActive(false);
                    canPressD = false;
                }
            }
        }
    }

    private void ShowDialogueUI(bool show)
    {
        dialogueUI.SetActive(show);
    }
}