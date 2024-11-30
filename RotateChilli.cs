using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateChilli : MonoBehaviour
{
    float zRot;

    // Start is called before the first frame update
    void Start()
    {
        zRot = Random.Range(-45, 45);    
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, zRot * Time.deltaTime);
    }
}
