using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnFaxAnim : MonoBehaviour
{
    Animator anim;
    public Animator otherAnim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Invoke("WaitFor", 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void WaitFor()
    {
        anim.enabled = true;
        otherAnim.enabled = true;
    }
}
