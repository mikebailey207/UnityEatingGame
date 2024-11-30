using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlacer : MonoBehaviour
{
    public GameObject player;
    public Transform shopSpawnPoint, houseSpawnPoint;

    private void Start()
    {
     if(GameManager.Instance.beenShopping)
        {
            player.transform.position = shopSpawnPoint.position;
        }
     else if(!GameManager.Instance.beenShopping)
        {
            player.transform.position = houseSpawnPoint.position;
        }
    }
}
