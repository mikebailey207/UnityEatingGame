using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCornParent : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("CornKiller"))
        {
            Destroy(gameObject);
        }
    }
}
