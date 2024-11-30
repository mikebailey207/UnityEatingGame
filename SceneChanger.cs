using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
//using Cinemachine;
public class SceneChanger : MonoBehaviour
{
    private AudioSource huhSound;
    public AudioSource disguiseSound;
    GameObject doris;
    bool dorisRotating = false;
    GameObject disguiseVisual;
    public GameObject townDisguiseMessage, disguiseButton;

    bool canEnterSushiChallenge = false;
    bool canEnterHotdogChallenge = false;
    bool canEnterPizzaChallenge = false;
    bool canEnterNachosChallenge = false;
    bool canEnterCornChallenge = false;
    bool canEnterChilliChallenge = false;

    bool canEnterShop = false;
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
        interactText.SetActive(false);
       disguiseVisual = GameObject.Find("Disguise");
        if (GameManager.Instance.wearingDisguise && GameManager.Instance.inSaffronWalden)
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
        huhSound = GameObject.Find("HuhSound").GetComponent<AudioSource>();
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
            
            ToggleDisguise();
        }
       
            if (Input.GetKeyDown(KeyCode.E))
        {
            if (canEnterHouse)
                SceneManager.LoadScene("House");
            else if (canGoOutside && GameManager.Instance.canStartDay)
                SceneManager.LoadScene("ExteriorHouse");
            else if (canEnterHotdogChallenge)
                SceneManager.LoadScene("FoodChallenge");
            else if (canEnterNachosChallenge)
                SceneManager.LoadScene("NachosChallenge");
            else if (canEnterCornChallenge)
                SceneManager.LoadScene("CornChallenge");
            else if (canEnterSushiChallenge)
                SceneManager.LoadScene("SushiChallenge");
            else if (canEnterChilliChallenge)
                SceneManager.LoadScene("ChiliChallenge");
            else if (canEnterPizzaChallenge)
                SceneManager.LoadScene("TypingGame");
            else if (canEnterShop && !GameManager.Instance.walkingDog)
                SceneManager.LoadScene("Shop");
            else if (canReturnHome)
            {
                if (GameManager.Instance.dailyTasksCompleted)
                {
                    GameManager.Instance.inSaffronWalden = false;
                    SceneManager.LoadScene("GardenExit");
                }
                else if(!GameManager.Instance.dailyTasksCompleted)
                {
                    interactText.GetComponent<TextMeshProUGUI>().text = "Still need to earn some dosh before going home";
                    
                }
            }
               
            else if (canEnterTown && GameManager.Instance.walkiesComplete)
            {
                if (GameManager.Instance.day == 2 && !GameManager.Instance.wearingDisguise)
                {
                    GameManager.Instance.talking = true;
                    ShowTownDisguiseMessage();
                    GetComponent<Animator>().SetBool("IsRunning", false);
                }
                else
                {
                    ProceedToTown();
                }
            }

            else if (canGoToBed)
            {
                if (GameManager.Instance.dailyTasksCompleted)
                {
                    NextDay(); // THIS IS BEING CALLED WHEN IT RUNS OUT OF TEXT
                }
                else interactText.GetComponent<TextMeshProUGUI>().text = "Still stuff to do before bed";
            }


        }
    }
    void ShowTownDisguiseMessage()
    {
        if (townDisguiseMessage != null)
        {
            townDisguiseMessage.SetActive(true);
        }
        if (disguiseButton != null)
        {
            disguiseButton.SetActive(true);
        }
    }
    void ProceedToTown()
    {
        GameManager.Instance.inSaffronWalden = true;
        GameManager.Instance.talking = false;
        SceneManager.LoadScene("TownEntrace");
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
        disguiseButton.SetActive(false);
        townDisguiseMessage.SetActive(false);
        disguiseSound.Play();
        Invoke("ProceedToTown", 3);
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
            if (interactText != null && !GameManager.Instance.walkingDog)
            {
                interactText.SetActive(true);
                interactText.GetComponent<TextMeshProUGUI>().text = "E to enter house";
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
                    interactText.GetComponent<TextMeshProUGUI>().text = "E to go outside";
                }
            }
        }
        if (collision.gameObject.CompareTag("HotdogChallenge"))
        {
            canEnterHotdogChallenge = true;
            if (interactText != null)
            {
                interactText.SetActive(true);
                if (GameManager.Instance.day >= 3)
                {
                    interactText.GetComponent<TextMeshProUGUI>().text = "E to enter the Hot Dog Emporium " + "Current prize £" +
                            GameManager.Instance.hotDogPrizeMoney;
                }
                else if(GameManager.Instance.day == 2)
                {
                    interactText.GetComponent<TextMeshProUGUI>().text = "Well this place will at least let me in.. press E to enter Hot dog emporium";
                }
            }
        }
        if (collision.gameObject.CompareTag("ChilliChallenge"))
        {if (GameManager.Instance.reputation >= 119.5f)
            {
                canEnterChilliChallenge = true;
                if (interactText != null)
                {
                    interactText.SetActive(true);
                    interactText.GetComponent<TextMeshProUGUI>().text = "E to enter the schofield chilli challenge";
                }
            }
            else
            {
                canEnterChilliChallenge = false;
                if (interactText != null)
                {
                    interactText.SetActive(true);
                    huhSound.Play();
                    interactText.GetComponent<TextMeshProUGUI>().text = "You need at least 120 myspace followers to even think about the chili place. Hothothot";
                }
            }
        }
        if (collision.gameObject.CompareTag("PizzaChallenge"))
        {
            if (GameManager.Instance.reputation >= 89.5f)
            {
                canEnterPizzaChallenge = true;
                if (interactText != null)
                {
                    interactText.SetActive(true);
                    interactText.GetComponent<TextMeshProUGUI>().text = "E to enter Yumma-mia's pizza " + "Current prize £" +
                        GameManager.Instance.pizzaPrizeMoney;
                }
            }
            else
            {
                canEnterPizzaChallenge = false;
                if (interactText != null)
                {
                    interactText.SetActive(true);
                    huhSound.Play();
                    interactText.GetComponent<TextMeshProUGUI>().text = "You need at least 90 myspace followers to plunge into pizza";
                }
            }
        }
        if (collision.gameObject.CompareTag("NachosChallenge"))
        {
            if (GameManager.Instance.reputation >= 29.5f)
            {
                canEnterNachosChallenge = true;
                if (interactText != null)
                {
                    interactText.SetActive(true);
                    interactText.GetComponent<TextMeshProUGUI>().text = "E to enter Nacho Man challenge " + "Current prize £" +
                        GameManager.Instance.nachosPrizeMoney;
                }
            }
            else
            {
                canEnterNachosChallenge = false;
                if (interactText != null)
                {
                    interactText.SetActive(true);
                    huhSound.Play();
                    interactText.GetComponent<TextMeshProUGUI>().text = "You need at least 30 myspace followers to go all Nacho";
                }
            }
        }
        if (collision.gameObject.CompareTag("CornChallenge"))
        {
            if (GameManager.Instance.reputation >= 59.5f)
            {
                canEnterCornChallenge = true;
                if (interactText != null)
                {
                    interactText.SetActive(true);
                    interactText.GetComponent<TextMeshProUGUI>().text = "E to enter corn challenge. " + "Current prize £" +
                        GameManager.Instance.cornPrizeMoney;
                }
            }
            else
            {
                canEnterCornChallenge = false;
                if (interactText != null)
                {
                    interactText.SetActive(true);
                    huhSound.Play();
                    interactText.GetComponent<TextMeshProUGUI>().text = "You need at least 60 myspace followers to get any kind of corn on";
                }
            }
        }
        if (collision.gameObject.CompareTag("SushiChallenge"))
        {
            if (GameManager.Instance.reputation >= 149.5f)
            {
                canEnterSushiChallenge = true;
                if (interactText != null)
                {
                    interactText.SetActive(true);
                    interactText.GetComponent<TextMeshProUGUI>().text = "E to enter Sushi challenge";
                }
            }
            else
            {
                canEnterSushiChallenge = false;
                interactText.SetActive(true);
                huhSound.Play();
                interactText.GetComponent<TextMeshProUGUI>().text = "Sushi below 150 myspace followers?! Forget it!";
            }
        }
        if (collision.gameObject.CompareTag("TownExit"))
        {

            canReturnHome = true;
            if (interactText != null)
            {
                interactText.SetActive(true);
                interactText.GetComponent<TextMeshProUGUI>().text = "E to leave Saffron Walden";
            }
        }
        if (collision.gameObject.CompareTag("TownEntrance"))
        {
            if (GameManager.Instance.walkiesComplete && !GameManager.Instance.dailyTasksCompleted)
            {
                canEnterTown = true;
                if (interactText != null)
                {
                    interactText.SetActive(true);
                    interactText.GetComponent<TextMeshProUGUI>().text = "E to enter Saffron Walden";
                  
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
                interactText.GetComponent<TextMeshProUGUI>().text = "E to hit the hay";
            }
        }
        if (collision.gameObject.CompareTag("Shop"))
        {
            if (GameManager.Instance.day >= 3 && !GameManager.Instance.walkingDog)
            {
                canEnterShop = true;
                if (interactText != null)
                {
                    interactText.SetActive(true);
                    if (SceneManager.GetActiveScene().name == "TownEntrace")
                    {
                        interactText.GetComponent<TextMeshProUGUI>().text = "E to enter strange man's wares";
                    }
                    else
                    {
                        interactText.GetComponent<TextMeshProUGUI>().text = "E to enter strange man's wares";
                    }
                }
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
        if (collision.gameObject.CompareTag("PizzaChallenge"))
        {
           canEnterPizzaChallenge = false;
            ResetInteractText();
        }
        if (collision.gameObject.CompareTag("NachosChallenge"))
        {
            canEnterNachosChallenge = false;
            ResetInteractText();
        }
        if (collision.gameObject.CompareTag("ChilliChallenge"))
        {
            canEnterChilliChallenge = false;
            ResetInteractText();
        }
        if (collision.gameObject.CompareTag("CornChallenge"))
        {
            canEnterCornChallenge = false;
            ResetInteractText();
        }
        if (collision.gameObject.CompareTag("SushiChallenge"))
        {
            canEnterSushiChallenge = false;
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
        if (collision.gameObject.CompareTag("Shop"))
        {
            canEnterShop = false;
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
        if (GameManager.Instance.reputation < 599.5f)
        {
            canGoToBed = false;
            GameManager.Instance.walkiesComplete = false;
            GameManager.Instance.houseVisits = 0;
            if (GameManager.Instance.day >= 2) GameManager.Instance.taskIndex++;
            GameManager.Instance.day++;
            GameManager.Instance.paidDoris = false;
            GameManager.Instance.dailyTasksCompleted = false;
            // GameManager.Instance.walkiesLength += 5;
            SceneManager.LoadScene("House");
        }
        else if(GameManager.Instance.reputation >= 599.5f)
        {
            canGoToBed = false;
            GameManager.Instance.walkiesComplete = true;
            GameManager.Instance.houseVisits = 0;
            GameManager.Instance.taskIndex+=2;
            GameManager.Instance.day++;
            GameManager.Instance.paidDoris = false;
            GameManager.Instance.dailyTasksCompleted = false;
            // GameManager.Instance.walkiesLength += 5;
            SceneManager.LoadScene("House");
        }
    }
    public void RetryHouse()
    {
        //GameObject.Find("GameOverScreen").SetActive(false);
        GameManager.Instance.houseVisits = 0;
        SceneManager.LoadScene("House");
       


    }
}
