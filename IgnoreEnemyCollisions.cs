using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreEnemyCollisions : MonoBehaviour
{
    void Start()
    {
        // Set up existing enemies to ignore collision with the boundary
        IgnoreExistingEnemies();
    }

    void IgnoreExistingEnemies()
    {
        // Find all objects in the scene with the "EnemyMovement" component
        EnemyMovement[] enemies = FindObjectsOfType<EnemyMovement>();
        Collider2D boundaryCollider = GetComponent<Collider2D>();

        foreach (EnemyMovement enemy in enemies)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider != null && boundaryCollider != null)
            {
                Physics2D.IgnoreCollision(boundaryCollider, enemyCollider);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object colliding has an EnemyMovement component
        EnemyMovement enemy = collision.gameObject.GetComponent<EnemyMovement>();
        if (enemy != null)
        {
            // Ignore collision between the boundary and the new enemy
            Collider2D enemyCollider = collision.collider;
            Collider2D boundaryCollider = GetComponent<Collider2D>();

            if (enemyCollider != null && boundaryCollider != null)
            {
                Physics2D.IgnoreCollision(boundaryCollider, enemyCollider);
            }
        }
    }
}