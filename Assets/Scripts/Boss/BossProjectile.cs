using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    private GameObject player;
    private Rigidbody playerRB;
    private SphereCollider AOE;
    private BossController boss;

    private Vector3 target;
    private bool hit = false;
    private float impactTime = 0.1f;

    public float speed;
    public float AOEsize;
    public float launchforce;

    // Start is called before the first frame update
    void Start()
    {
        AOE = GetComponent<SphereCollider>();
        AOE.radius = AOEsize;

        //Finds player in game and records their position when spawned
        player = GameObject.Find("Player");
        playerRB = GameObject.Find("Player").GetComponent<Rigidbody>();
        boss = GameObject.Find("Boss").GetComponent<BossController>();
        target = player.transform.position;

        speed = boss.projectileSpeed;
        AOEsize = boss.AOEsize;
        launchforce = boss.projectileLaunchForce;
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
        Vector3 heading = target - player.transform.position;

        if (heading == new Vector3(0, 0, 0))
        {
            heading = boss.transform.position - player.transform.position;
        }

        var distance = heading.magnitude;
        Vector3 direction = heading / distance; // This is now the normalized direction.

        Debug.Log(direction);

        /*if (direction.x > 0 && direction.x < 1)
        {
            direction.x = 1f;
        }
        if (direction.x < 0 && direction.x > -1)
        {
            direction.x = -1f;
        }
        if (direction.z > 0 && direction.z < 1)
        {
            direction.z = 1f;
        }
        if (direction.z < 0 && direction.z > -1)
        {
            direction.z = -1f;
        }*/

        if (direction.y < -0.5f)
        {
            direction.y = -0.5f;
        }

        playerRB.velocity = new Vector3(playerRB.velocity.x, 0f, playerRB.velocity.z);
        playerRB.AddForce((-direction + new Vector3(0, 0.3f, 0)) * launchforce, ForceMode.Impulse);

        Debug.Log(direction);
    }

    IEnumerator explode()
    {
        yield return new WaitForSeconds(impactTime);

        Destroy(gameObject);
    }
}
