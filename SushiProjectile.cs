using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiProjectile : MonoBehaviour
{
    TongueController tongue;
    private Rigidbody2D rb;

    private void Start()
    {
        tongue = FindObjectOfType<TongueController>();

    }
    private void Update()
    {
        if (!tongue.gameActive) Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Mouth"))
        {
            Destroy(gameObject);
        }
    }
}
