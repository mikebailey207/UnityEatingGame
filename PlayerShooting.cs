using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerShooting : MonoBehaviour
{
    public AudioSource wildgunSound;
    public SpriteRenderer[] guns;
    public GameObject casingPrefab;   // Assign your bullet casing prefab
    public Transform casingSpawnPoint;
    public GameObject oldBadgerText;
    public Transform housePoint;
    private LineRenderer lineRenderer;

    public float safeDistance = 1.5f;
    Transform playerTransform;
    public GameObject bulletPrefab, wildgunBulletPrefab;
    public Transform gunTransform;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 10f;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;
    public DogRandomMovement dog;
    public float recoilRate = 0.5f;
//    int upgrade = 0;
    // Cinemachine camera for screen shake
    public CinemachineVirtualCamera cinemachineCamera;
    private CinemachineBasicMultiChannelPerlin cameraNoise;

    public float shakeAmplitude = 1.2f;   // Amplitude of the camera shake
    public float shakeFrequency = 2.0f;   // Frequency of the camera shake
    public float shakeDuration = 0.1f;    // Duration of the shake effect

    private float shakeTimer = 0f;

    private void Start()
    {
        playerTransform = transform;
        lineRenderer = GetComponentInChildren<LineRenderer>();

        // Access the Cinemachine noise component for shake
        if (cinemachineCamera != null)
        {
            cameraNoise = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        lineRenderer.positionCount = 2;
        if (GameManager.Instance.day == 4)
        {
            FirstBadgerDay();
        }
        if (GameManager.Instance.gunUpgrade == 3)
        {
            bulletSpawnPoint.gameObject.GetComponent<Animator>().enabled = true;
        }
    }

    void Update()
    {
      /*  if(Input.GetKeyDown(KeyCode.U))
        {
            GameManager.Instance.gunUpgrade++;
           // upgrade++;
            if(GameManager.Instance.gunUpgrade <= 3) GameManager.Instance.fireRate -= .17f;
            Debug.Log("Upgrade: " + GameManager.Instance.gunUpgrade);
            if (GameManager.Instance.gunUpgrade == 4) 
            return;
        }*/
        if (dog.walkies && GameManager.Instance.day >= 4)
        {
            //         lineRenderer.SetPosition(0, housePoint.position);
            //      lineRenderer.SetPosition(1, playerTransform.position);
            gunTransform.gameObject.SetActive(true);
            guns[GameManager.Instance.gunUpgrade].enabled = true;
          
            RotateGunToMouse();

            // Check if shooting and apply recoil and shake
            if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
            {
                if (GameManager.Instance.gunUpgrade < 3)
                {
                    ShootBullet();
                    ApplyRecoil();
                    // StartCameraShake();

                    nextFireTime = Time.time + GameManager.Instance.fireRate;
                }
                else if(GameManager.Instance.gunUpgrade == 3)
                {
                    ShootBullet();
                    ApplyRecoil();
                    // StartCameraShake();

                    nextFireTime = Time.time + GameManager.Instance.fireRate;
                    wildgunSound.Play();
                    
                }
            }
            if(Input.GetMouseButtonUp(0))
            {
                if(GameManager.Instance.gunUpgrade == 3)
                {
                    wildgunSound.Stop();
                }
            }
        }
        else if (!dog.walkies)
        {
            gunTransform.gameObject.SetActive(false);
            lineRenderer.enabled = false;
        }

        if (Input.GetMouseButton(0))
        {
            if (oldBadgerText != null) oldBadgerText.SetActive(false);
        }
        if (Input.GetMouseButtonUp(0))
        {
            StopCameraShake();
        }
        // Update camera shake timer
        //   UpdateCameraShake();
    }
   void UpgradeGun()
    {
        if (GameManager.Instance.gunUpgrade <= 3) GameManager.Instance.fireRate -= .17f;
        Debug.Log("Upgrade: " + GameManager.Instance.gunUpgrade);
        if (GameManager.Instance.gunUpgrade == 4)
            return;

    }
    void FirstBadgerDay()
    {
        oldBadgerText.SetActive(true);
        Invoke("TurnOffBadgerText", 7);
    }

    void TurnOffBadgerText()
    {
        oldBadgerText.SetActive(false);
    }

    void RotateGunToMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector2 direction = mousePosition - playerTransform.position;
        float distanceToMouse = direction.magnitude;

        if (distanceToMouse > safeDistance)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            gunTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
    void SpawnCasing()
    {
        GameObject casing = Instantiate(casingPrefab, casingSpawnPoint.position, Quaternion.identity);

        // Apply upward and rotational force to the casing
        Rigidbody2D rb = casing.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 upwardForce = new Vector2(Random.Range(-0.5f, 0.5f), 1f);  // Random x for slight spread
            rb.AddForce(upwardForce * 10f, ForceMode2D.Impulse);  // Adjust multiplier as needed

            float randomTorque = Random.Range(-200f, 200f);
            rb.AddTorque(randomTorque);

            // Start coroutine to stop casing movement after 2 seconds
            StartCoroutine(StopCasingMovement(rb));
        }
    }
    void ShootBullet()
    {
        if (GameManager.Instance.gunUpgrade < 3)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = bulletSpawnPoint.right * bulletSpeed;
            }
        }
        else if (GameManager.Instance.gunUpgrade == 3)
        {
            GameManager.Instance.fireRate = 0.05f;

            GameObject bullet = Instantiate(wildgunBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            GameObject bullet2 = Instantiate(wildgunBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation 
                * Quaternion.Euler(0, 0, 45));
            GameObject bullet3 = Instantiate(wildgunBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation 
                * Quaternion.Euler(0, 0, -45));
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            Rigidbody2D rb2 = bullet2.GetComponent<Rigidbody2D>();
            Rigidbody2D rb3 = bullet3.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = bullet.transform.right * bulletSpeed;  // Use bullet's own right direction
            }
            if (rb2 != null)
            {
                rb2.velocity = bullet2.transform.right * bulletSpeed;  // Use bullet2's right direction
            }
            if (rb3 != null)
            {
                rb3.velocity = bullet3.transform.right * bulletSpeed;  // Use bullet3's right direction
            }
        }
    }
    IEnumerator StopCasingMovement(Rigidbody2D rb)
    {
        yield return new WaitForSeconds(0.5f);

        // Gradually bring velocity and angular velocity to zero
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;  // Optionally, set to kinematic to completely stop physics interactions
    }

    void ApplyRecoil()
    {
        // Apply a small random force to simulate recoil
        Vector2 recoil = new Vector2(Random.Range(-recoilRate, recoilRate), Random.Range(-recoilRate, recoilRate));
        playerTransform.position += (Vector3)recoil;

        SpawnCasing();
    }

    void StartCameraShake()
    {
        if (cameraNoise != null)
        {
            cameraNoise.m_AmplitudeGain = shakeAmplitude;
            cameraNoise.m_FrequencyGain = shakeFrequency;
            //  shakeTimer = shakeDuration;
        }
    }
    void StopCameraShake()
    {
        if (cameraNoise != null)
        {
            // Define the resting values for amplitude and frequency
            float targetAmplitude = 0.2f;
            float targetFrequency = 0.2f;

            // Smoothly transition back to the resting values using Lerp
            cameraNoise.m_AmplitudeGain = Mathf.Lerp(cameraNoise.m_AmplitudeGain, targetAmplitude, Time.deltaTime * 5f);
            cameraNoise.m_FrequencyGain = Mathf.Lerp(cameraNoise.m_FrequencyGain, targetFrequency, Time.deltaTime * 5f);
        }
    }
    void UpdateCameraShake()
    {
        if (cameraNoise != null && shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                cameraNoise.m_AmplitudeGain = 0f;
                cameraNoise.m_FrequencyGain = 0f;
            }
        }
    }
}