using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAllBullets : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Kill", 2);
    }
    void Kill()
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
