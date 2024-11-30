using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnGardenShop : MonoBehaviour
{
    public GameObject gardenShop;
    private void Start()
    {
        if(GameManager.Instance.day ==3 && GameManager.Instance.dailyTasksCompleted)
        {
            gardenShop.SetActive(true);
        }
        else if(GameManager.Instance.day >= 4)
        {
            gardenShop.SetActive(true);
        }
    }
}
