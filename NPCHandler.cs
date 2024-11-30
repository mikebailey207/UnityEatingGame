using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHandler : MonoBehaviour
{
  
    public GameObject shopkeeperIntro;

    private void Start()
    {
        if(GameManager.Instance.day == 3 && GameManager.Instance.canShowIntroShopkeeper)
        {
            shopkeeperIntro.SetActive(true);
            GameManager.Instance.canShowIntroShopkeeper = false;
        }
    }

}
