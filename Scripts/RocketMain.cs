using Console;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class RocketMain : MonoBehaviour
{
    //Initializing the variables used to store the gameobjects for the spacecraft componants
    [SerializeField] public GameObject Rocket;
    [SerializeField] public GameObject Camera;
    [SerializeField] public GameObject RocketBody;

    [SerializeField] public GameObject[] Stage1Body;
    [SerializeField] public GameObject[] Stage1Flames;
    //bool isStage1Active;

    [SerializeField] public GameObject[] Stage2Body;
    [SerializeField] public GameObject[] Stage2Flames;
    //bool isStage2Active;

    [SerializeField] public GameObject[] Stage3Body;
    [SerializeField] public GameObject[] Stage3Flames;
    //bool isStage3Active;

    [SerializeField] public GameObject[] LanderBody;
    [SerializeField] public GameObject[] LanderFLames;

    public List<GameObject> currentActiveFlames; //a list of the current flames is created, this is so that the size of the flames can be controlled based on the throttle level of the engine

    [SerializeField] GameObject FuelPivot;
    [SerializeField] public GameObject InstructionPanel; //This is the instructions screen that is activated when the appropriate command is entered
    [SerializeField] public GameObject conenctionScreen; //The refrence to the screen displaying "no connection to spacecraft"

    PhysicsCalculator calculator; //Refrence to the physics calculator class
    MainTimeScaleController timeController; //Refrence to the time class that controls the rate at which the simulation runs (allows for the simulation to speed up and slow down)
    CommandChecklist checklist; //Refrence to the list of commands that have been executed already, useful for determining whether the prerequisites have been completed


    RocketStage Stage1; //The variables used to store the information about each rocket stage
    RocketStage Stage2;
    RocketStage Stage3;
    RocketStage Lander;

    public double rocketMass; //Stores the current mass of the spacecraft


    //UI elements that display the output of the calculations and other information
    [Header("UI Elements")]
    [SerializeField]public TMP_Text AltitudeText;
    [SerializeField] public TMP_Text DistanceToMoonText;
    [SerializeField] public TMP_Text TotalMassText;
    [SerializeField] public TMP_Text AccelerationText;
    [SerializeField] public TMP_Text DeltaXText;
    [SerializeField] public TMP_Text DeltaYText;

    [SerializeField] public TMP_Text Stage1FuelText;
    [SerializeField] public TMP_Text Stage2FuelText;
    [SerializeField] public TMP_Text Stage3FuelText;
    [SerializeField] public TMP_Text LunarLanderFuelText;

    [SerializeField] public TMP_Text TemperatureText;

    [SerializeField] public TMP_Text GravitationalFieldStrengthText;

    [SerializeField] public TMP_Text ThrottleLevelText;

    [SerializeField] public TMP_Text TimeScaleText;

    [SerializeField] public TMP_Text TemporaryText;
    [SerializeField] public TMP_Text ThrustText;

    [SerializeField] public TMP_Text WelcomeText;

    //local boolean variables determining the major stages of the simulation
    public bool hasTakenOff;
    public bool hasLanded;
    //public bool isFueled;

    public string altitudeSuffix; //the string that changes the altitude units, from M to Km to Mm based on the appropriate altitude

    public double horizontalVelocity;
    public double verticalVelocity;
    public double velocity;
    public double crossSectionalArea;

    public const double rocketDryMass = 183000; //Constant that shows the weight of the spacecraft when no fuel is loaded


    public double fuelingRate;
    public int currentActiveStage;


    public double currentGravitationalFieldStrength;
    public double currentAltitude;
    public double deltaAltitude;

    public float throttlePercentage; //The variable used to store the throttle percentage of the engine entered by the user
    
    public float pitchAngle;

    public float Yvelocity;


    public double drag; //Stores the output of the drag calculation
    public double thrust; //Stores the output of the thrust calculation

    public GameObject timeScaleManager; //Refrence to the gameobject that contains the class to control the timescale of the program

    public bool hasExecutedBefore;
    public bool hasExecutedBefore2;



    public const float distanceToMoon = 3.84399e8f;  //Average distance from surface of earth to the lunar surface of the moon
    public bool executeOnce = false;


    private void Start()
    {
     
        hasExecutedBefore = false;
        hasExecutedBefore2 = false;
        hasLanded = false;


        Stage1 = new RocketStage(137000, 2077000, 0, 15890, Stage1Body, Stage1Flames, true, 78.54); //Passes the values into the constructor for each rocket stage
        Stage2 = new RocketStage(36000, 444000, 0, 1303.5, Stage2Body, Stage2Flames, true, 60.4);
        Stage3 = new RocketStage(10000, 109000, 0, 40, Stage3Body, Stage3Flames, true, 30.2);
        Lander = new RocketStage(4280, 11480, 11480, 5, LanderBody, LanderFLames, true, 5);


        //Assignes the corresponding script to each of the variables
        calculator = gameObject.AddComponent<PhysicsCalculator>(); 
        timeController = gameObject.GetComponent<MainTimeScaleController>();
        checklist = gameObject.GetComponent<CommandChecklist>();


        currentAltitude = 0; //Initializes the current altitude to 0 at the start of the program
        Vector3 startPos = new Vector3(-0.77f,0,0); //The physical starting position of the spacecraft within the unity engine
        Rocket.transform.position = startPos; //Ensures the positon of the rocket is set to its starting value
        hasTakenOff = false;
        
        //isFueled = false;
        crossSectionalArea = Stage1.crossSectionalArea; //Initially the cross sectional area, used in the drag calculation is set to that of the first rocket stage

        currentActiveStage = (int)RocketStageEnum.Stage1; //The current active stage of the rocket is obtained from the enum

        altitudeSuffix = " m";

        throttlePercentage = 0; //The throttle percentage is initially set to 0, must be set by the user by enterring a command

        timeScaleManager = FindObjectOfType<MainTimeScaleController>().gameObject;

        currentActiveFlames = Stage1Flames.ToList<GameObject>();

        WelcomeText.text = "Welcome: " + LoginSystem.username;

    }

    private void FixedUpdate() //This method runs at a rate of 60 times per second with a fixed interval between each execution
    {
        if (!hasLanded)
        {
            StartCoroutine(UpdatePosition()); //This starts a coroutine that updates the position of the spacecraft using the current velocity
        }
    }

    public void Update()  //This method runs at a rate of 60 times per second but the intervals between each execution are not constant
    {
        StartCoroutine(UpdateStats()); //This is a coroutine that updates the text elements displaying the output of the calculations

        if (currentAltitude == 0) //If the spacecraft's current altitude is zero, then this means that the boolean value can be set to zero 
        {
            hasTakenOff = false;
        }
        else
        {
            hasTakenOff = true;
        }

        rocketMass = CalculateCurrentMass(Stage1.currentFuel, Stage2.currentFuel, Stage3.currentFuel,Lander.currentFuel); //This method calls the method that returns the current total fuel of all the stages

        pitchAngle = RocketBody.transform.eulerAngles.z; //This sets the rotation of the rocket based on the value entered into the setPitch command
        currentGravitationalFieldStrength = calculator.CalculateGravitationalFieldStrength(currentAltitude); //Calls the method that returns the current gravitational field strength at the current altitude

        if (currentGravitationalFieldStrength > 0) //Once the rocket crosses the Lagrangian point, where the gravitational field strength is zero, the timescale is set back to real time (1:1)
        {
            if (!hasExecutedBefore)
            {
                timeScaleManager.GetComponent<MainTimeScaleController>().modifyScale = 1;
                InputConsole.AddStaticMessageToInputConsole("   Timescale set to 1");
                
            }
            hasExecutedBefore = true; //This boolean means that the command runs only once despite it being in the "Update" method


        }
        

        if (hasTakenOff) //This part only runs after the spacecraft has taken off
        {
            if (currentActiveStage == 1) //While the current stage is the first stage then decrease the fuel by the amount by the mass flow rate for each stage
            {
                Stage1.currentFuel -= Stage1.massFlowRate * Time.deltaTime * throttlePercentage; 

                if (Stage1.currentFuel < 0)
                {

                    throttlePercentage = 0; //Once the stage runs out of fuel, the throttle level is set to zero
                    
                }
            }
            else if (currentActiveStage == 2)
            {
                Stage2.currentFuel -= Stage2.massFlowRate * Time.deltaTime * throttlePercentage; //12890
                //
                Stage1.Deactivate(); //Deactivate method hides the game object related to the specified spacecraft stage
                Stage1.isActive = false;
                Stage2.SetFlamesActive();
                currentActiveFlames.Clear(); //Clears the list of the current active flame gameobjects, so that the flames for the next stage can be added
                for (int i = 0; i < Stage2.flames.Length; i++)
                {
                    currentActiveFlames.Add(Stage2.flames[i]);
                }

                Stage1.currentFuel = 0;
                //
                if (Stage2.currentFuel < 0)
                {
                    throttlePercentage = 0;
                }

            }
            else if (currentActiveStage == 3)
            {
                //
                Stage2.Deactivate();
                Stage2.DeactivateFlames();
                Stage3.SetFlamesActive();
                Stage2.isActive = false;
                Stage2.currentFuel = 0;
                currentActiveFlames.Clear();
                for (int i = 0; i < Stage3.flames.Length; i++)
                {
                    currentActiveFlames.Add(Stage3.flames[i]);
                }
                //
                Stage3.currentFuel -= Stage3.massFlowRate * Time.deltaTime * throttlePercentage; //100
                if (Stage3.currentFuel < 0)
                {
                    throttlePercentage = 0;
                }
                
            }
            else if (currentActiveStage == 4)
            {

                //
                Stage3.Deactivate();
                Stage3.isActive = false;
                Stage3.currentFuel = 0;
                Lander.SetFlamesActive();
                currentActiveFlames.Clear();
                for (int i = 0; i < Lander.flames.Length; i++)
                {
                    currentActiveFlames.Add(LanderFLames[i]);
                }
                //
                Lander.currentFuel -= 5 * Time.deltaTime * throttlePercentage;
                
                if (Lander.currentFuel < 0)
                {
                    if (!executeOnce)
                    {
                        InputConsole.AddStaticMessageToInputConsole("All stages out of fuel"); //This line runs once all of the stages are out of fuel
                        checklist.isOutOfFuel = true;
                        throttlePercentage = 0;
                        executeOnce = true;
                    }
                    
                }
            }
        }

        FlamesController();
    }

    public IEnumerator UpdatePosition() //This is the main method that updates the position of the rocket every 1/60th of a second (objective 3.1.12)
    {
        
        currentAltitude = (Rocket.transform.position.y) * 10000; 
        Vector3 RocketPos = Rocket.transform.position * 10000; 
        RocketPos.y += (float)deltaAltitude;
        Rocket.transform.position = RocketPos / 10000; 

        var temp = calculator.CalculateVelocity(DisplayAcceleration(), pitchAngle * Mathf.Deg2Rad); //This method calculates the velcoity
        double a = temp.YVelocity;
        deltaAltitude = (a * Time.deltaTime) + (0.5 * DisplayAcceleration() * Mathf.Pow(Time.deltaTime, 2)); //suvat equation that updates the altitude

        yield return null; 
    }

    public float ReturnThrottleLevel(float _ThrottlePercentage) //Method that is used to return a throttle level, this is used from other script
    {
        throttlePercentage = _ThrottlePercentage;

        return throttlePercentage;
    }

    public double CalculateCurrentMass(double S1Fuel, double S2Fuel, double S3Fuel, double landerFuel) //Method that calculates the current mass of the spacecraft
    {
        return CalculateRocketDryMas(Stage1.dryMass, Stage2.dryMass, Stage3.dryMass, Lander.dryMass) + S1Fuel + S2Fuel + S3Fuel + landerFuel;
    }

    public double CalculateRocketDryMas(double S1, double S2, double S3, double Lander) //Calculates the mass of the empty stages based on whether it is active or not
    {
        return (S1 * Convert.ToInt32(Stage3.isActive)) + (S2 * Convert.ToInt32(Stage2.isActive)) + (S3 * Convert.ToInt32(Stage3.isActive)) + Lander;
    }

    public double DisplayAcceleration() //This method calculates the acceleration by taking in all of the calculations from the physics calculator class
    {
        double temperature = calculator.CalculateTemperature(currentAltitude);
        double atmosphericPressure = calculator.CalculatePressure(currentAltitude);
        double airDensity = calculator.CalculateDesnityOfAir(atmosphericPressure, temperature);
        drag = calculator.CalculateDrag(airDensity, horizontalVelocity, verticalVelocity, crossSectionalArea, calculator.CalculateDragCoefficient(verticalVelocity));

        double specificImpulse = calculator.CalculateSpecificImpulse(currentActiveStage, currentAltitude);
        thrust = calculator.CalculateThrust(specificImpulse, currentActiveStage);

        double currentAcceleration = calculator.CalculateAcceleration(drag, (float)thrust * throttlePercentage, (float)rocketMass, currentGravitationalFieldStrength, hasTakenOff);

        return currentAcceleration;
    }

    public float RoundAltitude(double currentAltitude) //This method rounds the altitude so that the UI appears clean, this also sets the altitude suffix showing the units of the altitude
    {
        if (currentAltitude > 1000000)
        {
            altitudeSuffix = "Mm";
            return (float)(currentAltitude / 1000000);
        }
        else if (currentAltitude > 1000)
        {
            altitudeSuffix = " Km";
            return (float)(currentAltitude / 1000);
        }
        else
        {
            altitudeSuffix = " m";
            return (float)currentAltitude;
        }
        
    }
    
    public IEnumerator UpdateStats() //This is the main method of updating the text elements that displays the output to the user
    {
        float startTime;
        float endTime;
        float elapsedTime = 0;
        float desiredTime = 0.015f;

        startTime = Time.realtimeSinceStartup;
        AltitudeText.text = ("Altitude: " + Mathf.RoundToInt(RoundAltitude(currentAltitude)) + altitudeSuffix); //Where it is valid the value is rounded to a reasonable degree of accuracy when outputting to the user
        DistanceToMoonText.text = "Distance To Moon: " + (int)Mathf.Round(Mathf.Clamp(calculator.CalculateDistanceToMoon(distanceToMoon, (float)currentAltitude),0,99999999999)) + " m";
        TotalMassText.text = "Total Mass " + Mathf.RoundToInt((float)rocketMass) + " kg";


        Stage1FuelText.text = "Stage 1 Fuel: " + math.clamp(Math.Round(((Mathf.Round((float)Stage1.currentFuel)/ Stage1.maxFuel)*100),1), 0, 100) + " %";
        Stage2FuelText.text = "Stage 2 Fuel: " + math.clamp(Math.Round(((Mathf.Round((float)Stage2.currentFuel) / Stage2.maxFuel) * 100),1), 0, 100)  + " %";
        Stage3FuelText.text = "Stage 3 Fuel: " + math.clamp(Math.Round(((Mathf.Round((float)Stage3.currentFuel) / Stage3.maxFuel) * 100),1), 0 , 100) + " %";
        LunarLanderFuelText.text = "Lander Fuel: " + math.clamp(Math.Round(((Mathf.Round((float)Lander.currentFuel) / Lander.maxFuel) * 100), 1), 0, 100) + " %";

        TemperatureText.text = "Temperature: " + Math.Round(calculator.CalculateTemperature(currentAltitude),2) + " C";

        AccelerationText.text = "Acceleration: " + Math.Round(DisplayAcceleration(),2) + " ms^-2";

        var velocity = calculator.CalculateVelocity(DisplayAcceleration(), pitchAngle * Mathf.Deg2Rad);
        horizontalVelocity = velocity.XVelocity;
        verticalVelocity = velocity.YVelocity;
        DeltaXText.text = "Delta-V X: " + Math.Round(velocity.XVelocity,1) + "ms^-1";
        DeltaYText.text = "Delta-V Y: " + Math.Round(velocity.YVelocity, 1) + "ms^-1";
        Yvelocity = (float)velocity.YVelocity;


        GravitationalFieldStrengthText.text = "Grav. Field " +  Math.Round(currentGravitationalFieldStrength,3) + " Nm-1";

        ThrottleLevelText.text = "Throttle Level: " + throttlePercentage;

        TimeScaleText.text = "Time Scale: 1 : " + timeController.modifyScale;

        TemporaryText.text = "Drag force: " + Math.Round(drag) + "N";
        ThrustText.text = "Thrust force: " + Math.Round(calculator.CalculateThrust(calculator.CalculateSpecificImpulse(currentActiveStage, currentAltitude),currentActiveStage)) * throttlePercentage + "N";

        endTime = Time.realtimeSinceStartup;
        elapsedTime += endTime - startTime;

        if (elapsedTime >= desiredTime)
        {
            elapsedTime = 0;
            yield return null;
        }

    }


    public void BeginFuelling() //Method that begins the fuelling process
    {   
       StartCoroutine(Fueling()); 
    }

    public void FlamesController() //This method sets the size of the flames based on the trottle percentage
    {
        foreach (var flame in currentActiveFlames)
        {
            flame.transform.localScale = new Vector3( throttlePercentage/33,flame.transform.localScale.y,flame.transform.localScale.z);
        }
    }

    public IEnumerator Fueling() //The method that fuels the rocket, incorporated as a coroutine so that the user can see the fuel increasing in real time
    {
        fuelingRate = 1000;

        while (Stage1.currentFuel < Stage1.maxFuel)
        {
            fuelingRate = 450000; //350000
            Stage1.currentFuel += fuelingRate * Time.deltaTime;
            yield return null;
        }
        while (Stage2.currentFuel < Stage2.maxFuel)
        {
            Stage2.currentFuel += fuelingRate * Time.deltaTime;
            yield return null;
        }
        while(Stage3.currentFuel < Stage3.maxFuel)
        {
            Stage3.currentFuel += fuelingRate * Time.deltaTime;
            yield return null;
        }
        checklist.isFueled = true; //Sets the value within the commandChecklist class to be true 
    }


    public enum RocketStageEnum //The enum that sets the currentActiveStage
    {
        Stage1 = 1,
        Stage2 = 2,
        Stage3 = 3,
        Lander = 4
    }
}
