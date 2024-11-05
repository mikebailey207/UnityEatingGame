using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
//using Cinemachine;
public class SceneChanger : MonoBehaviour
{
    GameObject doris;
    bool dorisRotating = false;
    GameObject disguiseVisual;
    bool canEnterHotdogChallenge = false;
    bool canEnterTown = false;
    bool canEnterHouse = false;
    bool canGoOutside = false;
    bool canGoToBed = false;
    bool canReturnHome = false;
    public GameObject interactText;
    private static SceneChanger instance;
    // Start is called before the first frame update

    

    void Start()
    {
       disguiseVisual = GameObject.Find("Disguise");
        if (GameManager.Instance.wearingDisguise)
        {
            ApplyDisguise(true);
        }
        else
        {
            ApplyDisguise(false);
        }
        if (disguiseVisual == null)
        {
            Debug.LogError("Disguise GameObject not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(dorisRotating)
        {
            if(doris!=null)
            {
                doris.transform.Rotate(0, 0, 100 * Time.deltaTime);
                
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            /*  if (!disguised)
              {
                  disguise.SetActive(true);
                  GameManager.Instance.wearingDisguise = true;
                  disguised = true;
              }
              else
              {
                  disguise.SetActive(false);
                  disguised = false;
                  GameManager.Instance.wearingDisguise = false;
              }*/
            ToggleDisguise();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextDay();
        }
            if (Input.GetKeyDown(KeyCode.E))
        {
            if (canEnterHouse)
                SceneManager.LoadScene("House");
            else if (canGoOutside)
                SceneManager.LoadScene("ExteriorHouse");
            else if (canEnterHotdogChallenge)
                SceneManager.LoadScene("FoodChallenge");
            else if (canReturnHome)
                SceneManager.LoadScene("GardenExit");
            else if (canEnterTown && GameManager.Instance.walkiesComplete)
                SceneManager.LoadScene("TownEntrace");
            
            else if (canGoToBed)
            {
                if (GameManager.Instance.dailyTasksCompleted)
                {
                    NextDay();
                }
                else interactText.GetComponent<TextMeshProUGUI>().text = "Still stuff to do before bed";
            }


        }
    }
    void StopRotating()
    {
        dorisRotating = false;
    }
    public void ToggleDisguise()
    {
        bool newDisguiseState = !GameManager.Instance.wearingDisguise;
        GameManager.Instance.wearingDisguise = newDisguiseState;
        ApplyDisguise(newDisguiseState);
    }
    public void ApplyDisguise(bool isDisguised)
    {
        // Enable or disable the disguise visuals
        disguiseVisual.SetActive(isDisguised);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Doris") && GameManager.Instance.dailyTasksCompleted)
        {
            doris = collision.gameObject;
            doris.transform.parent = gameObject.transform;
            doris.transform.position = gameObject.transform.position;
            dorisRotating = true;
            Invoke("StopRotating", 1);
        }
        if (collision.gameObject.CompareTag("House"))
        {
            canEnterHouse = true;
            if (interactText != null)
            {
                interactText.SetActive(true);
                interactText.GetComponent<TextMeshProUGUI>().text = "E to interact";
            }
        }
        if (collision.gameObject.CompareTag("Outside"))
        {
            if (GameManager.Instance.day != 1)
            {
                canGoOutside = true;
                if (interactText != null)
                {
                    interactText.SetActive(true);
                    interactText.GetComponent<TextMeshProUGUI>().text = "E to interact";
                }
            }
        }
        if (collision.gameObject.CompareTag("HotdogChallenge"))
        {
            canEnterHotdogChallenge = true;
            if (interactText != null)
            {
                interactText.SetActive(true);
                interactText.GetComponent<TextMeshProUGUI>().text = "E to interact";
            }
        }
        if (collision.gameObject.CompareTag("TownExit"))
        {
            canReturnHome = true;
            if (interactText != null)
            {
                interactText.SetActive(true);
                interactText.GetComponent<TextMeshProUGUI>().text = "E to interact";
            }
        }
        if (collision.gameObject.CompareTag("TownEntrance"))
        {
            if (GameManager.Instance.walkiesComplete)
            {
                canEnterTown = true;
                if (interactText != null)
                {
                    interactText.SetActive(true);
                    interactText.GetComponent<TextMeshProUGUI>().text = "E to interact";
                }
            }
            else
            {
                if (interactText != null)
                {
                    interactText.SetActive(true);
                    interactText.GetComponent<TextMeshProUGUI>().text = "You still have tasks to do for Doris";
                }
            }
        }
        if (collision.gameObject.CompareTag("Upstairs"))
        {
            canGoToBed = true;
            if (interactText != null)
            {
                interactText.SetActive(true);
                interactText.GetComponent<TextMeshProUGUI>().text = "E to interact";
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("House"))
        {
            canEnterHouse = false;
            ResetInteractText();
        }
        if (collision.gameObject.CompareTag("Outside"))
        {
            canGoOutside = false;
            ResetInteractText();
        }
        if (collision.gameObject.CompareTag("TownExit"))
        {
            canReturnHome = false;
            ResetInteractText();
        }
        if (collision.gameObject.CompareTag("HotdogChallenge"))
        {
            canEnterHotdogChallenge = false;
            ResetInteractText();
        }
        if (collision.gameObject.CompareTag("TownEntrance"))
        {
            canEnterTown = false;
            ResetInteractText();
        }
        if (collision.gameObject.CompareTag("Upstairs"))
        {
            canGoToBed = false;
            ResetInteractText();
        }
    }

    // Helper method to reset the interact text
    private void ResetInteractText()
    {
        if (interactText != null)
        {
            interactText.SetActive(false);
            interactText.GetComponent<TextMeshProUGUI>().text = "E to interact";
        }
    }

    void NextDay()
    {
        GameManager.Instance.walkiesComplete = false;
        GameManager.Instance.houseVisits = 0;
        GameManager.Instance.day++;
        GameManager.Instance.dailyTasksCompleted = false;
        SceneManager.LoadScene("House");
    }
    public void RetryHouse()
    {
        //GameObject.Find("GameOverScreen").SetActive(false);
        GameManager.Instance.houseVisits = 0;
        SceneManager.LoadScene("House");
       


    }
}
