using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    //Time Number
    public float time;

    //Timer On or Off
    public bool timerOn = false;
    //Timer counts up (best time) or counts down (run down clock)
    public bool timerUp = false;
    
    public bool worldTimer = true;

    //Timer Visuals
    public TextMeshProUGUI timeText;
    private int minutes = 0;
    private int seconds = 0;

    // Start is called before the first frame update
    void Start()
    {
        timerOn = true;

        if (worldTimer)
        {
            timeText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn)
        {
            if (timerUp)
            {
                time += Time.deltaTime;
                updateTimer(time);
            }
            else
            {
                time -= Time.deltaTime;
                updateTimer(time);
            }
        }

        seconds = (int)(time % 60);
        minutes = (int)(time / 60);
    }

    void updateTimer(float currentTime)
    {
        timeText.text = "Time: " + string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}
