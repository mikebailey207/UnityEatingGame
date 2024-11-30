using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Cinemachine;

public class EndDialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public string speaker;
        [TextArea(2, 5)] public string line;
    }
    bool credits = false;
    public GameObject endText;
    public CinemachineVirtualCamera cam1, cam3;
    public GameObject winScreen, winScreen2;
    public AudioSource music, scratchSound, cheeringSound, panicSound, henmanSound;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueBox;

    public List<DialogueLine> dialogue;
    private int currentLineIndex = 0;

    public GameObject shopkeeper;
    public GameObject dog;
    public GameObject doris;
    private bool isDialogueActive = false;
    private bool canClickToProgress = true; // Cooldown flag

    void Start()
    {
        dialogueBox.SetActive(false);
   /*     GameManager.Instance.tshirtsText.gameObject.SetActive(false);
        GameManager.Instance.repText.gameObject.SetActive(false);
        GameManager.Instance.moneyText.gameObject.SetActive(false);
        GameManager.Instance.dayText.gameObject.SetActive(false);
        GameManager.Instance.toggleProgressText.gameObject.SetActive(false);
        */

        StartCoroutine(StartScene());
    }

    IEnumerator StartScene()
    {

        yield return new WaitForSeconds(1f);
        StartDialogue();
        yield return new WaitForSeconds(2f);
        cam1.Priority = -10;
    }

    public void StartDialogue()
    {
        music.Play();
        isDialogueActive = true;
        dialogueBox.SetActive(true);
        currentLineIndex = 0;
        DisplayNextLine();
    
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canClickToProgress && isDialogueActive)
        {
            StartCoroutine(ProgressDialogueWithDelay());
        }
        if(credits)
        {
            endText.transform.Translate(0, 70 * Time.deltaTime, 0);
        }
    }

    IEnumerator ProgressDialogueWithDelay()
    {
        canClickToProgress = false; // Disable further clicks
        DisplayNextLine();         // Display the next line of dialogue
        yield return new WaitForSeconds(2f); // Wait for cooldown
        canClickToProgress = true; // Re-enable clicks
    }

    public void DisplayNextLine()
    {
        if (currentLineIndex >= dialogue.Count)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = dialogue[currentLineIndex];

        speakerText.text = line.speaker;
        dialogueText.text = line.line;

        // Trigger special actions based on line index
        TriggerActions(currentLineIndex);

        currentLineIndex++;
    }

    void TriggerActions(int index)
    {
        switch (index)
        {
            case 5:
                StartCoroutine(ShopkeeperReturnWithDoris());
                break;
            case 16:
                panicSound.Stop();
                scratchSound.Play();
               
                break;
            case 17:
                
                cheeringSound.Play();
                music.Play();
                break;

            case 22:
                DogEntrance();
                break;
            case 24:
                dog.GetComponent<AudioSource>().Play();
                break;
                
        }
    }

    IEnumerator ShopkeeperFloatAway()
    {
        shopkeeper.GetComponent<Animator>().SetTrigger("FloatAway");
        yield return new WaitForSeconds(2f);
    }

    IEnumerator ShopkeeperFOff()
    {
        doris.transform.SetParent(null);
        shopkeeper.GetComponent<Animator>().SetTrigger("FOff");
        yield return new WaitForSeconds(2f);
    }

    IEnumerator ShopkeeperReturnWithDoris()
    {
        shopkeeper.transform.position = new Vector3(-10, 0, 0);
        shopkeeper.GetComponent<Animator>().SetTrigger("ShootIn");
        yield return new WaitForSeconds(1f);
        music.Stop();
        scratchSound.Play();
        panicSound.Play();
    }

    void DogEntrance()
    {
        music.Stop();
        scratchSound.Play();
        dog.SetActive(true);
        dog.GetComponent<Animator>().SetTrigger("RunIn");
    }

    public void EndDialogue()
    {
        dog.GetComponent<AudioSource>().Stop();
        isDialogueActive = false;
        dialogueBox.SetActive(false);
        StartCoroutine(TransitionToNextScene());
        cheeringSound.Play();
    }

    IEnumerator TransitionToNextScene()
    {
        cam3.Priority = 500;
        yield return new WaitForSeconds(3);
        henmanSound.Play();
        yield return new WaitForSeconds(2f);
        music.Stop();
        winScreen.SetActive(true);
        yield return new WaitForSeconds(3f);
        winScreen2.SetActive(true);
        winScreen.SetActive(false);
        yield return new WaitForSeconds(3f);
        credits = true;
        Debug.Log("Transitioning to the plane scene...");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Intro");
    }
}