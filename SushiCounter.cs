using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SushiCounter : MonoBehaviour
{
    //public GameObject loseScreen;
    public int lives = 3;
    public TextMeshProUGUI countText, highScoreText, livesText;

   // int dailyBest = 0;
    float count;
    float highScore;
    // Start is called before the first frame update
    void Start()
    {
        highScore = PlayerPrefs.GetFloat("HighScore");
    }

    // Update is called once per frame
    void Update()
    {
        if (lives <= 0) GameOver();
        count += Time.deltaTime;
        livesText.text = "Lives: " + lives;
        countText.text = "Score: " + count.ToString("0") + " seconds";
        highScoreText.text = "High: " + highScore.ToString("0") + "seconds";
        if(count>=highScore)
        {
            highScore = count;
            PlayerPrefs.SetFloat("HighScore", count);
        }
        
    }
    public void GameOver()
    {
        SceneManager.LoadScene("SushiChallenge");
    }
}
