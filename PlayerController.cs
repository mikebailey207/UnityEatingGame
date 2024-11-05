using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject house;
    bool canHitCircle = true;
    public Transform[] circles; // Array of circles to jump between
    private int currentCircleIndex = 0; // Track the current circle
    public float launchForce = 10f; // Force applied when launching
    public int maxDoubleJumps = 1; // Maximum double jumps
    private int doubleJumpCount = -1; // Current double jumps available
    private Rigidbody2D rb;
    public float airMovementSpeed = 5;
    private Transform lastCircle; // Reference to the last circle
    private bool grounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Disable gravity at the start

        /*// Deactivate all circles except the first one
         for (int i = 1; i < circles.Length; i++)
         {
             circles[i].gameObject.SetActive(false);
         }
         lastCircle = circles[currentCircleIndex];
     }*/
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0); // Reload the scene
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (grounded)
            {
                Launch(); // Launch the player
            }
            else if (doubleJumpCount >= 1)
            {
                MouseLaunch();
            }
        }

        if (!grounded) HandleAirMovement();
    }

    private void MouseLaunch()
    {
        rb.velocity = Vector2.zero;
        Vector2 launchDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
     //   launchDirection.y = Mathf.Max(launchDirection.y, 0); // Ensure a positive Y value for launch
        rb.AddForce(launchDirection.normalized * launchForce, ForceMode2D.Impulse); // Launch towards mouse cursor
        transform.parent = null; // Detach from any parent
        doubleJumpCount--;
    }

    private void HandleAirMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized * airMovementSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }

    private void Launch()
    {
        canHitCircle = true;
        grounded = false;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.gravityScale = 1; // Enable gravity
        Vector2 launchDirection = transform.parent.right; // Get the right direction of the parent circle
        rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse); // Launch in the direction the circle is facing
        transform.parent = null; // Detach from any parent

        // Deactivate the circle after launching
        if (lastCircle != null && currentCircleIndex >=2)
        {
            lastCircle.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RotatingCircle") && canHitCircle )
        {
            maxDoubleJumps++;
            doubleJumpCount = maxDoubleJumps;
            grounded = true;

            if (lastCircle != null)
            {
                // Draw a line from last circle to current circle
                DrawLineBetweenCircles(lastCircle.position, collision.transform.position);
            }

            lastCircle = collision.transform; // Update last circle to the current one
            Transform targetCircle = collision.transform;

            transform.rotation = targetCircle.rotation;
            transform.parent = targetCircle;
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(LerpToCircle(targetCircle));

            // Activate the next circle
         
            if (currentCircleIndex < circles.Length)
            {
                circles[currentCircleIndex].gameObject.SetActive(true);
            }
            currentCircleIndex++;
        }
        if (currentCircleIndex >= circles.Length+2) house.SetActive(true);
        canHitCircle = false;
    }

    private IEnumerator LerpToCircle(Transform targetCircle)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = targetCircle.position;
        float lerpDuration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < lerpDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / lerpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        transform.SetParent(targetCircle);
        rb.gravityScale = 0;
    }

    private void DrawLineBetweenCircles(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("LineBetweenCircles");
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.white;
    }
}