using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    public float speed = 0;
    public float leftBound = -15;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(-1f,-1f,0f) * speed * Time.deltaTime, Space.World);

        if (transform.position.x < leftBound)
        {
            Destroy(gameObject);
        }
    }
}
