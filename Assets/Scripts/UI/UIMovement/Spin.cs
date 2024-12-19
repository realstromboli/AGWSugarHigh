using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public string axis;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (axis == "x")
        {
            transform.Rotate(speed * Time.deltaTime, 0f, 0f, Space.Self);
        }
        else if (axis == "y")
        {
            transform.Rotate(0f, speed * Time.deltaTime, 0f, Space.Self);
        }
        else if (axis == "z")
        {
            transform.Rotate(0f, 0f, speed * Time.deltaTime, Space.Self);
        }
        else
        {
            Debug.Log("INVALID ROTATION");
        }
    }
}
