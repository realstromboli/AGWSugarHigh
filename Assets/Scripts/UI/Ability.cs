using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public Timer timer;
    public string ability;
    
    private AbilityUI abilityManager;

    // Start is called before the first frame update
    void Start()
    {
        abilityManager = GameObject.Find("UI").GetComponent<AbilityUI>();
        timer = GetComponentInChildren<Timer>();
        timer.time = 30;
        timer.timerOn = true;
    }

    private void OnEnable()
    {
        timer.timerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.time <= 0)
        {
            abilityManager.removefromList(ability);

            timer.timerOn = false;
            timer.time = 30;

            gameObject.SetActive(false);
        }
    }

    public void ResetTimer()
    {
        timer.time = 30;
    }
}
