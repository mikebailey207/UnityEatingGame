using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontKillPauseScreen : MonoBehaviour
{
    private static DontKillPauseScreen instance;
    // Start is called before the first frame update
    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Make this GameObject persistent across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
}
