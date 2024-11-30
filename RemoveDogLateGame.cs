using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveDogLateGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.reputation > 350) Destroy(gameObject);
    }

   
}
