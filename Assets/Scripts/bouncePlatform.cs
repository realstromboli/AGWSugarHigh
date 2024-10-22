using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouncePlatform : MonoBehaviour
{
    //   BOUNCE
    private Rigidbody rb;

    public float bounceHeight = 18;

    // Start is called before the first frame update
    void Start()
    {
        //Finds character controller script
        rb = GameObject.Find("Player").GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
            //Launches player up
        //resets y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * bounceHeight, ForceMode.Impulse);
    }
}
