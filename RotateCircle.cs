using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCircle : MonoBehaviour
{
    public GameObject playerHand;
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.boughtLead)
        {
            transform.localScale = new Vector3(.5f, .5f, .5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerHand.transform.position;
        transform.Rotate(0, 0, 20 * Time.deltaTime);    
        
    }
}
