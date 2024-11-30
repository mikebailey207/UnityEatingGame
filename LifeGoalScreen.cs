using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LifeGoalScreen : MonoBehaviour
{
    public GameObject lifeGoalScreen;
    int i = 0;
    public TextMeshProUGUI lifeGoalScreenText;

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.Instance.day ==2 && GameManager.Instance.dailyTasksCompleted)
        {
            lifeGoalScreen.SetActive(true);
            GameManager.Instance.talking = true;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && lifeGoalScreen.activeSelf)
        {
            
            lifeGoalScreenText.text = "In scotland.";
            Invoke("TurnOff", 2.5f);
        }   
        
    }
    void TurnOff()
    {
        lifeGoalScreen.SetActive(false);
        GameManager.Instance.tshirtHolder.SetActive(true);
        GameManager.Instance.talking = false;
    }
}
