using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;   

public class GameManager : MonoBehaviour
{
    public GameObject persistentUI;
    public float walkiesCountdownStart = 20;// ADD TO SAVE AND LOAD FUNCTIONS
    public float bardMyspaceTarget = 1000;// ADD TO SAVE AND LOAD FUNCTIONS

    bool canTurnOnUIscreen = true;// ADD TO SAVE AND LOAD FUNCTIONS
    public GameObject toggleProgressText;
    bool canToggleUI = false;// ADD TO SAVE AND LOAD FUNCTIONS
    bool progressUIon = true;
    public TextMeshProUGUI hotdogProgressUI, nachosProgressUIText, cornProgressUIText, pizzaProgressUIText, chilliProgressUIText, sushiProgressUIText;

    public int bardDeaths = 0; // ADD TO SAVE AND LOAD FUNCTIONS
    public bool vegan = false;
    public bool beenShopping = false;
    public bool canSpeedRun = false;
    AudioSource mouseClickSound;
    public bool inSaffronWalden;// ADD TO SAVE AND LOAD FUNCTIONS

    public GameObject tshirtHolder;
    public GameObject nachosT, cornT, pizzaT, chiliT, sushiT;
    public GameObject tick1, tick2, tick3, tick4, tick5, tick6;
    public bool canShowWimbledonDialogue = true;

    public bool walkingDog = false;


    public Slider finaleSlider, finaleSliderBard;
    public Image playerIcon, bardIcon;


    public bool canShowNewItemNotification = true;// ADD TO SAVE AND LOAD FUNCTIONS

    public string[] fartReminderSentences, fartNames;

    public bool canStartDay = false;// ADD TO SAVE AND LOAD FUNCTIONS
    private Coroutine bardEatingCoroutine;


    [Range(0.5f, 3.0f)] // Adjustable in the Inspector, defaults between 0.5x and 3x speed
    public float bardSpeedModifier = 1.0f;
    public int bardEatenCount = 0;

    public bool startedFinale = false;
    public TextMeshProUGUI finalePlayerCounterText, finaleBardCounterText;

    public bool isFinale = false;

    public int hotdogsFinaleTarget = 20;
    public int nachosFinaleTarget = 60;
    public int cornFinaleTarget = 10;
    public int pizzaFinaleTarget = 10;
    public int sushiFinaleTarget = 50;
    public int chiliFinaleTarget = 50;

    public int finalePlayerCounter = 0;
  


    public float speedrunTime = 0f;
    public bool speedrun = false;
    public TextMeshProUGUI speedrunTimerText;

    public bool canShowBardIntro = true; // ADD TO SAVE AND LOAD FUNCTIONS

    public TextMeshProUGUI tshirtsText;
    public int tShirts = 0;

    public bool gotAllTshirts = false;

    public bool gotHotdogTshirt;
    public bool gotNachosTshirt;
    public bool gotCornTshirt;
    public bool gotPizzaTshirt;
    public bool gotChiliTshirt;
    public bool gotSushiTshirt;


    public bool nachosUnlockedShown = false; 
    public bool cornUnlockedShown = false;
    public bool pizzaUnlockedShown = false;
    public bool chiliUnlockedShown = false;
    public bool sushiUnlockedShown = false;

    public string currentChallengeType;

    public PlayerSettings playerSettings;
    public GameObject mainMenu;
    public AudioSource music, finaleMusic;

    public bool isPaused = false;

    // Reference to the Pause Screen GameObject
    [SerializeField] private GameObject pauseScreen;

    public float reputation = 0; 
    public TextMeshProUGUI repText;

    public int dailySushiBest = 0; 


    public float fireRate = 0.5f; 

    public int gunUpgrade = 0; 
    public bool gotMagnum = false;
    public bool gotSubMachineGun = false; 
    public bool gotWildGun = false; 


    public bool gotTrainers;
    public bool boughtLead = false;
    public float leadMaxLength = 7;

