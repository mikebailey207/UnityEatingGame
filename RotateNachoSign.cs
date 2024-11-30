using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateNachoSign : MonoBehaviour
{
    LineRenderer line;
    public GameObject nachosWord;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.SetPosition(0, transform.position);
        line.SetPosition(1, nachosWord.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 50 * Time.deltaTime, 0);
        line.SetPosition(0, transform.position);
        line.SetPosition(1, nachosWord.transform.position);
    }
}
