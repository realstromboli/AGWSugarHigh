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

    public Image bg1;
    public Image bg2;

    public Color defaultColor;
    public Color wallRunColor;
    public Color grappleColor;
    public Color swingColor;

    //Time Number
    private float time;
    //Timer On or Off
    private bool timerOn = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.wallrunPowerActive == true)
        {
            abilityText.text = "Ability: Wall Run";
            abilityDuration.text = string.Format("{00}", player.powerupDuration) + " seconds";
            bg1.color = wallRunColor;
            bg2.color = wallRunColor;
        }
        else if (player.grapplePowerActive == true)
        {
            abilityText.text = "Ability: Grapple";
            abilityDuration.text = string.Format("{00}", player.powerupDuration) + " seconds";
            bg1.color = grappleColor;
            bg2.color = grappleColor;
        }
        else if (player.swingPowerActive == true)
        {
            abilityText.text = "Ability: Swing";
            abilityDuration.text = string.Format("{00}", player.powerupDuration) + " seconds";
            bg1.color = swingColor;
            bg2.color = swingColor;
        }
        else
        {
            abilityText.text = "Ability: None";
            abilityDuration.text = " ";
            bg1.color = defaultColor;
            bg2.color = defaultColor;
        }

        if (timerOn)
        {
            time -= Time.deltaTime;
            updateTimer(time);
        }

        //seconds = (int)(time % 60);
        //minutes = (int)(time / 60);
    }

    void updateTimer(float currentTime)
    {

    }
}
