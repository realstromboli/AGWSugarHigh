using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseScreen;
    

    [Header("Game Status")]
    //Game Management
    public int deathCount = 0;
    public float[] gradeTimes;

    private bool isPaused = false;
    public bool gameEnd = false;


    private void Start()
    {
        pauseScreen = GameObject.Find("Pause Screen");
        pauseScreen.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    void Update()
    {
        //Pauses if Escape is pressed
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

    //Pauses Game
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

    //Resumes Game
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

    //Restarts Level by Resetting Scene
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }
    
    //Goes to level select screen
    public void gotoLevelSelect()
    {
        SceneManager.LoadScene("Level Select");
    }

    //Goes to level select screen
    public void gotoTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void nextLevel(int nextLevel)
    {
        SceneManager.LoadScene("Level " + nextLevel.ToString());
    }

    //Shuts the game down
    public void QuitGame()
    {
        #if UNITY_STANDALONE
        Application.Quit();
        #endif
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
