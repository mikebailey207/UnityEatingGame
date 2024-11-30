using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrangeHouseHandler : MonoBehaviour
{
    public Transform offScreen;
    float speed = 1;
    private void Start()
    {
        Invoke("Kill", 3);
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, offScreen.position, speed * Time.deltaTime);
    }
    void Kill()
    {
        speed = 5;
        Invoke("ActualKill", 3);
     
    }
    void ActualKill()
    {
        Destroy(gameObject);
    }
}
