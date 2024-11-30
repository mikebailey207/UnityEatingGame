using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontKillFinaleMusic : MonoBehaviour
{
    private static DontKillFinaleMusic instance;

    private void Awake()
    {
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
    
}
