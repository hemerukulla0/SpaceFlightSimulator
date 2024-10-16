using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerControl : MonoBehaviour //Even though this script is called timercontrol, it is really a stopwatch
{
    public int hours;
    public int minutes;
    public int seconds;

    public TMP_Text StopWatchTimer;
    float Timer = 0f;

    void Update()
    {

        Timer += Time.deltaTime; //The value of the time elapsed is incremented by the time between frame executions

        seconds = (int)(Timer % 60); //shows how the time is converted into seconds
        minutes = (int)(Timer / 60) % 60; //shows how the time is converted into minutes
        hours = (int)(Timer / 3600) % 24; //shows how the time is converted into hours

        string timerString = "T + " + string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds); //output message is formatted in this way
        
        StopWatchTimer.text = timerString; //The output text value is updated by the string

    }
}
