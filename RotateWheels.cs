using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWheels : MonoBehaviour
{
    bool moving = true;
 
    // Start is called before the first frame update
    void Start()
    {
       
        Invoke("StopAnimating", 5);
    }

    // Update is called once per frame
    void Update()
    {
        if(moving)
        transform.Rotate(0, 0, -200 * Time.deltaTime);
    }
    void StopAnimating()
    {
        moving = false;
    }
}
