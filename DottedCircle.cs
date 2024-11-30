using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DottedCircle : MonoBehaviour
{
    public DogRandomMovement dog;
    public Transform player;        // Reference to the player object
    public float radius = 10f;      // Radius of the circle (maximum lead length)
    public int segments = 50;       // Total points on the circumference
    public float dotSpacing = 0.1f; // Spacing between each dot

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.loop = true;
        lineRenderer.widthMultiplier = 0.05f;  // Line thickness
        UpdateCircle();
    }

    void Update()
    {
       // if (dog.walkies)
      //  {
            lineRenderer.enabled = true;
            UpdateCircle();
            transform.position = player.position;  // Keep the circle centered on the player
      //  }
      //  else
     //   {
     //       lineRenderer.enabled = false;
     //   }
    }

    void UpdateCircle()
    {
        // Calculate number of visible dots based on dotSpacing
        int visibleDots = Mathf.CeilToInt(segments * dotSpacing);

        // Set LineRenderer position count
        lineRenderer.positionCount = visibleDots;

        // Angle step for each segment
        float angleStep = 360f / segments;

        // Generate points along the circumference
        int dotIndex = 0;
        for (int i = 0; i < segments; i++)
        {
            // Only place a point every other segment for dotted effect
            if (i % 2 == 0)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector3 point = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);
                lineRenderer.SetPosition(dotIndex, point);
                dotIndex++;
            }
        }
    }
}