using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Win_Lose : MonoBehaviour
{
    private GameManager gameManager;
    private Respawn respawn;
    private Timer timer;

    public float minDeath = -10;
    public bool gameEnd = false;
    public bool noMoreScreens = false;

    public GameObject winScreen;
    public GameObject loseScreen;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        respawn = GameObject.Find("Game Manager").GetComponent<Respawn>();
        timer = GameObject.Find("Game Manager").GetComponent<Timer>();

        winScreen = GameObject.Find("Win Screen");
        winScreen.SetActive(false);

        loseScreen = GameObject.Find("Lose Screen");
        loseScreen.SetActive(false);
        noMoreScreens = false;
    }

    // Update is called once per frame
    void Update()
    {
        //If the character falls, they die
        if (gameObject.transform.position.y < minDeath)
        {
            //Calls the lose state
            callLose();
        }

        //Resets the level once the game has ended
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (gameEnd)
            {
                //Starts game movement
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;

                respawn.respawn();

                //Resets game
                gameEnd = false;
                loseScreen.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the character makes it to the end, they win!
        if (other.gameObject.tag == "WinZone")
        {
            //Calls win state
            callWin();
        }
        if (other.gameObject.tag == "KillZone" && !noMoreScreens)
        {
            //Calls lose state
            callLose();
        }

        //Updates Respawn Point when touching Checkpoints
        if (other.tag == "CheckPoint")
        {
            respawn.UpdateCheckpoint();
        }
    }

    public void callWin()
    {
        //Marks game as ended
        gameEnd = true;

        //Stops game movement
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;

        //Debug
        Debug.Log("Win!!");

        //Shows win screen
        winScreen.SetActive(true);
    }

    private void callLose()
    {
        //Marks game as ended
        gameEnd = true;

        //Stops game movement
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;

        //Debug
        //Debug.Log("Lose!");

        //Shows lose screen
        loseScreen.SetActive(true);
        noMoreScreens = true;
    }
}
