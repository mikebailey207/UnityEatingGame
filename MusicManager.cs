using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource, runMusic;

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
        if (scene.name == "FoodChallenge")
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