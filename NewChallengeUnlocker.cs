using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class NewChallengeUnlocker : MonoBehaviour
{

    public GameObject nachosUnlockScreen, nachosUnlockMiniScreen;
    public GameObject cornUnlockScreen;
    public GameObject pizzaUnlockScreen;
    public GameObject chiliUnlockScreen;
    public GameObject sushiUnlockScreen;

    public TextMeshProUGUI taskTextText;
    public GameObject taskText;

    private bool screenActive = false;
    private Queue<GameObject> screensToShow = new Queue<GameObject>();
    private List<string> newlyUnlockedChallenges = new List<string>(); // To store unlocked challenges

    void Start()
    {
        CheckReputationAndUnlockChallenge();
    }

    void Update()
    {
        if (screenActive && Input.GetMouseButtonDown(0))
        {
            DeactivateScreen();
            ShowNextScreen();
        }
    }

    private void CheckReputationAndUnlockChallenge()
    {
        float reputation = GameManager.Instance.reputation;

        newlyUnlockedChallenges.Clear(); // Clear the list before checking

        if (reputation >= 149.5f && !GameManager.Instance.sushiUnlockedShown)
        {
            UnlockChallenge("Sushi", sushiUnlockScreen, ref GameManager.Instance.sushiUnlockedShown, GameManager.Instance.sushiT);
        }
        if (reputation >= 119.5f && !GameManager.Instance.chiliUnlockedShown)
        {
            UnlockChallenge("Chili", chiliUnlockScreen, ref GameManager.Instance.chiliUnlockedShown, GameManager.Instance.chiliT);
        }
        if (reputation >= 89.5f && !GameManager.Instance.pizzaUnlockedShown)
        {
            UnlockChallenge("Pizza", pizzaUnlockScreen, ref GameManager.Instance.pizzaUnlockedShown, GameManager.Instance.pizzaT);
        }
        if (reputation >= 59.5f && !GameManager.Instance.cornUnlockedShown)
        {
            UnlockChallenge("Corn", cornUnlockScreen, ref GameManager.Instance.cornUnlockedShown, GameManager.Instance.cornT);
        }
        if (reputation >= 29.5f && !GameManager.Instance.nachosUnlockedShown)
        {
            UnlockChallenge("Nachos", GameManager.Instance.day == 2 ? nachosUnlockMiniScreen : nachosUnlockScreen, ref GameManager.Instance.nachosUnlockedShown, GameManager.Instance.nachosT);
        }

        // Update the task text for the next task if any challenges were unlocked
        if (newlyUnlockedChallenges.Count > 0)
        {
            UpdateTaskText();
        }

        if (!screenActive && screensToShow.Count > 0)
        {
            ShowNextScreen();
        }
    }

    private void UnlockChallenge(string challengeName, GameObject unlockScreen, ref bool unlockedFlag, GameObject challengeUI)
    {
        challengeUI.SetActive(true);
        EnqueueScreen(unlockScreen);
        unlockedFlag = true;
        newlyUnlockedChallenges.Add(challengeName);
    }

    private void UpdateTaskText()
    {
        List<string> availableChallenges = new List<string>();

        // Check all unlocked challenges that the player hasn't completed yet.
        if (GameManager.Instance.nachosUnlockedShown && !GameManager.Instance.gotNachosTshirt)
        {
            availableChallenges.Add("Nachos");
        }
        if (GameManager.Instance.cornUnlockedShown && !GameManager.Instance.gotCornTshirt)
        {
            availableChallenges.Add("Corn");
        }
        if (GameManager.Instance.pizzaUnlockedShown && !GameManager.Instance.gotPizzaTshirt)
        {
            availableChallenges.Add("Pizza");
        }
        if (GameManager.Instance.chiliUnlockedShown && !GameManager.Instance.gotChiliTshirt)
        {
            availableChallenges.Add("Chili");
        }
        if (GameManager.Instance.sushiUnlockedShown && !GameManager.Instance.gotSushiTshirt)
        {
            availableChallenges.Add("Sushi");
        }

        // Combine newly unlocked challenges with existing options
        foreach (string challenge in newlyUnlockedChallenges)
        {
            if (!availableChallenges.Contains(challenge))
            {
                availableChallenges.Add(challenge);
            }
        }

        // Create the task message based on available options
        string taskMessage;
        if (availableChallenges.Count == 1)
        {
            taskMessage = $"Go check out the {availableChallenges[0]} challenge!";
        }
        else
        {
            taskMessage = $"{availableChallenges.Count} options today: {string.Join(", ", availableChallenges)}. Up to you!";
        }

        // Update the next task text
        GameManager.Instance.taskTexts[GameManager.Instance.taskIndex + 2] = taskMessage;    
}

    private void EnqueueScreen(GameObject unlockScreen)
    {
        if (!screensToShow.Contains(unlockScreen))
        {
            screensToShow.Enqueue(unlockScreen);
        }
    }

    private void ShowNextScreen()
    {
        if (screensToShow.Count > 0)
        {
            GameObject nextScreen = screensToShow.Dequeue();
            ActivateScreen(nextScreen);
        }
    }

    private void ActivateScreen(GameObject unlockScreen)
    {
        GameManager.Instance.music.Stop();
        GameManager.Instance.talking = true;

        unlockScreen.SetActive(true);
        screenActive = true;

        taskText.SetActive(false);
    }

    private void DeactivateScreen()
    {
        nachosUnlockScreen.SetActive(false);
        cornUnlockScreen.SetActive(false);
        pizzaUnlockScreen.SetActive(false);
        chiliUnlockScreen.SetActive(false);
        sushiUnlockScreen.SetActive(false);
        nachosUnlockMiniScreen.SetActive(false);

        GameManager.Instance.music.Play();
        GameManager.Instance.talking = false;

        screenActive = false;

        if (screensToShow.Count == 0)
        {
            taskTextText.text = GameManager.Instance.taskTexts[GameManager.Instance.taskIndex];
            taskText.SetActive(true);
        }
    }
}