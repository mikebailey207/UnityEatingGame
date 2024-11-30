using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWindmill : MonoBehaviour
{
   
    void Update()
    {
        transform.Rotate(0, 0, -20 * Time.deltaTime);
    }
}
