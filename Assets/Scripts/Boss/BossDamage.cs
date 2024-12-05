using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BossDamage : MonoBehaviour
{
    private BossController boss;
    public Win_Lose win_lose;

    private GameObject player;
    private Rigidbody playerRB;

    public float launchforce;

    public float bufferTime;

    private bool canDamage = true;

    public Image health1;
    public Image health2;
    public Image health3;

    public Sprite unwrapped;

    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.Find("Boss").GetComponent<BossController>();

        player = GameObject.Find("Player");
        playerRB = GameObject.Find("Player").GetComponent<Rigidbody>();
        win_lose = GameObject.Find("Player").GetComponent<Win_Lose>();
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

            if (boss.health == 2)
            {
                health3.sprite = unwrapped;
            }
            if (boss.health == 1)
            {
                health2.sprite = unwrapped;
            }
            if (boss.health == 0)
            {
                health1.sprite = unwrapped;

                win_lose.callWin();
            }

            if (boss.health > 0)
            {
                canDamage = false;
                StartCoroutine(damageBuffer());

                Vector3 heading = new Vector3(0, 0, 0) - player.transform.position;
                var distance = heading.magnitude;
                Vector3 direction = heading / distance;
                playerRB.velocity = new Vector3(playerRB.velocity.x, 0f, playerRB.velocity.z);
                playerRB.AddForce((-direction + new Vector3(0, 0, 0)) * launchforce, ForceMode.Impulse);
                //Debug.Log("Buffer:" + direction);
            }
        }
    }

    IEnumerator damageBuffer()
    {
        yield return new WaitForSeconds(bufferTime);

        canDamage = true;
        //Debug.Log("Damage again");
    }
}
