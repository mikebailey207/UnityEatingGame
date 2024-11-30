using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource, runMusic;
    public AudioClip mainMusicClip, townMusicClip;
    void Awake()
    {
        // Ensure there's only one instance of MusicManager across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Make this GameObject persistent across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate MusicManager instances
            return;
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        runMusic = GameObject.Find("RunMusic").GetComponent<AudioSource>();
        // Add the sceneLoaded listener
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // This function is called every time a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
         if(scene.name == "TownEntrace" || scene.name == "OutsidePub" || scene.name == "ShopExit" || scene.name == "GardenExit")
        {
            audioSource.clip = townMusicClip;
        }
         if(scene.name == "ExteriorHouse")
        {
            audioSource.clip = mainMusicClip;
        }
         if(scene.name == "House" && GameManager.Instance.dailyTasksCompleted)
        {
            audioSource.clip = townMusicClip;
        }
         else if(scene.name == "House" && !GameManager.Instance.dailyTasksCompleted)
        {
            audioSource.clip = mainMusicClip;
        }

        if (scene.name == "FoodChallenge")
        {
            audioSource.Stop();
            runMusic.Stop();
        }
        else if (scene.name == "TypingGame")
        {
            audioSource.Stop();
            runMusic.Stop();
        }
        else if (scene.name == "NachosChallenge")
        {
            audioSource.Stop();
            runMusic.Stop();
        }
        else if (scene.name == "EndSceneTown")
        {
            audioSource.Stop();
            runMusic.Stop();
        }
        else if (scene.name == "CornChallenge")
        {
            audioSource.Stop();
            runMusic.Stop();
        }
        else if (scene.name == "SushiChallenge")
        {
            audioSource.Stop();
            runMusic.Stop();
        }
        else if (scene.name == "ChiliChallenge")
        {
            audioSource.Stop();
            runMusic.Stop();
        }
        else if (scene.name == "House" && GameManager.Instance.day == 1 && GameManager.Instance.houseVisits == 0)
        {
            audioSource.Stop();
            runMusic.Stop();
        }
        else if (!audioSource.isPlaying) // Resume music if it’s not playing
        {
            audioSource.Play();
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}