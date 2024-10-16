using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Console;

public class CountdownTimerControl : MonoBehaviour
{
    public GameObject CountdownTimer;
    public GameObject StopwatchTimer;

    public GameObject Rocket;

    public float countdownTime = 10f; //This sets the start time of the countdown, which is 10 seconds in this case
    public TMP_Text countdown;

    public bool executeOnce = false; 


    private void Start()
    {
        CountdownTimer.SetActive(true); //Once the script is initalized, the countdown timer gameobject is set to visible
        Rocket = FindObjectOfType<RocketMain>().gameObject; //Refrence to the spacecraft gameObject
        
    }
    void Update()
    {
        if (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime; //While the current remaining time is greater than 0, the countdown time is decreased by the time interval between frames
        }
        else
        {
            countdownTime = 0;
        }

        if (countdownTime < 3) //If the time is below 3 seconds, then the ignition of the rocket is set to active, mimics what happens in real spacecraft launches
        {
            if (!executeOnce)
            {
                for (int i = 0; i < Rocket.GetComponent<RocketMain>().Stage1Flames.Length; i++)
                {
                    Rocket.GetComponent<RocketMain>().Stage1Flames[i].SetActive(true); //sets the flames of stage 1 to active
                }
                InputConsole.AddStaticMessageToInputConsole("   Ignition Active"); //outputs a message stating that ignition has started
                executeOnce = true;
            }

            
        }

        if (countdownTime == 0) //When the countdown timer reaches zero then the countdown gameobject is deactivated, and the stopwatch gameobject is activated
        {
            CountdownTimer.SetActive(false);
            StopwatchTimer.SetActive(true);
            Vector3 NewPos = new Vector3(-0.77f, 0.001f, 0); 
            Rocket.GetComponent<RocketMain>().Rocket.transform.position = NewPos; //The position of the rocket is set to a very low height, so that the rocket main script is triggered


        }

        DisplayTime(countdownTime); //Calls the method that displays the output to the user
    }

    void DisplayTime(float Time)
    {
        if(Time < 0)
        {
            Time = 0;
        }

        float hours = 0;
        float minutes = Mathf.FloorToInt(Time / 60); //how the time is converted into minutes, and rounded to an integer
        float seconds = Mathf.FloorToInt(Time % 60); //how the time is converted into seconds, and rounded to an integer

        countdown.text = "T - "  + string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds); //How the countdonw timer is displayed
    }
}
