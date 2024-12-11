using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject player;
    private bool isDefeated = false;

    [Header("Boss Stats")]
    public float turnRate = 1;
    private Vector3 toTarget;
    private Quaternion playerPos;

    public int health = 3;

    [Header("Boss Attack")]
    public float attackRate;
    public float aggroLength;
    public float tillAttack;
    private bool isAggro = false;
    public GameObject angryEyes;

    [Header("Melee")]
    public MeleeAttack melee;
    public float verticalLaunchForce;
    public float meleeLaunchForce;

    [Header("Projectile")]
    public GameObject projectile;
    public float projectileSpeed;
    public float AOEsize;
    public float projectileLaunchForce;

    public Animator golemAnimation;
    public GameObject golemObject;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        melee = GameObject.Find("Melee Range").GetComponent<MeleeAttack>();

        InvokeRepeating("aggro", attackRate, attackRate);

        golemAnimation = GetComponent<Animator>();

        golemAnimation.SetBool("is_dead", false);
    }

    //Vector3 golemOffset = (0, 90, 0);

    // Update is called once per frame
    void Update()
    {
        if (!isDefeated)
        {
            if (player.transform.position.y < 10)
            {
                //Boss continuously looks at the player. With dampening to feel more natural
                toTarget = player.transform.position - transform.position;
                playerPos = Quaternion.LookRotation(toTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, playerPos, Time.deltaTime / turnRate);
            }
        }
        else
        {
            CancelInvoke("aggro");

            //Temp Dead Position
            transform.rotation = new Quaternion(0.4f, 0.6f, 0.5f, 0.4f);
            transform.position = new Vector3(0, 1.9f, 0);
        }

        if (health == 0)
        {
            isDefeated = true;
            //golemAnimation.SetBool("is_dead", true);
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

        if (melee.inRange)
        {
            melee.meleeAttack();
            Debug.Log("Melee Attack");
            golemAnimation.SetBool("attack_type", true);
        }
        else
        {
            Instantiate(projectile, transform.position, transform.rotation);
            Debug.Log("Range Attack");
            golemAnimation.SetBool("attack_type", false);
        }
        golemAnimation.SetTrigger("attack_trigger");
    }

    IEnumerator endAggro()
    {
        yield return new WaitForSeconds(aggroLength);

        angryEyes.SetActive(false);
        isAggro = false;

    }
}
