using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NachoController : MonoBehaviour
{
private bool holdingNacho = false;
    private Rigidbody2D rb;
    private bool isFollowingMouse = false;
    AudioSource pickUpSound;
    private void Start()
    {
        GameManager.Instance.holdingNacho = false;
        rb = GetComponent<Rigidbody2D>();
        pickUpSound = GameObject.Find("PickUpSound").GetComponent<AudioSource>();
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
    }

    private void Update()
    {
       // holdingNacho = GameManager.Instance.holdingNacho;
        if (isFollowingMouse)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z; // Keep the z-position unchanged
            transform.position = mousePosition;
        }

        // Detect mouse click on this object
        if (Input.GetMouseButtonDown(0) && !GameManager.Instance.holdingNacho && !GameManager.Instance.talking)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] hits = Physics2D.OverlapPointAll(mousePos);

            foreach (Collider2D hit in hits)
            {
                NachoController nacho = hit.GetComponent<NachoController>();
                if (nacho != null && !GameManager.Instance.holdingNacho)
                {
                    nacho.PickUpNacho();
                    break;
                }
            }
        }
    }
    private void PickUpNacho()
    {
        GameManager.Instance.holdingNacho = true;
        holdingNacho = true;  // Mark this nacho as picked up
        GetComponent<Collider2D>().enabled = false;  // Disable collider to prevent picking up again
        StartFollowingMouse();
        pickUpSound.Play();
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }
    private void StartFollowingMouse()
    {
        isFollowingMouse = true;
        rb.isKinematic = true; // Stop physics interactions
        GetComponent<Collider2D>().isTrigger = true; // Make it non-physical
    }

    public void DropNacho()
    {
        // Call this when you want to drop the nacho and make it interact with physics again
        isFollowingMouse = false;
        rb.isKinematic = false; // Re-enable physics
        GetComponent<Collider2D>().isTrigger = false; // Make it solid again
    }

    
}