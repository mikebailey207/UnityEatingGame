using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed of rotation

    void Update()
    {
        // Rotate the circle
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    public void OnPlayerJump()
    {
        // Logic for when a player jumps from this circle
        // For example, you might want to play a sound or some visual effect
    }
}