using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Countdown : MonoBehaviour
{
    bool countingDown = true;
    public float countdown = 60;
    public AudioSource music;
    public int hotdogs = 0;
    public GameObject winScreen;
    public TextMeshProUGUI countdownText, hotdogsText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (countingDown)
        {
            countdown -= Time.deltaTime;
        }

        if(countdown <= 0)
        {
            LoadOutsideHouse();
        }
        countdownText.text = countdown.ToString("0");
        hotdogsText.text = "Hotdogs: " + hotdogs;
        if (hotdogs >= 10) ShowWinScreen();
    }
    void ShowWinScreen()
    {
        countingDown = false;
        music.Stop();
        winScreen.SetActive(true);
        Invoke("LoadOutsidePub", 3);
      //  Time.timeScale = 0;
       
    }
    void LoadOutsidePub()
    {
        //       Time.timeScale = 1;
        GameManager.Instance.dailyTasksCompleted = true;
        GameManager.Instance.taskIndex++;
        SceneManager.LoadScene("OutsidePub");
    }
    void LoadOutsideHouse()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("ExteriorHouse");
    }
}
