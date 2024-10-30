using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject player;
    public Vector3 respawnPoint;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        player = GameObject.Find("Player");

        respawnPoint = player.transform.position;
        Debug.Log(player.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CheckPoint")
        {
            UpdateCheckpoint();
        }
    }

    public void UpdateCheckpoint()
    {
        respawnPoint = player.transform.position;
        Debug.Log("Checkpoint Reached!");
    }

    public void respawn()
    {
        Debug.Log("respawning");
        player.transform.position = respawnPoint;
    }
}