    public float moneyCollectedThisSession = 0;
    public int hotdogsVisitCount = 0;
    public int nachosVisitCount = 0;
    public int pizzaVisitCount = 0;
    public int cornVisitCount = 0;
    public int sushiVisitsCount = 0;
    public int chilliVisitsCount = 0;

    public int index2;//FOR SAVE
    //  public float walkiesLength = 10;
    public string[] dorisDialogueAfterDay1;


   public bool canShowIntroShopkeeper = true;

    public float chilliResistance = 1;

    public float drinkRefreshMultiplier =1;
    public int drinkRefreshIndex = 1;

    public float burpAddMultiplier =1;
    public int burpAddIndex = 1;

    public float staminaLoseMultiplier =1; // NOT USING THIS ONE!
    public int staminaLoseIndex = 1;

    public float fullNessReduceMultiplier =1;
    public int fullNessReduceAddIndex = 1;

    public float biteCostMultiplier = 1;
    public int biteCostIndex = 1;


    public AudioSource fartSound;
    public int hotdogsTarget = 20;
    public int nachosTarget = 20;
    public int pizzaTarget = 5;
    public int cornTarget = 5;

    public bool paidDoris = false;
    public float money = 50;
    public TextMeshProUGUI moneyText, dayText;
    public bool holdingNacho = false;
    public bool talking = false;
    public int biteCost = 15;
    public int fullnessAmount = 0;
    public bool dailyTasksCompleted;
    public bool metDoris;
    public static GameManager Instance;
    public bool wearingDisguise;
    public bool walkiesComplete;

    public float hotDogPrizeMoney = 100;
    public float pizzaPrizeMoney = 100;
    public float nachosPrizeMoney = 100;
    public float cornPrizeMoney = 100;


    public int day = 1;
    public int houseVisits = 0;
    public string[] taskTexts;
    public int taskIndex;

