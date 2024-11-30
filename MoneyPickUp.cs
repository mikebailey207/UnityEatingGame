using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPickUp : MonoBehaviour
{
    private float value;
    AudioSource moneySound;

    private void Start()
    {

        Invoke("Kill", 10);
    }

    void Kill()
    {
        Destroy(gameObject);
    }
    public void SetValue(float moneyValue)
    {
        value = moneyValue;
        moneySound = GameObject.Find("MoneySound").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            moneySound.Play();
            // Add money value to player's total money in GameManager
            GameManager.Instance.money += value;
            GameManager.Instance.moneyCollectedThisSession += value;
            // Destroy the money pickup after it's collected
            Destroy(gameObject);
        }
    }
}
