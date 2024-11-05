using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OutsideTaskText : MonoBehaviour
{
    public TextMeshProUGUI taskText;
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.Instance.day == 1)
        {
            taskText.text = "";
        }
        if(GameManager.Instance.day >= 2)
        {
            taskText.text = GameManager.Instance.taskTexts[GameManager.Instance.taskIndex];
        }
    }

    // Update is called once per frame
    void Update()
    {
       // if (GameManager.Instance.day >= 2)
        //{
            taskText.text = GameManager.Instance.taskTexts[GameManager.Instance.taskIndex];
       // }
    }
}
