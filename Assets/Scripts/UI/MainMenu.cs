using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private bool[] levelsUnlocked = {true, true, false, false, false, false};

    public void StartGame()
    {
        //Change to game scene
        SceneManager.LoadScene("Level 1");
    }

    public void QuitGame()
    {
        //Closes the game
        #if UNITY_STANDALONE
        Application.Quit();
        #endif
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void OpenLevelSelect()
    {
        //Change to level select menu
        SceneManager.LoadScene("Level Select");
    }

    public void OpenTitle()
    {
        //Change to level select menu
        SceneManager.LoadScene("Title");
    }

    public void OpenBossScene()
    {
        //Change to boss sample level
        SceneManager.LoadScene("SampleBoss");
    }

    public void OpenSandbox()
    {
        //Change to movement sandbox level
        SceneManager.LoadScene("SampleScene");
    }

    public void LevelSelect(int level)
    {
        //Change to selected level
        switch (level)
        {
            case 1:
                if(levelsUnlocked[1] == true)
                {
                    SceneManager.LoadScene("Level 1");
                }
                break;
            case 2:
                if (levelsUnlocked[2] == true)
                {
                    SceneManager.LoadScene("Level 2");
                }
                break;
            case 3:
                if (levelsUnlocked[3] == true)
                {
                    SceneManager.LoadScene("Level 3");
                }
                break;
            case 4:
                if (levelsUnlocked[4] == true)
                {
                    SceneManager.LoadScene("Level 4");
                }
                break;
            case 5:
                if (levelsUnlocked[5] == true)
                {
                    SceneManager.LoadScene("Level 5");
                }
                break;
        }
        
    }
}
