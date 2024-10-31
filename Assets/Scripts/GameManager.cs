using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private PlayerMovement player;
    public Respawn respawn;

    public int deathCount = 0;

    private void Start()
    {
        respawn = GetComponent<Respawn>();
    }

    public void callRespawn()
    {
        respawn.respawn();
        Debug.Log("called");
    }

}
