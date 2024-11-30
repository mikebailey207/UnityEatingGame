using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BardHandler : MonoBehaviour

{
    public TextMeshProUGUI taskTextText;
    public GameObject bard;
    public GameObject taskPopup;
    private void Start()
    {
        taskTextText.text = GameManager.Instance.taskTexts[GameManager.Instance.taskIndex];
        if (GameManager.Instance.tShirts >=6 && GameManager.Instance.canShowBardIntro)
        {
            GameManager.Instance.tshirtsText.gameObject.SetActive(false);
            taskPopup.SetActive(false);
            bard.SetActive(true);
            GameManager.Instance.talking = true;
            GameManager.Instance.bardMyspaceTarget = GameManager.Instance.reputation + 100;
            GameManager.Instance.canShowBardIntro = false;
        }
      
        if (GameManager.Instance.reputation >= GameManager.Instance.bardMyspaceTarget)
        {
            GameManager.Instance.talking = true;
            GameManager.Instance.tshirtsText.gameObject.SetActive(false);
            GameManager.Instance.finaleBardCounterText.gameObject.SetActive(false);
            GameManager.Instance.finalePlayerCounterText.gameObject.SetActive(false);
            GameManager.Instance.finaleSliderBard.gameObject.SetActive(false);
            GameManager.Instance.finaleSlider.gameObject.SetActive(false);
            taskPopup.SetActive(false);
            bard.SetActive(true);
            Debug.Log("ACTIVATING DUE TO HIGH REP");
        }

    }
}
