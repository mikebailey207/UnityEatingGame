using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FirstTimeCarText : MonoBehaviour
{
    public GameObject TextObject;
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.Instance.day == 2 && GameManager.Instance.houseVisits==1)
        {
            StartCoroutine(ShowText());
            GameManager.Instance.talking = true;
        }
        else
        {
            GameManager.Instance.talking = false;
            Destroy(TextObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.day == 2)
        {
          if(TextObject!= null) TextObject.SetActive(false);
            GameManager.Instance.talking = false;
        }
    }
    IEnumerator ShowText()
    {
        TextObject.SetActive(true);
        yield return new WaitForSeconds(12);
        GameManager.Instance.talking = false;
        TextObject.SetActive(false);
    }
}
