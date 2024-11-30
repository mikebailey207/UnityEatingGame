using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
   
    public Transform target; // Dog's position
    public float moveSpeed = 2f;
    AudioSource splatSound;
    public float pushbackForce = 2f;

    // Reference to the money prefab
    GameObject moneyPrefab;

    private void Start()
    {
        GameObject particles = Resources.Load<GameObject>("BadgerWomanParticles"); // Path to your prefab inside the Resources folder
        if (particles != null)
        {
            Instantiate(particles, transform.position, Quaternion.identity);
        }
        target = FindObjectOfType<DogRandomMovement>().transform;
        splatSound = GameObject.Find("SplatSound").GetComponent<AudioSource>();
       
    }

    void Update()
    {
        if (GameManager.Instance.walkiesComplete)
        {
            GameObject particles = Resources.Load<GameObject>("BadgerWomanParticles"); // Path to your prefab inside the Resources folder
            if (particles != null)
            {
                Instantiate(particles, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }

        // Move towards the dog
        Vector2 direction = (target.position - transform.position).normalized;
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
    }

    public void TakeDamage()
    {
        DropMoney();
        Destroy(gameObject); // Destroy the enemy on being shot
    }

    private void DropMoney()
    {
      
            GameObject moneyPrefab = Resources.Load<GameObject>("Money");
            // Instantiate money at the enemy's position
            GameObject money = Instantiate(moneyPrefab, transform.position, Quaternion.identity);

            // Set a random value for the money between 0.50 and 2.00
            float moneyValue = Random.Range(3f, 20f);

            // Assuming the money prefab has a MoneyPickup script that sets its value
            money.GetComponent<MoneyPickUp>().SetValue(moneyValue);
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            // Play particles and sound
            GameObject particles = Resources.Load<GameObject>("BadgerWomanParticles"); // Path to your prefab inside the Resources folder
            if (particles != null)
            {
                Instantiate(particles, transform.position, Quaternion.identity);
            }
            splatSound.Play();
            TakeDamage();

            // Destroy the bullet after collision
            Destroy(collision.gameObject);
        }
    }
}