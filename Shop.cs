using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class Shop : MonoBehaviour
{
    public AudioSource hoverSound, moneySound;
    public GameObject productInfoObject;
    public TextMeshProUGUI productInfoText;
    public TextMeshProUGUI upgradeLevelText1, upgradeLevelText2, upgradeLevelText3, upgradeLevelText4;
    float difficultyMultiplier = 0.3f;
    public GameObject maxedOut1, maxedOut2, maxedOut3, maxedOut4, leadBoughtObject, 
        trainersObject, magnumObject, subMachinegunObject, wildGunObject, upgrade1gameObject, 
        upgrade2gameObject, upgrade3gameObject, upgrade4gameObject;
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.Instance.boughtLead)
        {
            leadBoughtObject.SetActive(false);
        }
        if (GameManager.Instance.gotTrainers)
        {
            trainersObject.SetActive(false);
        }
        if (GameManager.Instance.gotMagnum && !GameManager.Instance.gotSubMachineGun && !GameManager.Instance.gotWildGun)
        {
            magnumObject.SetActive(false);
            subMachinegunObject.SetActive(true);
        }
        if(GameManager.Instance.gotSubMachineGun)
        {
            magnumObject.SetActive(false);
            subMachinegunObject.SetActive(false);
            wildGunObject.SetActive(true);
        }
        if (GameManager.Instance.gotWildGun)
        {
            magnumObject.SetActive(false);
            subMachinegunObject.SetActive(false);
            wildGunObject.SetActive(false);
        }
        if (GameManager.Instance.reputation < 119.5f)
        {
            upgrade1gameObject.SetActive(false);
            upgrade2gameObject.SetActive(false);
            upgrade3gameObject.SetActive(false);
            upgrade4gameObject.SetActive(false);
        }
        if (GameManager.Instance.drinkRefreshIndex >= 2)
        {
            maxedOut1.SetActive(true);
        }
       
        if (GameManager.Instance.biteCostIndex >= 2)
        {
            maxedOut2.SetActive(true);
        }
        if (GameManager.Instance.burpAddIndex >= 2)
        {
            maxedOut4.SetActive(true);
        }
        if (GameManager.Instance.fullNessReduceAddIndex >= 2)
        {
            maxedOut3.SetActive(true);
        }

    }

    private void Update()
    {
        upgradeLevelText1.text = GameManager.Instance.drinkRefreshIndex.ToString("0") + "/10";
        upgradeLevelText2.text = GameManager.Instance.biteCostIndex.ToString("0") + "/10";
        upgradeLevelText3.text = GameManager.Instance.fullNessReduceAddIndex.ToString("0") + "/10";
        upgradeLevelText4.text = GameManager.Instance.burpAddIndex.ToString("0") + "/10";
    }
    public void ShowProductInfoDrinkRefresh()
    {
        hoverSound.Play();
        productInfoObject.SetActive(true);
        productInfoText.text = "Increases the rate in which drinking increases stamina";
    }
    public void ShowProductInfoBiteCost()
    {
        hoverSound.Play();
        productInfoObject.SetActive(true);
        productInfoText.text = "Lose less stamina per bite";
    }
    public void ShowProductInfoFullnessReduce()
    {
        hoverSound.Play();
        productInfoObject.SetActive(true);
        productInfoText.text = "Increases how fast you generally recover";
    }
    public void ShowProductBurpAdd()
    {
        hoverSound.Play();
        productInfoObject.SetActive(true);
        productInfoText.text = "Build up more squiffle-boffs and create more space per squiffle-boff";
    }
    public void ShowProductMagnum()
    {
        hoverSound.Play();
        productInfoObject.SetActive(true);
        if (GameManager.Instance.day <= 3) productInfoText.text = "Why on earth do I need a gun?!";
        else
        productInfoText.text = "Increased fire rate. Bad ar*e";
    }
    public void ShowProductSubMachineGun()
    {
        hoverSound.Play();
        productInfoObject.SetActive(true);
        productInfoText.text = "Awesome fire rate. What Bruce willis might use";
    }
    public void ShowProductWildGun()
    {
        hoverSound.Play();
        productInfoObject.SetActive(true);
        productInfoText.text = "Bonkers gun, incinerates everything in site. For those that want a relaxing walkies";
    }
    public void ShowProductTrainers()
    {
        hoverSound.Play();
        productInfoObject.SetActive(true);
        productInfoText.text = "Click RIGHT MOUSE BUTTON to attract the dog before and during walkies";
    }
    public void ShowDogLead()
    {
        hoverSound.Play();
        productInfoObject.SetActive(true);
        productInfoText.text = "Dog keep escaping? Try this";
    }
    public void HideProductInfo()
    {
       
        productInfoObject.SetActive(false);
    }
    public void IncreaseDrinkRefresh()
    {
       
        if(GameManager.Instance.money >= 149.99f && GameManager.Instance.drinkRefreshIndex < 2 && 
            GameManager.Instance.reputation >= 119.5f)
        {
          //  GameManager.Instance.drinkRefreshMultiplier += 10f;
           GameManager.Instance.drinkRefreshMultiplier += 10f * difficultyMultiplier;
            GameManager.Instance.drinkRefreshIndex++;
            GameManager.Instance.money -= 149.99f;
            GameManager.Instance.chilliResistance += 0.02f;
            moneySound.Play();
            if(GameManager.Instance.drinkRefreshIndex >=  2)
            {
                maxedOut1.SetActive(true);
            }
        }

    }
   
    public void DecreaseBiteCost()
    {

        if (GameManager.Instance.money >= 149.99f && GameManager.Instance.biteCostIndex < 2 &&
            GameManager.Instance.reputation >= 119.5f)
        {

            // GameManager.Instance.biteCostMultiplier += 5f;
            GameManager.Instance.biteCostMultiplier += 10f * difficultyMultiplier;
            GameManager.Instance.biteCostIndex++;
            GameManager.Instance.money -= 149.99f;
            GameManager.Instance.chilliResistance += 0.03f;
            moneySound.Play();
        }
        if (GameManager.Instance.biteCostIndex >= 2)
        {
            maxedOut2.SetActive(true);
        }


    }

    public void IncreaseFullnessDecrease()
    {

        if (GameManager.Instance.money >= 149.99f && GameManager.Instance.fullNessReduceAddIndex < 2 &&
            GameManager.Instance.reputation >= 119.5f)
        {
            //  GameManager.Instance.fullNessReduceMultiplier += 5f;
            GameManager.Instance.fullNessReduceMultiplier += 5f * difficultyMultiplier;
            GameManager.Instance.fullNessReduceAddIndex++;
            GameManager.Instance.chilliResistance += 0.03f;
            GameManager.Instance.money -= 149.99f;
            moneySound.Play();
        }
        if (GameManager.Instance.fullNessReduceAddIndex >= 2)
        {
            maxedOut3.SetActive(true);
        }

    }

    public void IncreaseBurpMultiplier()
    {

        if (GameManager.Instance.money >= 149.99f && GameManager.Instance.burpAddIndex < 2 &&
            GameManager.Instance.reputation >=119.5f)
        {
            //     GameManager.Instance.burpAddMultiplier += 50f;
            GameManager.Instance.burpAddMultiplier += 50f * difficultyMultiplier;
            GameManager.Instance.burpAddIndex++;
            GameManager.Instance.chilliResistance += 0.03f;
            GameManager.Instance.money -= 149.99f;
            moneySound.Play();
        }
        if (GameManager.Instance.burpAddIndex >= 2)
        {
            maxedOut4.SetActive(true);
        }

    }

    public void BuyDogLead()
    {
        if (!GameManager.Instance.boughtLead && GameManager.Instance.money >= 199.99f)
        {
            GameManager.Instance.leadMaxLength = 14;
            GameManager.Instance.money -= 199.99f;
            leadBoughtObject.SetActive(false);
            GameManager.Instance.boughtLead = true;
            moneySound.Play();

        }
    }


    public void BuyTrainers()
    {
        if (!GameManager.Instance.gotTrainers && GameManager.Instance.money >= 39.99f)
        {
            GameManager.Instance.gotTrainers = true;
            GameManager.Instance.money -= 39.99f;
            trainersObject.SetActive(false);
            moneySound.Play();
        }

    }


    public void BuyMagnum()
    {
        if(!GameManager.Instance.gotMagnum && GameManager.Instance.money >= 149.99f)
        {
            GameManager.Instance.gunUpgrade++;
            GameManager.Instance.fireRate -= 0.3f;
            GameManager.Instance.gotMagnum = true;
            GameManager.Instance.money -= 149.99f;
            magnumObject.SetActive(false);
            subMachinegunObject.SetActive(true);
            moneySound.Play();
        }
    }
    public void BuySubMachineGun()
    {
        if (!GameManager.Instance.gotSubMachineGun && GameManager.Instance.money >= 299.99f)
        {
            GameManager.Instance.gunUpgrade++;
            GameManager.Instance.fireRate -= 0.2f;
            GameManager.Instance.gotMagnum = false;
            subMachinegunObject.SetActive(false);
            GameManager.Instance.money -= 299.9f;
            wildGunObject.SetActive(true);
            GameManager.Instance.gotSubMachineGun = true;
            moneySound.Play();
        }
    }
    public void BuyWildGun()
    {
        if (!GameManager.Instance.gotWildGun && GameManager.Instance.money >= 999.99f)
        {
            GameManager.Instance.gunUpgrade++;
            
            GameManager.Instance.gotMagnum = false;
            GameManager.Instance.gotSubMachineGun = false;
            GameManager.Instance.money -= 999.9f;
            wildGunObject.SetActive(false);
            moneySound.Play();
            GameManager.Instance.gotWildGun = true;
        }
    }

    public void LeaveShop()
    {
        if (GameManager.Instance.inSaffronWalden)
        {
            if (!GameManager.Instance.dailyTasksCompleted)
                SceneManager.LoadScene("ShopExit");
            else SceneManager.LoadScene("OutsidePub");
        }
        else if (!GameManager.Instance.inSaffronWalden)
        {
            GameManager.Instance.beenShopping = true;
            if (GameManager.Instance.dailyTasksCompleted)
            {
                SceneManager.LoadScene("GardenExit");
            }
            else if(!GameManager.Instance.dailyTasksCompleted)
            {
                SceneManager.LoadScene("ExteriorHouse");
            }
     
        }
    }
}
