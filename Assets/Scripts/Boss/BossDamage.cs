using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamage : MonoBehaviour
{
    private BossController boss;

    private GameObject player;
    private Rigidbody playerRB;

    public float launchforce;

    public float bufferTime;

    private bool canDamage = true;

    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.Find("Boss").GetComponent<BossController>();

        player = GameObject.Find("Player");
        playerRB = GameObject.Find("Player").GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && canDamage)
        {
            //Takes health from boss
            boss.health--;

            Debug.Log("Boss Health: " + boss.health);


            if (boss.health > 0)
            {
                canDamage = false;
                StartCoroutine(damageBuffer());

                Vector3 heading = new Vector3(0, 0, 0) - player.transform.position;
                var distance = heading.magnitude;
                Vector3 direction = heading / distance;
                playerRB.velocity = new Vector3(playerRB.velocity.x, 0f, playerRB.velocity.z);
                playerRB.AddForce((-direction + new Vector3(0, 0, 0)) * launchforce, ForceMode.Impulse);
                Debug.Log("Buffer:" + direction);
            }
        }
    }

    IEnumerator damageBuffer()
    {
        yield return new WaitForSeconds(bufferTime);

        canDamage = true;
        Debug.Log("Damge again");
    }
}
