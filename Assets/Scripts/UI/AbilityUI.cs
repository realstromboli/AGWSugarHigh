using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    private PlayerMovement player;

    //Text Visuals
    public TextMeshProUGUI abilityText;
    public TextMeshProUGUI abilityDuration;

    /*public Image bg1;
    public Image bg2;

    public Color defaultColor;
    public Color wallRunColor;
    public Color grappleColor;
    public Color swingColor;*/

    //Time Number
    public float time;
    //Timer On or Off
    private bool timerOn = false;

    int seconds;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        abilityText = GameObject.Find("Ability").GetComponent<TextMeshProUGUI>();
        abilityDuration = GameObject.Find("Ability Duration").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.wallrunPowerActive == true)
        {
            abilityText.text = "Ability: Wall Run";
            timerOn = true;
            time = player.powerupDuration;

        }
        else if (player.grapplePowerActive == true)
        {
            abilityText.text = "Ability: Grapple";
            timerOn = true;
            time = player.powerupDuration;
        }
        else if (player.swingPowerActive == true)
        {
            abilityText.text = "Ability: Swing";
            timerOn = true;
            time = player.powerupDuration;
        }
        else
        {
            abilityText.text = "Ability: None";
        }

        if (timerOn)
        {
            time -= Time.deltaTime;
            updateTimer(time);
        }
        else
        {
            abilityDuration.text = " ";
        }

        seconds = (int)(time % 60);
    }

    void updateTimer(float currentTime)
    {
        abilityDuration.text = seconds + " seconds";
    }
}
