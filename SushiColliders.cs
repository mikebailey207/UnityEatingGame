using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiColliders : MonoBehaviour
{
    public TongueController tongueController;
    public AudioSource loseLifeSound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Sushi"))
        {
            loseLifeSound.Play();
            tongueController.lives--;
     
            if(GameManager.Instance.isFinale)
            {
                tongueController.sushisCaught--;
                GameManager.Instance.finalePlayerCounter--;
            }
            Destroy(collision.gameObject);
        }
    }
}
