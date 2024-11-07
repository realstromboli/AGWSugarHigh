using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{ 
    private GameObject player;
    private Rigidbody playerRB;
    private BossController boss;

    public bool inRange;

    private float launchForce;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerRB = GameObject.Find("Player").GetComponent<Rigidbody>();
        boss = GameObject.Find("Boss").GetComponent<BossController>();

        launchForce = boss.meleeLaunchForce;
    }

    private void Update()
    {
        launchForce = boss.meleeLaunchForce;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inRange = false;
        }
    }

    public void meleeAttack()
    {
        Vector3 heading = transform.position - player.transform.position;

        var distance = heading.magnitude;
        Vector3 direction = heading / distance; // This is now the normalized direction.

        Debug.Log(direction);
        
        //Adds launch force to direction
        direction = -direction * launchForce;

        //Fix vertical launch so player launches up every timer
        direction.y = 20f;

        playerRB.velocity = new Vector3(playerRB.velocity.x, 0f, playerRB.velocity.z);
        playerRB.AddForce(direction, ForceMode.Impulse);

        Debug.Log("Launch direction: " + direction);
    }
}
