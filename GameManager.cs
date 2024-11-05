using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool dailyTasksCompleted;
    public bool metDoris;
    public static GameManager Instance;
    public bool wearingDisguise;
    public bool walkiesComplete;

        
    public int day = 1;
    public int houseVisits = 0;
    public string[] taskTexts;
    public int taskIndex;
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
        walkiesComplete = false;
    }
  



}
