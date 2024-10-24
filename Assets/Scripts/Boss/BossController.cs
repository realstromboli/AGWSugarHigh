using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject player;
    private bool isDefeated = false;

    [Header("Boss Movement")]
    public float turnRate = 1;
        Vector3 toTarget;
        Quaternion playerPos;

    [Header("Boss Attack")]
    private bool isAggro = false;    
    public float attackRate;
    public float aggroLength;
    public float tillAttack;
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
            transform.rotation = new Quaternion(0.417919606f, 0.610811353f, 0.492304474f, 0.45813641f);
            transform.position = new Vector3(0, 1.92999995f, 0);
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
