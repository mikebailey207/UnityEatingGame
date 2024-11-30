using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaxMachineSound : MonoBehaviour
{
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        if (!GameManager.Instance.gotPizzaTshirt)
        {
            InvokeRepeating("PlaySound", 8.3f, 8f);
        }
        else if(GameManager.Instance.gotPizzaTshirt)
        {
            InvokeRepeating("PlaySound", 5.3f, 8);
        }
    }
    void PlaySound()
    {
        if(audioSource!=null)
        audioSource.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
