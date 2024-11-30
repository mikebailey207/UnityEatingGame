using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPizzaAnim : MonoBehaviour
{
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        if (!GameManager.Instance.gotPizzaTshirt)
            Invoke("EnableAnim", 3);
        else if (GameManager.Instance.gotPizzaTshirt) anim.enabled = false;
    }
    void EnableAnim()
    {
        anim.enabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
