using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueTip : MonoBehaviour
{
    public AudioSource pickUpSound;
    private TongueController tongueController;

    void Start()
    {
        // Get reference to the TongueController on the parent
        tongueController = GetComponentInParent<TongueController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sushi"))
        {
            pickUpSound.Play();
            // Notify the TongueController of the caught sushi
            tongueController.CatchSushi(other.gameObject);
        }
    }
}
