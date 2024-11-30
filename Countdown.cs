using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Countdown : MonoBehaviour
{
    public bool gameOn = true;
    public TextMeshProUGUI winText;
    bool canShowWinScreen = true;
    public bool countingDown = false;
    public float countdown = 60;
    public AudioSource music;
    public int foodsEaten = 0;
    public GameObject winScreen;
    public TextMeshProUGUI countdownText, foodEatenText;
    public int foodsGoalAmount = 10;
    public float prizeMoney;

    private void Start()
    {
        if(GameManager.Instance.isFinale)
        {
            countdownText.gameObject.SetActive(false);
           
        }
    }

    void Update()
    {
        if (countingDown && !GameManager.Instance.isFinale)
        {
            countdown -= Time.deltaTime;
        }

        if(countdown <= 0 && !GameManager.Instance.isFinale)
        {
            countingDown = false;
            LoadCurrentScene();
        }

        countdownText.text = countdown.ToString("0");
        if (SceneManager.GetActiveScene().name == "FoodChallenge")
        {
            if (!GameManager.Instance.isFinale)
            {
                if (!GameManager.Instance.vegan)
                {
                    foodEatenText.text = "Hotdogs: " + foodsEaten + "/" + GameManager.Instance.hotdogsTarget;
                }
                else if (GameManager.Instance.vegan)
                {
                    foodEatenText.text = "Vegan hotdogs: " + foodsEaten + "/" + GameManager.Instance.hotdogsTarget;
                }
                if (foodsEaten >= GameManager.Instance.hotdogsTarget && canShowWinScreen) ShowWinScreen();
            }
            else
            {
                foodEatenText.text = "Hotdogs: " + foodsEaten + "/" + GameManager.Instance.hotdogsFinaleTarget;
                if (foodsEaten >= GameManager.Instance.hotdogsFinaleTarget) SceneManager.LoadScene("NachosChallenge");
            }
        }
        if (SceneManager.GetActiveScene().name == "TypingGame")
        {
            if (!GameManager.Instance.isFinale)
            {
                if (!GameManager.Instance.vegan)
                {
                    foodEatenText.text = "Slices: " + foodsEaten + "/" + GameManager.Instance.pizzaTarget;
                }
                else
                {
                    foodEatenText.text = "Vegan slices: " + foodsEaten + "/" + GameManager.Instance.hotdogsTarget;
                }
                if (foodsEaten >= GameManager.Instance.pizzaTarget && canShowWinScreen) ShowWinScreen();
            }
            else
            {
                foodEatenText.text = "Slices: " + foodsEaten + "/" + GameManager.Instance.pizzaFinaleTarget;
                if (foodsEaten >= GameManager.Instance.pizzaFinaleTarget) SceneManager.LoadScene("SushiChallenge");
            }
        }
        if (SceneManager.GetActiveScene().name == "NachosChallenge")
        {
            if (!GameManager.Instance.isFinale)
            {
                foodEatenText.text = "Nachos: " + foodsEaten + "/" + GameManager.Instance.nachosTarget;
                if (foodsEaten >= GameManager.Instance.nachosTarget && canShowWinScreen) ShowWinScreen();
            }
            else
            {
                foodEatenText.text = "Nachos: " + foodsEaten + "/" + GameManager.Instance.nachosFinaleTarget;
                if (foodsEaten >= GameManager.Instance.nachosFinaleTarget) SceneManager.LoadScene("CornChallenge");
            }
        }
        if (SceneManager.GetActiveScene().name == "CornChallenge")
        {
            if (!GameManager.Instance.isFinale)
            {
                foodEatenText.text = "Corns cleaned: " + foodsEaten + "/" + GameManager.Instance.cornTarget;
                if (foodsEaten >= GameManager.Instance.cornTarget && canShowWinScreen) ShowWinScreen();
            }
            else
            {
                foodEatenText.text = "Corns cleaned: " + foodsEaten + "/" + GameManager.Instance.cornFinaleTarget;
                if (foodsEaten >= GameManager.Instance.cornFinaleTarget) SceneManager.LoadScene("TypingGame");
            }
        }


    }
    void ShowWinScreen()
    {
        gameOn = false;
        if (SceneManager.GetActiveScene().name == "FoodChallenge")
        {
            GameManager.Instance.money += GameManager.Instance.hotDogPrizeMoney;
            winText.text = "You ate " + GameManager.Instance.hotdogsTarget +
                " hotdogs with " + countdown.ToString("0") + " seconds to spare! " +
                "You have won £" + GameManager.Instance.hotDogPrizeMoney.ToString("0") + " and you gained " + countdown.ToString("0")
                + " Myspace followers!";
          
        }
        if (SceneManager.GetActiveScene().name == "NachosChallenge")
        {
            GameManager.Instance.money += GameManager.Instance.nachosPrizeMoney;
            winText.text = "You ate " + GameManager.Instance.nachosTarget +
               " nachos with " + countdown.ToString("0") + " seconds to spare! " +
               "You have won £" + GameManager.Instance.nachosPrizeMoney.ToString("0") + " and you gained " + countdown.ToString("0")
                + " Myspace followers!";
        }
        if (SceneManager.GetActiveScene().name == "TypingGame")
        {
            GameManager.Instance.money += GameManager.Instance.pizzaPrizeMoney;
            winText.text = "You ate " + GameManager.Instance.pizzaTarget +
               " slices with " + countdown.ToString("0") + " seconds to spare! " +
               "You have won £" + GameManager.Instance.pizzaPrizeMoney.ToString("0") + " and you gained " + countdown.ToString("0")
                + " Myspace followers!";
        }
        if (SceneManager.GetActiveScene().name == "CornChallenge")
        {
            GameManager.Instance.money += GameManager.Instance.cornPrizeMoney;
            winText.text = "You cleaned " + GameManager.Instance.cornTarget +
               " corns with " + countdown.ToString("0") + " seconds to spare! " +
               "You have won £" + GameManager.Instance.cornPrizeMoney.ToString("0") + " and you gained " + countdown.ToString("0")
                + " Myspace followers!";
        }
        countingDown = false;
        music.Stop();
        winScreen.SetActive(true);
        GameManager.Instance.taskIndex++;
        canShowWinScreen = false;
      //  Invoke("LoadOutsidePub", 3);
      //  Time.timeScale = 0;
       
    }
    void ShortCutTest()
    {

    }
    public void LoadOutsidePub()
    {
        GameManager.Instance.dailyTasksCompleted = true;
        GameManager.Instance.reputation += countdown;
        if (SceneManager.GetActiveScene().name == "FoodChallenge")
        {
            GameManager.Instance.hotdogsVisitCount++;
           
           // if (GameManager.Instance.hotdogsVisitCount % 2 == 0)
           // {
                GameManager.Instance.hotdogsTarget++;
                GameManager.Instance.hotDogPrizeMoney += 50;
            if(!GameManager.Instance.gotHotdogTshirt)
            {
                GameManager.Instance.tShirts++;
                GameManager.Instance.gotHotdogTshirt = true;
            }
           // }
        }

        if (SceneManager.GetActiveScene().name == "NachosChallenge")
        {
            GameManager.Instance.nachosVisitCount++;

           // if (GameManager.Instance.nachosVisitCount % 2 == 0)
           // {
                GameManager.Instance.nachosTarget += 5;
                GameManager.Instance.nachosPrizeMoney += 50;
            // }
            if (!GameManager.Instance.gotNachosTshirt)
            {
                GameManager.Instance.tShirts++;
                GameManager.Instance.gotNachosTshirt = true;
                GameManager.Instance.tick2.SetActive(true);
            }
        }

        if (SceneManager.GetActiveScene().name == "TypingGame")
        {
            GameManager.Instance.pizzaVisitCount++;

           // if (GameManager.Instance.pizzaVisitCount % 2 == 0)
          //  {
                GameManager.Instance.pizzaTarget++;
                GameManager.Instance.pizzaPrizeMoney += 50;
            //  }
            if (!GameManager.Instance.gotPizzaTshirt)
            {
                GameManager.Instance.tShirts++;
                GameManager.Instance.gotPizzaTshirt = true;
                GameManager.Instance.tick4.SetActive(true);
            }
        }

        if (SceneManager.GetActiveScene().name == "CornChallenge")
        {
            GameManager.Instance.cornVisitCount++;

          //  if (GameManager.Instance.cornVisitCount % 2 == 0)
          //  {
                GameManager.Instance.cornTarget++;
                GameManager.Instance.cornPrizeMoney += 50;
            //  }
            if (!GameManager.Instance.gotCornTshirt)
            {
                GameManager.Instance.tShirts++;
                GameManager.Instance.tick3.SetActive(true);
                GameManager.Instance.gotCornTshirt = true;
            }
        }

        SceneManager.LoadScene("OutsidePub");
    }
    void LoadCurrentScene()
    {
        Time.timeScale = 1;
        string thisScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(thisScene);
    }
}
