using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTimeController : MonoBehaviour
{
    public bool toggleActive = true;

    public GameObject Stopwatch;
    public GameObject Countdown;

    public string script = "TimerControl"; //The names of the scripts that are are enabled upon the toggle being changed
    public string script2 = "CountdownTimerControl";



    void Update() //This method sets the current script active, for either the countdown timer, or the stopwatch
    {
        if (Stopwatch.activeInHierarchy) 
        {
            (Stopwatch.GetComponent(script) as MonoBehaviour).enabled = true;
        }
        
        (Countdown.GetComponent(script2) as MonoBehaviour).enabled = !toggleActive; //This means that at any one time only the stopwatch, or the countdown timer can be active
        
    }
}
