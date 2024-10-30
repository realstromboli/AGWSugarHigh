using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject player;
    private bool isDefeated = false;

    [Header("Boss Movement")]
    public float turnRate = 1;
    private Vector3 toTarget;
    private Quaternion playerPos;

    [Header("Boss Attack")]
    public float attackRate;
    public float aggroLength;
    public float tillAttack;
    private bool isAggro = false;
    public GameObject angryEyes;
    public GameObject projectile;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        InvokeRepeating("aggro", attackRate, attackRate);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDefeated)
        {
            //Boss continuously looks at the player. With dampening to feel more natural
            toTarget = player.transform.position - transform.position;
            playerPos = Quaternion.LookRotation(toTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, playerPos, Time.deltaTime / turnRate);
        }
        else
        {
            CancelInvoke("aggro");

            //Temp Dead Position
            transform.rotation = new Quaternion(0.4f, 0.6f, 0.5f, 0.4f);
            transform.position = new Vector3(0, 1.9f, 0);
        }

        //Temp kill switch
        if (Input.GetKeyDown(KeyCode.K))
        {
            isDefeated = true;
        }
    }

    void aggro()
    {
        if (isAggro != true)
        {
            angryEyes.SetActive(true);
            isAggro = true;
            
            StartCoroutine(attack());
            StartCoroutine(endAggro());
        }
    }

    IEnumerator attack()
    {
        yield return new WaitForSeconds(tillAttack);

        Instantiate(projectile, transform.position, transform.rotation);

    }

    IEnumerator endAggro()
    {
        yield return new WaitForSeconds(aggroLength);

        angryEyes.SetActive(false);
        isAggro = false;

    }

}
