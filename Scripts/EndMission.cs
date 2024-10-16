using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMission : MonoBehaviour
{
    public RocketMain manager; //Reference to the other scripts
    public GameObject timeController;
    public TimerControl stopwatch;

    public GameObject moon;
    public GameObject floor;


    [SerializeField] public GameObject OverallUI; //The UI elements have been stored in these variables
    [SerializeField] public GameObject SimulationUI;
    [SerializeField] public GameObject SuccessUI;
    [SerializeField] public GameObject FailUI;


    [Header("Success UI")]
    public TMP_Text AverageVelocityText;
    public TMP_Text TimeTakenText;
    public TMP_Text ScoreText;


    [Header("Fail UI")]
    public TMP_Text FailReason;

    public string scoreEntry;


    public GameObject timeScaleController;


    private void Awake()
    {
        manager = FindObjectOfType<RocketMain>(); //The gameobjects containing the scripts are loaded in here
        timeScaleController = FindObjectOfType<MainTimeScaleController>().gameObject;

        
    }

    private void OnCollisionEnter2D(Collision2D collision) //This method detects whether a collision has been detected
    {

        if (collision.gameObject == moon) 
        {

            manager.throttlePercentage = 0;

            manager.hasLanded = true;



            timeScaleController.GetComponent<MainTimeScaleController>().modifyScale = 0; //When the spacecraft lands on the moon, the timescale is set to zero to stop the spacecraft from moving any more
           

            if (manager.Yvelocity < 5000 && manager.currentActiveStage ==4) //The mission is only a success if the impact velocity of the spacecraft is below 5000 meters per second
            {
                SimulationUI.SetActive(false); //These lines set the UI for the main simulation to be disabled
                OverallUI.SetActive(true);
                SuccessUI.SetActive(true); //This line will enable the success UI since the spacecraft has landed with acceptable velocity
                FailUI.SetActive(false);


                ScoreText.text = "Score: " + CalculateScore(stopwatch.hours, stopwatch.minutes).ToString(); //This line calculated the score and also displays it to the user

                int timeInSeconds = (stopwatch.hours * 3600) + (stopwatch.minutes * 60) * stopwatch.seconds; //This line converts the time taken into seconds

                float averageVelocity = (RocketMain.distanceToMoon / timeInSeconds); //Average velocity is calculated based on the time taken in seconds and the total distance to the moon

                TimeTakenText.text = "Time Taken:" + string.Format("{0} Hours {1} Minutes {2} Seconds", stopwatch.hours, stopwatch.minutes, stopwatch.seconds); //This command outputs the time taken to the user

                AverageVelocityText.text = "Average Velocity: " + Math.Round(averageVelocity).ToString(); //Average velcotiy calculation is displayed to the user after being rounded



                
            }
            else //This part will execute if the mission has failed for any reason
            {
                SimulationUI.SetActive(false);
                OverallUI.SetActive(true);
                SuccessUI.SetActive(false);
                FailUI.SetActive(true);
                if (manager.currentActiveStage != 4) //This statement will appear if the current active stage is not the lander
                {
                    FailReason.text = "Reason: You did not switch to the lander, hint use the separate stage command before you reach the moon to switch to the laner";
                }
                else //This statement will appear if the velocity of the user exceeds 5000 meters per second
                {
                    FailReason.text = "Reason: Your impact velocity was too high, hint - try and reduce your velocity before you land";
                }
                
            }
            
        }

        if (collision.gameObject == floor) //This code will execute if the user crashed onto the earth for some reason
        {
            SimulationUI.SetActive(false);
            OverallUI.SetActive(true);
            SuccessUI.SetActive(false);
            FailUI.SetActive(true);
            Debug.Log("Failed");
            FailReason.text = "Reason: You crashed onto the floor, hint - Set throttle level higher"; //Reason for this mission failure
        }
    }

    public void OnSaveButtonPress() //the code that will execute once the user presses the save button
    {

        if (LoginSystem.username == "" || LoginSystem.username == null) //Ensures that if a username is not set a defualt value is available
        {
            LoginSystem.username = "NoVal";
        }
        scoreEntry = LoginSystem.username + ":" + CalculateScore(stopwatch.hours, stopwatch.minutes).ToString(); //Adds the score to the username in the correct syntax

        
        LeaderboardManager.EnterNewValue(scoreEntry); //This command stores the new entry into the leaderboard text file

        LeaderboardManager.isUpdated = true; //The leaderboard will only update if this value is true
    }

    public void OnReturnButtonPress() //Upon pressing the return button, the user is returned to the main menu
    {
        SceneManager.LoadScene("MainMenu");
    }

   
    private int CalculateScore(int hours, int minutes) //Method that calculates the user's score
    {
        int combinedTime = Convert.ToInt32(string.Format("{0}{1}", hours, minutes));
        int score = 10000 - combinedTime; //The score is a fixed value of 10000 take away the combined hours + minutes of the user.

        return score;

    }
}
