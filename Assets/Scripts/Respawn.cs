using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject player;
    public Vector3 respawnPoint;

    private GameManager gameManager;
    private Win_Lose winLose;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        winLose = GameObject.Find("Player").GetComponent<Win_Lose>();

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

    public void loseRespawn()
    {
        //Starts game movement
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;

        respawn();

        //Resets game
        winLose.gameEnd = false;
        winLose.loseScreen.SetActive(false);
    }
}
