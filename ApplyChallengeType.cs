using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplyChallengeType : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if(SceneManager.GetActiveScene().name == "FoodChallenge")
        {
            GameManager.Instance.currentChallengeType = "hotdogs";
        }
        if (SceneManager.GetActiveScene().name == "NachosChallenge")
        {
            GameManager.Instance.currentChallengeType = "nachos";
        }
        if (SceneManager.GetActiveScene().name == "TypingGame")
        {
            GameManager.Instance.currentChallengeType = "pizza";
        }
        if (SceneManager.GetActiveScene().name == "ChiliChallenge")
        {
            GameManager.Instance.currentChallengeType = "chilli";
        }
        if (SceneManager.GetActiveScene().name == "CornChallenge")
        {
            GameManager.Instance.currentChallengeType = "corn";
        }
        if (SceneManager.GetActiveScene().name == "SushiChallenge")
        {
            GameManager.Instance.currentChallengeType = "sushi";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
