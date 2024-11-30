using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    Animator anim;
    bool canAnimate = true;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canAnimate)
        {
            StartCoroutine(AnimateEnum());
        }
    }
    IEnumerator AnimateEnum()
    {
        anim.SetTrigger("Hit");
        canAnimate = false;
        yield return new WaitForSeconds(1);
        canAnimate = true;
    }
}
