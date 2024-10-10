using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Win_Lose : MonoBehaviour
{
    public int minDeath = -10;
    bool gameEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gameEnd)
            {
                //Stops game movement
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
                
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
    }

    private void callWin ()
    {
        //Marks game as ended
        gameEnd = true;

        //Stops game movement
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;

        //Debug
        Debug.Log("Win!!");
    }

    private void callLose()
    {
        //Marks game as ended
        gameEnd = true;

        //Stops game movement
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;

        //Debug
        Debug.Log("Lose!");
    }
}
