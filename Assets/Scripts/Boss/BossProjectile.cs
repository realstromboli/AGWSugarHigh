using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    private GameObject player;
    private Rigidbody playerRB;
    private SphereCollider AOE;

    private Vector3 target;
    private bool hit = false;
    private float impactTime = 0.1f;

    public float speed;
    public float AOEsize;
    public float launchforce;

    // Start is called before the first frame update
    void Start()
    {
        /*Finds and sets AOE
        AOE = transform.GetChild(1).gameObject;
        AOE.transform.localScale = new Vector3(AOEsize, AOEsize, AOEsize);*/

        AOE = GetComponent<SphereCollider>();
        AOE.radius = AOEsize;

        //Finds player in game and records their position when spawned
        player = GameObject.Find("Player");
        playerRB = GameObject.Find("Player").GetComponent<Rigidbody>();
        target = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Checks to see if projectile has already reached its target
        if (!hit)
        {   
            //Travels toward target
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }

        //Once the projectile hits target
        if (target == transform.position)
        {
            hit = true;

            AOE.enabled = true;
            StartCoroutine(explode());

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            launchPlayer();
        }
    }

    void launchPlayer()
    {
        playerRB.velocity = new Vector3(playerRB.velocity.x, 0f, playerRB.velocity.z);
        playerRB.AddForce(transform.up * launchforce, ForceMode.Impulse);
    }

    IEnumerator explode()
    {
        yield return new WaitForSeconds(impactTime);

        Destroy(gameObject);
    }
}
