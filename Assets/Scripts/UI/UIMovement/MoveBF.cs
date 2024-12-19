using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBF : MonoBehaviour
{
    public float moveSpeed = 0;

    public Vector3 pointA;
    public Vector3 pointB;

    // Start is called before the first frame update
    void Start()
    {
        pointA = pointA + transform.position;
        pointB = pointB + transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float time = Mathf.PingPong(Time.time * moveSpeed, 1);
        transform.position = Vector3.Lerp(pointA, pointB, time);
    }
}
