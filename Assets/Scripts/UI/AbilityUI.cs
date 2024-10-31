using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    private PlayerMovement player;
    private Timer timer;

    //Text Visuals
    public TextMeshProUGUI abilityText;
    public TextMeshProUGUI abilityDuration;

    /*public Image bg1;
    public Image bg2;

    public Color defaultColor;
    public Color wallRunColor;
    public Color grappleColor;
    public Color swingColor;*/

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        abilityText = GameObject.Find("Ability").GetComponent<TextMeshProUGUI>();
        abilityDuration = GameObject.Find("Ability Duration").GetComponent<TextMeshProUGUI>();

        timer = GameObject.Find("UI").GetComponent<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.wallrunPowerActive == true)
        {
            if(!timer.timerOn)
            {
                abilityText.text = "Ability: Wall Run";
                timer.timerOn = true;
                timer.enabled = true;
                timer.time = player.powerupDuration;
            }
        }
        else if (player.grapplePowerActive == true)
        {
            if (!timer.timerOn)
            {
                abilityText.text = "Ability: Grapple";
                timer.timerOn = true;
                timer.enabled = true;
                timer.time = player.powerupDuration;
            }
        }
        else if (player.swingPowerActive == true)
        {
            if (!timer.timerOn)
            {
                abilityText.text = "Ability: Swing";
                timer.timerOn = true;
                timer.enabled = true;
                timer.time = player.powerupDuration;
            }
        }
        else
        {
            abilityText.text = "Ability: None";
        }
    }
}
