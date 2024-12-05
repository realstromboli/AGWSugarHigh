using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    public List<GameObject> abilities;
    
    private PlayerMovement player;

    public GameObject wallRunScreen;
    public GameObject grappleScreen;
    public GameObject swingScreen;
    public GameObject dashScreen;

    // -15, -85, -155, -225

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.wallrunPowerActive)
        {
            if (wallRunScreen.activeInHierarchy == false)
            {
                wallRunScreen.SetActive(true);
                
                wallRunScreen.transform.localPosition = new Vector3(-75, -270 + 70 * abilities.Count, 0);
                abilities.Add(wallRunScreen);
            }
            else
            {
                wallRunScreen.transform.localPosition = new Vector3(-75, -270 + 70 * abilities.IndexOf(wallRunScreen), 0);
            }
        }
        
        if (player.grapplePowerActive)
        {
            if (grappleScreen.activeInHierarchy == false)
            {
                grappleScreen.SetActive(true);

                grappleScreen.transform.localPosition = new Vector3(-75, -270 + 70 * abilities.Count, 0);
                abilities.Add(grappleScreen);
            }
            else
            {
                grappleScreen.transform.localPosition = new Vector3(-75, -270 + 70 * abilities.IndexOf(grappleScreen) , 0);
            }
        }

        if (player.swingPowerActive) 
        {
            if (swingScreen.activeInHierarchy == false)
            {
                swingScreen.SetActive(true);

                swingScreen.transform.localPosition = new Vector3(-75, -270 + 70 * abilities.Count, 0);
                abilities.Add(swingScreen);
            }
            else
            {
                swingScreen.transform.localPosition = new Vector3(-75, -270 + 70 * abilities.IndexOf(swingScreen), 0);
            }
        }

        if (player.dashPowerActive)
        {
            if (dashScreen.activeInHierarchy == false)
            {
                dashScreen.SetActive(true);

                dashScreen.transform.localPosition = new Vector3(-75, -270 + 70 * abilities.Count, 0);
                abilities.Add(dashScreen);
            }
            else
            {
                dashScreen.transform.localPosition = new Vector3(-75, -270 + 70 * abilities.IndexOf(dashScreen), 0);
            }
        }
    }

    public void removefromList(string power)
    {
        switch (power)
        {
            case "wallRun":
                abilities.Remove(wallRunScreen);
                break;
            case "grapple":
                abilities.Remove(grappleScreen);
                break;
            case "swing":
                abilities.Remove(swingScreen);
                break;
            case "dash":
                abilities.Remove(dashScreen);
                break;
        }
    }
}
