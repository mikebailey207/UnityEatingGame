using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSettings : MonoBehaviour
{

    public TextMeshProUGUI choiceText, pleaseChooseText;
    public AudioSource[] fartSounds; // Array of fart sounds to choose from
    public AudioSource music;
    public GameObject confirmButton, backButton, settingsScreen, car, disclaimerScreen;
    

    void Start()
    {
        int i = Random.Range(0, GameManager.Instance.fartNames.Length);
        pleaseChooseText.text = "Please choose your favourite " + GameManager.Instance.fartNames[i] + " sound";
        music.Stop();
        choiceText.text = "NO SOUND SELECTED";

        Time.timeScale = 0;

    }
    private void Update()
    {
       
    }
    public void TurnOffDisclaimer()
    {
        disclaimerScreen.SetActive(false);
    }

    public void ClassicFartSound()
    {
        int i = Random.Range(0, GameManager.Instance.fartNames.Length - 1);


        SelectFartSound(0);
        choiceText.text = "'CLASSIC "+ GameManager.Instance.fartNames[i] + " SOUND'";
    }

    public void SqueakyFartSound()
    {
        int i = Random.Range(0, GameManager.Instance.fartNames.Length - 1);
        SelectFartSound(1);
        choiceText.text = "'ETHER " + GameManager.Instance.fartNames[i] + " SOUND'";
    }
    public void CosmicFartSound()
    {
        int i = Random.Range(0, GameManager.Instance.fartNames.Length - 1);
        SelectFartSound(2);
        choiceText.text = "'ROCK STAR " + GameManager.Instance.fartNames[i] + " SOUND'";
    }
    public void ClownFartSound()
    {
        int i = Random.Range(0, GameManager.Instance.fartNames.Length - 1);
        SelectFartSound(3);
        choiceText.text = "'CLOWN " + GameManager.Instance.fartNames[i] + " SOUND'";
    }
    public void AlienFartSound()
    {
        int i = Random.Range(0, GameManager.Instance.fartNames.Length - 1);
        SelectFartSound(4);
        choiceText.text = "'ALIEN " + GameManager.Instance.fartNames[i] + " SOUND'";
    }
    public void FurutreBreezeFartSound()
    {
        int i = Random.Range(0, GameManager.Instance.fartNames.Length - 1);
        SelectFartSound(5);
        choiceText.text = "'FUTURE BREEZE " + GameManager.Instance.fartNames[i] + " SOUND'";
    }

    void SelectFartSound(int index)
    {
        fartSounds[index].Play();
        GameManager.Instance.fartSound = fartSounds[index];
        PlayerPrefs.SetInt("SelectedFartSoundIndex", index);
        PlayerPrefs.Save();

        // Make the selected fart sound persistent
        DontDestroyOnLoad(GameManager.Instance.fartSound.gameObject);

        confirmButton.SetActive(true);
     //   backButton.SetActive(true);
    }

    public void Confirm()
    {
        
        music.Play();
        car.SetActive(true);
        Time.timeScale = 1;
        settingsScreen.SetActive(false);
    }

    public void Back()
    {
        confirmButton.SetActive(false);
        backButton.SetActive(false);
    }
}
