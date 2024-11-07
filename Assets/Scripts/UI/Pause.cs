using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject pauseScreen;
    private bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else if (isPaused)
            {
                ResumeGame();
            }
                
        }
    }
    public void PauseGame()
    {
        //Shows pause menu screen
        pauseScreen.SetActive(true);
        
        //Marks game as paused
        isPaused = true;
        
        //Pauses game
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        //Hides pause menu screen
        pauseScreen.SetActive(false);

        //Marks game as not paused
        isPaused = false;

        //Resumes Game
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }
}