    bool canLoadFinalScene = true;
    private void Awake()
    {

     
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        //walkiesComplete = false;
    }
    private void Start()
    {
        tshirtsText.text = "";
        mouseClickSound = GetComponent<AudioSource>();
       // repText.gameObject.SetActive(false);
        if (speedrun && speedrunTimerText != null)
        {
            //speedrunTimerText.gameObject.SetActive(true);
        }
      
    }
    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "EndSceneTown")
        {
            persistentUI.SetActive(false);
        }
        HandleProgressTexts();
        HandleUIOffInChallenges();
        if (isFinale)
        {
            finaleSlider.value = finalePlayerCounter;
            finaleSliderBard.value = bardEatenCount;
            //  UpdateSlider();
            //  finalePlayerCounterText.gameObject.SetActive(true);
            //  finaleBardCounterText.gameObject.SetActive(true);
        }
        /*  if (finalePlayerCounter >= 170)
          {
              if (canLoadFinalScene)
              {
                  music.Stop();
                  finaleMusic.Stop();
                  SceneManager.LoadScene("EndSceneTown");
                  canLoadFinalScene = false;
              }
          }*/
        if (bardEatenCount >= 170)
        {

            bardEatenCount = 0;
            finalePlayerCounter = 0;
            if (bardEatingCoroutine != null)
            {
                StopCoroutine(bardEatingCoroutine);
            }
            finaleMusic.Stop();
            bardDeaths++;
            SceneManager.LoadScene("OutsidePub");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            // TestShortcut();
        }

        moneyText.text = "£" + money.ToString("F2");
        dayText.text = "Day " + day;
        if (tShirts < 6) tshirtsText.text = "Win all t-shirts (new life goal) " + tShirts + "/6";
        else if (tShirts >= 6)
        {
            tshirtsText.text = " Get 100 more myspace followers: " + reputation.ToString("0") + "/" + bardMyspaceTarget.ToString("0");
            tshirtHolder.gameObject.SetActive(false);
            toggleProgressText.gameObject.SetActive(false);
        }
        finalePlayerCounterText.text = "PLAYER:" + finalePlayerCounter.ToString("0") + "/170";
        finaleBardCounterText.text = bardEatenCount + "/170" + ": BARD";

        if (day >= 2 && hotdogsVisitCount >= 1)
        {
            canToggleUI = true;
            tshirtsText.gameObject.SetActive(true);
           
            repText.gameObject.SetActive(true);
        
            toggleProgressText.SetActive(true);
            if (canTurnOnUIscreen)
            {
                progressUIon = true;
                canTurnOnUIscreen = false;
            }
        }
        repText.text = "MySPACE followers: " + reputation.ToString("0");
        //  if (day >= 2 && SceneManager.GetActiveScene().name == "TownEntrace") repText.gameObject.SetActive(true);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            TogglePauseScreen();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleProgressUI();
        }
       
        if (speedrun)
        {
            speedrunTime += Time.deltaTime;
            UpdateSpeedrunTimer();
        }
        if(Input.GetMouseButtonDown(0) && talking)
        {
            mouseClickSound.Play();
        }
    }
    public void StopBardEatingCoroutine()
    {
        if (bardEatingCoroutine != null)
        {
            StopCoroutine(bardEatingCoroutine);
            bardEatingCoroutine = null; // Clear the reference
        }
    }
    void HandleProgressTexts()
    {
        if (reputation == 0) hotdogProgressUI.text = "DOORS OPEN";
        else hotdogProgressUI.text = "Been there, done that";

        if (reputation <= 29.5f) nachosProgressUIText.text = reputation.ToString("0") + "/30. Locked, more followers required";
        else if (reputation > 29.5f && !gotNachosTshirt) nachosProgressUIText.text = "DOORS OPEN";
        else if (reputation > 29.5f && gotNachosTshirt) nachosProgressUIText.text = "Been there, done that";

        if (reputation <= 59.5f) cornProgressUIText.text = reputation.ToString("0") + "/60. Locked, more followers required";
        else if (reputation > 59.5f && !gotCornTshirt) cornProgressUIText.text = "DOORS OPEN";
        else if (reputation > 59.5f && gotCornTshirt) cornProgressUIText.text = "Been there, done that";

        if (reputation <= 89.5f) pizzaProgressUIText.text = reputation.ToString("0") + "/90. Locked, more followers required";
        else if (reputation > 89.5f && !gotPizzaTshirt) pizzaProgressUIText.text = "DOORS OPEN";
        else if (reputation > 89.5f && gotPizzaTshirt) pizzaProgressUIText.text = "Been there, done that";

        if (reputation <= 119.5f) chilliProgressUIText.text = reputation.ToString("0") + "/120. Locked, more followers required";
        else if (reputation > 119.5f && !gotChiliTshirt) chilliProgressUIText.text = "DOORS OPEN";
        else if (reputation > 119.5f && gotChiliTshirt) chilliProgressUIText.text = "Been there, done that";

        if (reputation <= 149.5f) sushiProgressUIText.text = reputation.ToString("0") + "/150. Locked, more followers required";
        else if (reputation > 149.5f && !gotSushiTshirt) sushiProgressUIText.text = "DOORS OPEN";
        else if (reputation > 149.5f && gotSushiTshirt) sushiProgressUIText.text = "Been there, done that";

    }
    void TestShortcut()
    {
        SceneManager.LoadScene("OutsidePub");
    }
    public void StartBardEating()
    {
        if (!startedFinale) return;

        // Stop any existing coroutine if running
        if (bardEatingCoroutine != null)
        {
            StopCoroutine(bardEatingCoroutine);
        }

        // Reset bard's progress
        bardEatenCount = 0;

        // Start the eating routine
        bardEatingCoroutine = StartCoroutine(BardEatingRoutine());
    }

    private IEnumerator BardEatingRoutine()
    {
        // Define stages for eating with (target count, base time per item in seconds)
        (int target, float minTime, float maxTime)[] stages = new (int, float, float)[]
        {
        (10, 2.0f, 3.0f),   // Hotdogs: 10 in ~25 seconds
        (50, 0.8f, 1.2f),   // Nachos: 40 in ~40 seconds
        (60, 6.0f, 8.0f),   // Corn: 10 in ~70 seconds
        (70, 4.0f, 6.0f),   // Pizza: 10 in ~50 seconds
        (120, 1.3f, 1.7f),  // Sushi: 50 in ~50 seconds
        (170, 1f, 1.6f),  // Chili: 50 in ~50 seconds
        };

        foreach (var stage in stages)
        {
            while (bardEatenCount < stage.target)
            {
                bardEatenCount++;

                // Adjust timing based on bardSpeedModifier (smaller = faster)
                float adjustedMinTime = stage.minTime / bardSpeedModifier;
                float adjustedMaxTime = stage.maxTime / bardSpeedModifier;

                yield return new WaitForSeconds(Random.Range(adjustedMinTime, adjustedMaxTime));
            }
        }

        // End of Bard's eating progression
        Debug.Log("Bard has finished eating!");
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (Instance != null)
        {
            Debug.Log("Re-assigning PauseScreen after scene load.");
            pauseScreen = GameObject.Find("PauseCanvas"); // Adjust name as necessary
        }
    }
    private void UpdateSpeedrunTimer()
    {
        int minutes = Mathf.FloorToInt(speedrunTime / 60f);
        int seconds = Mathf.FloorToInt(speedrunTime % 60f);
        int hundredths = Mathf.FloorToInt((speedrunTime * 100) % 100);

        // Format: MM:SS:HS
        speedrunTimerText.text = "SPEEDRUN: " + string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, hundredths);
    }
    void HandleUIOffInChallenges()
    {
      
        if (SceneManager.GetActiveScene().name != "OutsidePub" &&
            SceneManager.GetActiveScene().name != "TownEntrace" &&
            SceneManager.GetActiveScene().name != "ExteriorHouse" &&
            SceneManager.GetActiveScene().name != "ShopExit" &&       
            SceneManager.GetActiveScene().name != "EndSceneTown" &&
            SceneManager.GetActiveScene().name != "Shop" &&
            SceneManager.GetActiveScene().name != "GardenExit" &&
            SceneManager.GetActiveScene().name != "House")
        {
            moneyText.gameObject.SetActive(false);
            repText.GetComponent<TextMeshProUGUI>().enabled = false;
            dayText.gameObject.SetActive(false);
            speedrunTimerText.gameObject.SetActive(false);
        }
        else
        {
            moneyText.gameObject.SetActive(true);
            repText.GetComponent<TextMeshProUGUI>().enabled = true;
            dayText.gameObject.SetActive(true);
         //   speedrunTimerText.gameObject.SetActive(true);
        }
    }
    public void TogglePauseScreen()
    {
       
       
        if (isPaused)
        {
            music.UnPause();
            // Unpause the game
            Time.timeScale = 1;
            pauseScreen.SetActive(false);
            isPaused = false;
        }
        else
        {
            music.Pause();
            // Pause the game
            Time.timeScale = 0;
            pauseScreen.SetActive(true);
            isPaused = true;
        }
    }
    public void ToggleProgressUI()
    {
        if (progressUIon)
        {         
            tshirtHolder.SetActive(false);
            progressUIon = false;
        }
        else if(!progressUIon)
        {
            tshirtHolder.SetActive(true);
            progressUIon = true;
        }
    }
    public void ContinueGame()
    {
        talking = false;
        music.UnPause();
        // Unpause the game and hide the pause screen
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
        isPaused = false;
    }

    public void SaveGame()
    {
        talking = false;
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
        //music.Play();
        pauseScreen.SetActive(false);
        PlayerPrefs.SetFloat("Reputation", reputation);
        PlayerPrefs.SetInt("DailySushiBest", dailySushiBest);
        PlayerPrefs.SetFloat("FireRate", fireRate);
        PlayerPrefs.SetInt("GunUpgrade", gunUpgrade);
        PlayerPrefs.SetInt("GotMagnum", gotMagnum ? 1 : 0);
        PlayerPrefs.SetInt("GotSubMachineGun", gotSubMachineGun ? 1 : 0);
        PlayerPrefs.SetInt("GotWildGun", gotWildGun ? 1 : 0);
        PlayerPrefs.SetInt("GotTrainers", gotTrainers ? 1 : 0);
        PlayerPrefs.SetInt("BoughtLead", boughtLead ? 1 : 0);
        PlayerPrefs.SetFloat("LeadMaxLength", leadMaxLength);
        PlayerPrefs.SetFloat("MoneyCollectedThisSession", moneyCollectedThisSession);
        PlayerPrefs.SetInt("HotdogsVisitCount", hotdogsVisitCount);
        PlayerPrefs.SetInt("NachosVisitCount", nachosVisitCount);
        PlayerPrefs.SetInt("PizzaVisitCount", pizzaVisitCount);
        PlayerPrefs.SetInt("ChilliVisitsCount", chilliVisitsCount);
        PlayerPrefs.SetInt("SushiVisitsCount", sushiVisitsCount);
        PlayerPrefs.SetInt("CornVisitCount", cornVisitCount);
        PlayerPrefs.SetInt("Index2", index2);
        PlayerPrefs.SetInt("CanShowIntroShopkeeper", canShowIntroShopkeeper ? 1 : 0);
        PlayerPrefs.SetFloat("DrinkRefreshMultiplier", drinkRefreshMultiplier);
        PlayerPrefs.SetInt("DrinkRefreshIndex", drinkRefreshIndex);
        PlayerPrefs.SetFloat("BurpAddMultiplier", burpAddMultiplier);
        PlayerPrefs.SetInt("BurpAddIndex", burpAddIndex);
        PlayerPrefs.SetFloat("FullNessReduceMultiplier", fullNessReduceMultiplier);
        PlayerPrefs.SetInt("FullNessReduceAddIndex", fullNessReduceAddIndex);
        PlayerPrefs.SetFloat("BiteCostMultiplier", biteCostMultiplier);
        PlayerPrefs.SetInt("BiteCostIndex", biteCostIndex);
        PlayerPrefs.SetInt("HotdogsTarget", hotdogsTarget);
        PlayerPrefs.SetInt("NachosTarget", nachosTarget);
        PlayerPrefs.SetInt("PizzaTarget", pizzaTarget);
        PlayerPrefs.SetInt("CornTarget", cornTarget);
        PlayerPrefs.SetInt("PaidDoris", paidDoris ? 1 : 0);
        PlayerPrefs.SetFloat("Money", money);
        PlayerPrefs.SetInt("HoldingNacho", holdingNacho ? 1 : 0);
      //  PlayerPrefs.SetInt("Talking", talking ? 1 : 0);
        PlayerPrefs.SetInt("FullnessAmount", fullnessAmount);
        PlayerPrefs.SetInt("DailyTasksCompleted", dailyTasksCompleted ? 1 : 0);
        PlayerPrefs.SetInt("MetDoris", metDoris ? 1 : 0);
        PlayerPrefs.SetInt("WearingDisguise", wearingDisguise ? 1 : 0);
        PlayerPrefs.SetInt("WalkiesComplete", walkiesComplete ? 1 : 0);
        PlayerPrefs.SetFloat("HotDogPrizeMoney", hotDogPrizeMoney);
        PlayerPrefs.SetFloat("PizzaPrizeMoney", pizzaPrizeMoney);
        PlayerPrefs.SetFloat("NachosPrizeMoney", nachosPrizeMoney);
        PlayerPrefs.SetFloat("CornPrizeMoney", cornPrizeMoney);
        PlayerPrefs.SetInt("Day", day);
        PlayerPrefs.SetInt("HouseVisits", houseVisits);
        PlayerPrefs.SetInt("TaskIndex", taskIndex);

        PlayerPrefs.SetInt("CanShowBardIntro", canShowBardIntro ? 1 : 0);
        PlayerPrefs.SetInt("TShirts", tShirts);
        PlayerPrefs.SetInt("GotAllTshirts", gotAllTshirts ? 1 : 0);
        PlayerPrefs.SetInt("GotHotdogTshirt", gotHotdogTshirt ? 1 : 0);
        PlayerPrefs.SetInt("GotNachosTshirt", gotNachosTshirt ? 1 : 0);
        PlayerPrefs.SetInt("GotCornTshirt", gotCornTshirt ? 1 : 0);
        PlayerPrefs.SetInt("GotPizzaTshirt", gotPizzaTshirt ? 1 : 0);
        PlayerPrefs.SetInt("GotChiliTshirt", gotChiliTshirt ? 1 : 0);
        PlayerPrefs.SetInt("GotSushiTshirt", gotSushiTshirt ? 1 : 0);
        PlayerPrefs.SetInt("CanLoadFinalScene", canLoadFinalScene ? 1 : 0);

        PlayerPrefs.SetInt("CanShowWimbledonDialogue", canShowWimbledonDialogue ? 1 : 0);
        PlayerPrefs.SetInt("WalkingDog", walkingDog ? 1 : 0);
        PlayerPrefs.SetInt("CanShowNewItemNotification", canShowNewItemNotification ? 1 : 0);
        PlayerPrefs.SetInt("CanStartDay", canStartDay ? 1 : 0);

        PlayerPrefs.SetInt("NachosUnlockedShown", nachosUnlockedShown ? 1 : 0);
        PlayerPrefs.SetInt("CornUnlockedShown", cornUnlockedShown ? 1 : 0);
        PlayerPrefs.SetInt("PizzaUnlockedShown", pizzaUnlockedShown ? 1 : 0);
        PlayerPrefs.SetInt("ChiliUnlockedShown", chiliUnlockedShown ? 1 : 0);
        PlayerPrefs.SetInt("SushiUnlockedShown", sushiUnlockedShown ? 1 : 0);

        PlayerPrefs.SetFloat("WalkiesCountdownStart", walkiesCountdownStart);
        PlayerPrefs.SetFloat("BardMyspaceTarget", bardMyspaceTarget);
        PlayerPrefs.SetInt("CanTurnOnUIScreen", canTurnOnUIscreen ? 1 : 0);
        PlayerPrefs.SetInt("CanToggleUI", canToggleUI ? 1 : 0);
        PlayerPrefs.SetInt("BardDeaths", bardDeaths);
        PlayerPrefs.SetInt("InSaffronWalden", inSaffronWalden ? 1 : 0);
        PlayerPrefs.SetInt("CanShowNewItemNotification", canShowNewItemNotification ? 1 : 0);
        PlayerPrefs.SetInt("CanStartDay", canStartDay ? 1 : 0);
        PlayerPrefs.SetInt("CanShowBardIntro", canShowBardIntro ? 1 : 0);

        PlayerPrefs.Save();
        Debug.Log("Game Saved!");
    }
    public void LoadGame()
    {

        talking = false;
        Time.timeScale = 1;
       // Time.timeScale = 1;
        if (!PlayerPrefs.HasKey("Day"))
        {
            Debug.LogWarning("No save data found!");
            return;
        }
       int selectedFartIndex = PlayerPrefs.GetInt("SelectedFartSoundIndex", 0);
       fartSound = playerSettings.fartSounds[selectedFartIndex];
        DontDestroyOnLoad(fartSound.gameObject);
        // Set the correct fart sound based on the loaded index



        mainMenu.SetActive(false);
        reputation = PlayerPrefs.GetFloat("Reputation");
        dailySushiBest = PlayerPrefs.GetInt("DailySushiBest");
        fireRate = PlayerPrefs.GetFloat("FireRate");
        gunUpgrade = PlayerPrefs.GetInt("GunUpgrade");
        gotMagnum = PlayerPrefs.GetInt("GotMagnum") == 1;
        gotSubMachineGun = PlayerPrefs.GetInt("GotSubMachineGun") == 1;
        gotWildGun = PlayerPrefs.GetInt("GotWildGun") == 1;
        gotTrainers = PlayerPrefs.GetInt("GotTrainers") == 1;
        boughtLead = PlayerPrefs.GetInt("BoughtLead") == 1;
        leadMaxLength = PlayerPrefs.GetFloat("LeadMaxLength");
        moneyCollectedThisSession = PlayerPrefs.GetFloat("MoneyCollectedThisSession");
        hotdogsVisitCount = PlayerPrefs.GetInt("HotdogsVisitCount");
        nachosVisitCount = PlayerPrefs.GetInt("NachosVisitCount");
        pizzaVisitCount = PlayerPrefs.GetInt("PizzaVisitCount");
        cornVisitCount = PlayerPrefs.GetInt("CornVisitCount");
        chilliVisitsCount = PlayerPrefs.GetInt("ChilliVisitsCount");
        sushiVisitsCount = PlayerPrefs.GetInt("SushiVisitsCount");
        index2 = PlayerPrefs.GetInt("Index2");
        canShowIntroShopkeeper = PlayerPrefs.GetInt("CanShowIntroShopkeeper") == 1;
        drinkRefreshMultiplier = PlayerPrefs.GetFloat("DrinkRefreshMultiplier");
        drinkRefreshIndex = PlayerPrefs.GetInt("DrinkRefreshIndex");
        burpAddMultiplier = PlayerPrefs.GetFloat("BurpAddMultiplier");
        burpAddIndex = PlayerPrefs.GetInt("BurpAddIndex");
        fullNessReduceMultiplier = PlayerPrefs.GetFloat("FullNessReduceMultiplier");
        fullNessReduceAddIndex = PlayerPrefs.GetInt("FullNessReduceAddIndex");
        biteCostMultiplier = PlayerPrefs.GetFloat("BiteCostMultiplier");
        biteCostIndex = PlayerPrefs.GetInt("BiteCostIndex");
        hotdogsTarget = PlayerPrefs.GetInt("HotdogsTarget");
        nachosTarget = PlayerPrefs.GetInt("NachosTarget");
        pizzaTarget = PlayerPrefs.GetInt("PizzaTarget");
        cornTarget = PlayerPrefs.GetInt("CornTarget");
        paidDoris = PlayerPrefs.GetInt("PaidDoris") == 1;
        money = PlayerPrefs.GetFloat("Money");
        holdingNacho = PlayerPrefs.GetInt("HoldingNacho") == 1;
        talking = PlayerPrefs.GetInt("Talking") == 1;
        fullnessAmount = PlayerPrefs.GetInt("FullnessAmount");
        dailyTasksCompleted = PlayerPrefs.GetInt("DailyTasksCompleted") == 1;
        metDoris = PlayerPrefs.GetInt("MetDoris") == 1;
        wearingDisguise = PlayerPrefs.GetInt("WearingDisguise") == 1;
        walkiesComplete = PlayerPrefs.GetInt("WalkiesComplete") == 1;
        hotDogPrizeMoney = PlayerPrefs.GetFloat("HotDogPrizeMoney");
        pizzaPrizeMoney = PlayerPrefs.GetFloat("PizzaPrizeMoney");
        nachosPrizeMoney = PlayerPrefs.GetFloat("NachosPrizeMoney");
        cornPrizeMoney = PlayerPrefs.GetFloat("CornPrizeMoney");
        day = PlayerPrefs.GetInt("Day");
        houseVisits = PlayerPrefs.GetInt("HouseVisits");
        taskIndex = PlayerPrefs.GetInt("TaskIndex");

        canShowBardIntro = PlayerPrefs.GetInt("CanShowBardIntro") == 1;
        tShirts = PlayerPrefs.GetInt("TShirts");
        gotAllTshirts = PlayerPrefs.GetInt("GotAllTshirts") == 1;
        gotHotdogTshirt = PlayerPrefs.GetInt("GotHotdogTshirt") == 1;
        gotNachosTshirt = PlayerPrefs.GetInt("GotNachosTshirt") == 1;
        gotCornTshirt = PlayerPrefs.GetInt("GotCornTshirt") == 1;
        gotPizzaTshirt = PlayerPrefs.GetInt("GotPizzaTshirt") == 1;
        gotChiliTshirt = PlayerPrefs.GetInt("GotChiliTshirt") == 1;
        gotSushiTshirt = PlayerPrefs.GetInt("GotSushiTshirt") == 1;
        canLoadFinalScene = PlayerPrefs.GetInt("CanLoadFinalScene") == 1;

        nachosUnlockedShown = PlayerPrefs.GetInt("NachosUnlockedShown", 0) == 1;
        cornUnlockedShown = PlayerPrefs.GetInt("CornUnlockedShown", 0) == 1;
        pizzaUnlockedShown = PlayerPrefs.GetInt("PizzaUnlockedShown", 0) == 1;
        chiliUnlockedShown = PlayerPrefs.GetInt("ChiliUnlockedShown", 0) == 1;
        sushiUnlockedShown = PlayerPrefs.GetInt("SushiUnlockedShown", 0) == 1;
        canShowWimbledonDialogue = PlayerPrefs.GetInt("CanShowWimbledonDialogue", 0) == 1;
        walkingDog = PlayerPrefs.GetInt("WalkingDog", 0) == 1;
        canShowNewItemNotification = PlayerPrefs.GetInt("CanShowNewItemNotification", 0) == 1;
        sushiUnlockedShown = PlayerPrefs.GetInt("SushiUnlockedShown", 0) == 1;
        canStartDay = PlayerPrefs.GetInt("CanStartDay", 0) == 1;

        walkiesCountdownStart = PlayerPrefs.GetFloat("WalkiesCountdownStart", 20f);
        bardMyspaceTarget = PlayerPrefs.GetFloat("BardMyspaceTarget", 1000f);
        canTurnOnUIscreen = PlayerPrefs.GetInt("CanTurnOnUIScreen", 1) == 1;
        canToggleUI = PlayerPrefs.GetInt("CanToggleUI", 0) == 1;
        bardDeaths = PlayerPrefs.GetInt("BardDeaths", 0);
        inSaffronWalden = PlayerPrefs.GetInt("InSaffronWalden", 0) == 1;
        canShowNewItemNotification = PlayerPrefs.GetInt("CanShowNewItemNotification", 1) == 1;
        canStartDay = PlayerPrefs.GetInt("CanStartDay", 0) == 1;
        canShowBardIntro = PlayerPrefs.GetInt("CanShowBardIntro", 1) == 1;
    
    Debug.Log("Game Loaded!");

        Debug.Log("Game Loaded!");

        SceneManager.LoadScene("House");
    }
    void UpdateSlider()
    {
        // Set slider's value based on the tug-of-war progress
        float totalProgress = finalePlayerCounter + bardEatenCount;
        float playerProgress = Mathf.Clamp01(finalePlayerCounter / totalProgress);

        finaleSlider.value = playerProgress;
        Debug.Log(playerProgress);
        // Update handle icon and slider fill color
     //   if (playerProgress > 0.5f)
    //   {
         //   finaleSlider.fillRect.GetComponent<Image>().color = Color.red; // Player's color
     //      finaleSlider.handleRect.GetComponent<Image>().sprite = playerIcon.sprite;
      //  }
     //   else
        //{
       //     finaleSlider.fillRect.GetComponent<Image>().color = Color.blue; // Bard's color
        //   finaleSlider.handleRect.GetComponent<Image>().sprite = bardIcon.sprite;
        //}
    }
    public void NewGame()
    {
      //  mainMenu.GetComponent<Animator>().enabled = true;
        mainMenu.SetActive(false);
    //    Time.timeScale = 0;
    }

}
