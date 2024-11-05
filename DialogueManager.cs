using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogueManager : MonoBehaviour
{
  

    public GameObject dialogueUI;       // UI panel or box for the dialogue
    public TextMeshProUGUI dialogueText;           // Text component to display the dialogue

    [TextArea(3, 10)]
    public string[] dialogueLines;      // Array to hold dialogue lines, editable in the Inspector

    private int currentLineIndex = 0;   // Track the current line in the dialogue
    private bool isDialogueActive = false;


    private void Start()
    {
        Invoke("StartDialogue", 5);
    }
    public void StartDialogue()
    {
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
        ShowDialogueUI(false);
        isDialogueActive = false;
    }

    private void Update()
    {
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            currentLineIndex++;
            DisplayLine();
        }
    }

    private void ShowDialogueUI(bool show)
    {
        dialogueUI.SetActive(show);
    }
}