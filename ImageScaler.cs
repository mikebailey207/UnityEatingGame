using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ImageScaler : MonoBehaviour
{
    
    bool started = false;
    public GameObject disclaimer;
    public AudioSource launchsound;

    AudioSource beep;
    bool canShrink = true;
    public TextMeshProUGUI configText;
    public float scaleStep = 0.05f;  // Amount to scale up or down
    public GameObject button, title;
    private void Start()
    {
        configText.text = "Adjust with A and D until Mavis is barely visible";
        beep = GetComponent<AudioSource>();
        Invoke("GetStarted", 12);
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            CancelInvoke("GetStarted");
            GetStarted();
        }
        if (started)
        {
            // Increase size with right arrow
            if (Input.GetKeyDown(KeyCode.D))
            {
                transform.localScale += new Vector3(scaleStep, scaleStep, 0);
            }
            // Decrease size with left arrow
            else if (Input.GetKeyDown(KeyCode.A) && canShrink)
            {
                transform.localScale -= new Vector3(scaleStep, scaleStep, 0);
            }
            if (transform.localScale.x <= 0.1f && canShrink)
            {
                beep.Play();
                title.SetActive(false);
                configText.text = "Great, thanks.";
                Invoke("LoadButtonEtc", 2);
                canShrink = false;

            }
        }

    }
    void GetStarted()
    {
        disclaimer.SetActive(false);
        started = true;
    }
    void LoadButtonEtc()
    {
        SceneManager.LoadScene("ExteriorStart");
       // configText.text = "So do you want to play the game?";
       // button.SetActive(true);
    }
    public void SimpleStart()
    {
        SceneManager.LoadScene("ExteriorStart");
    }
    public void StartTheGame()
    {
        button.SetActive(false);
        GetComponent<Image>().enabled = false;
        launchsound.Play();
        StartCoroutine(StartTheGameEnum());
    }

    IEnumerator StartTheGameEnum()
    {
        configText.text = "Ok no worries one sec..";
        yield return new WaitForSeconds(3);
        configText.text = "Here we go";
        yield return new WaitForSeconds(2);
        configText.text = "Buckle UP!";
        yield return new WaitForSeconds(2);
        configText.text = "Ah wait..";
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("ExteriorStart");
    }
}
