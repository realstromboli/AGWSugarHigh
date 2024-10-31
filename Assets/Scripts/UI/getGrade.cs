using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class getGrade : MonoBehaviour
{
    public TextMeshProUGUI gradeText;
    public TextMeshProUGUI timeText;

    public Timer timer;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gradeText = GameObject.Find("Grade").GetComponent<TextMeshProUGUI>();
        timeText = GameObject.Find("Final Time").GetComponent<TextMeshProUGUI>();
        
        timer = GameObject.Find("Game Manager").GetComponent<Timer>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = "Final " + timer.timeText.text;

        if (timer.time <= gameManager.gradeTimes[0])
        {
            gradeText.text = "A";
        }
        else if (timer.time <= gameManager.gradeTimes[1])
        {
            gradeText.text = "B";
        }
        else if (timer.time <= gameManager.gradeTimes[2])
        {
            gradeText.text = "C";
        }
        else
        {
            gradeText.text = "D";
        }
    }
}
