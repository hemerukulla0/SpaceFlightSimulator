using System;
using UnityEngine;

public class PhysicsCalculator : MonoBehaviour
{
    public const double initialGravity = 9.81;

    //general variables
    private const double gravitationalConstant = 6.67e-11;

    //earth variables
    private const double earthMass = 5.9722e24;
    private const double earthRadius = 6371e3;
    private const int earthAtmosphereThickness = 84852;
    private const double seaLevelATP = 101325;
    private const double temperatureLapseRate = 0.00976;
    private const double seaLevelStandardTemp = 288.16;


    public const double speedOfSound = 343.0;


    //moon variables
    private const double moonMass = 1.348e22;
    private const double moonRadius = 1734e3;

    public double DensityOfAir;
    public const double airDensityAtSeaLevel = 1.2250;
    public const double molarMassOfAir = 0.0289644;
    public const double universalGasConstant = 8.3144598;

    public double initialVelocity = 0;

    public double initialXVelocity = 0;
    public double initialYVelocity = 0;


    public double CalculateGravitationalFieldStrength(double Altitude) //This method calculates the gravitational field strength at a given altitude (objective 3.1.9)
    {
        double currentGravityDueToEarth;
        double currentGravityDueToMoon;

        double currentDistanceToMoon = RocketMain.distanceToMoon - Altitude;

        currentGravityDueToEarth = -(gravitationalConstant * earthMass) / Mathf.Pow((float)Altitude + (float)earthRadius, 2); //Calculates gravitational attraction from the earth

        currentGravityDueToMoon = (gravitationalConstant * moonMass) / Mathf.Pow((float)currentDistanceToMoon, 2); //(float)moonRadius,2); //Calculates the gravitational attraction from the moon

        return currentGravityDueToEarth + currentGravityDueToMoon; //Gets the vector sum of the two calculations, and hence shows the resultant field strength 

    }

    

    public double CalculateAcceleration(double Drag, float CurrentThrust, float CurrentMass, double GravitationalFieldStrength, bool HasTakenOff) //The main method that calculates the acceleration taking in various parameters (objective 3.1.11)
    {
        if (!HasTakenOff) //This method only runs when the spacecraft has taken off, to reduce the number of irrelevant calculations
        {
            return 0;
        }

        double acceleration;

        if (CurrentMass == 0) //Ensures that the total mass never reaches zero so that there is never a division by zero error 
        {
            CurrentMass = 1;
        }

        if (FindObjectOfType<RocketMain>().pitchAngle >= 180 || FindObjectOfType<RocketMain>().pitchAngle <= -180)// If the spacecraft is upside down, then make sure the spacecraft is still accelerating in the correct direction
        {
            GravitationalFieldStrength = GravitationalFieldStrength * -1; //
        }

        acceleration = (CurrentThrust + (Drag + (CurrentMass * GravitationalFieldStrength))) / CurrentMass;

        return acceleration;
    }

    public double CalculateTemperature(double CurrentAltitude) //This method calculates temperature with respect to altitude (objective 3.1.6)
    {
        double temperatrure;

        if (CurrentAltitude < 12000) //average height of the tropospehere
        {
            temperatrure = 27 - (6.5e-3 * CurrentAltitude);  //27 is the average temperature recorded at the kennedy space centre
            return temperatrure;
        }
        else if (CurrentAltitude < 20000) //temperature between 12,000 and 20,000 meters remains fairly constant - approximated to constant here
        {
            temperatrure = -60;
            return temperatrure;
        }
        else if (CurrentAltitude < 48000) // average max height of stratosphere
        {
            temperatrure = -60 + (1.9e-3 * (CurrentAltitude - 20000));
            return temperatrure;
        }
        else if (CurrentAltitude < 51000) // temperature between 48,000 and 51,000 remains fairly constant
        {
            return temperatrure = -1;
        }
        else if (CurrentAltitude < 86000)
        {
            temperatrure = -1 - (2.4e-3 * (CurrentAltitude - 51000));
            return temperatrure;
        }
        else
        {
            return temperatrure = -100; // the temperature after the atmosphere can be approximated to -100
        }
       
        
    }


    public double CalculateDesnityOfAir(double currentPressure, double currentTemperature) //This method calculates the air density taking in pressure and temperature as parameters, this is used to calculate drag experienced by the spacecraft (forms part of objective 3.1.8)
    {

        double density;
        density = currentPressure / (universalGasConstant * (currentTemperature + 273.15)); //formula that calulates the density, the temperature must be converted to kelvin to be used in the formula
        return density;
    }

    public double CalculateDrag(double AirDensity, double XVelocity, double YVelocity, double CrossSectionalArea, double DragCoefficient) //This method calculates the drag taking in velocity in both componants (objective 3.1.8)
    {
        double Drag;

        Drag = 0.5 * AirDensity * Mathf.Pow((float)((XVelocity * XVelocity) + (YVelocity * YVelocity)), 0.5f) * DragCoefficient * CrossSectionalArea; //This is the equation that relates all of the inputs and calculates the drag force exerted on the spacecraft
        return Drag;
    }

    public float CalculateDistanceToMoon(float distanceToMoon, float currentAltitude) //A simple subtraction, has been incorporated as a method so that the value returned is rounded to an integer
    {
        return (int)Mathf.Round((float)((distanceToMoon- 742944) - currentAltitude)); //The value here is the "thickness" of the moon
    }
    public (double XVelocity, double YVelocity) CalculateVelocity(double acceleration, float pitchAngle ) //A tuple method is able to return two values, in this case returns the X and Y velocity (objective 3.1.4 and 3.1.5)
    {


        initialXVelocity -= acceleration * Mathf.Sin((float)pitchAngle) * Time.deltaTime; //Trigonometric functions used to calculate the the velocity in both planes by splitting the acceleration into its componants
        initialYVelocity += acceleration * Mathf.Cos((float)pitchAngle) * Time.deltaTime;


        return (initialXVelocity, initialYVelocity); //Both values for the tuple are returned at once
        
    }

    public double CalculatePressure(double currentAltitude) //This method calculates the pressure at given altitude (objective 3.1.7)
    {
        double pressure;
        while (currentAltitude < 29000)
        {
            

            double a = temperatureLapseRate * currentAltitude;
            double b = initialGravity * molarMassOfAir;
            double c = universalGasConstant * temperatureLapseRate;

            pressure = seaLevelATP * Math.Pow((1 - (a / seaLevelStandardTemp)), (b / c)); //formula used to calculate the pressure, has been split into a multistep calculation to improve readablilty

            return pressure;
        }

        return pressure = 0.00005; //After around 30km, the pressure due to the atmosphere is negligible hence has been approximated to a small value
        
    }

    public double CalculateThrust(double specificImpulse, int currentActiveStage) //The thrust depends on the current active stage because each stage has different engines with different powers (objective 3.1.10a)
    {
        double massFlowRate;
        switch (currentActiveStage) // case statement used to assign the mass flow rate depending on the current active stage
        {
            case 1:
                massFlowRate = 12890; //Stage 1 (the solid rocket boosters) have the most powerful engines hence their mass flow rate is the highest
                break;
            case 2:
                massFlowRate = 1303.5; //
                break;
            case 3:
                massFlowRate = 400;
                break;
            case 4:
                massFlowRate = 7;
                break;
            default: //Default statement will only be accessed if all stages are out of fuel, in which case there should be no thrust force - hence value is set to zero here
                massFlowRate = 0;
                break;
        }

        double thrust = initialGravity * massFlowRate * specificImpulse; //The thrust force is a product of the mass flow rate and the specific impulse, which is unique for each fuel type in the spacecraft 

        return thrust;
    }

    /*public double CalculateMassFlowRate(int currentActiveStage)
    {
        double massFlowRate;
        switch(currentActiveStage)
        {
            case 1:
                massFlowRate = 12890; //12890
                break;

            case 2:
                massFlowRate = 1203.5;
                break;

            case 3:
                massFlowRate = 15;
                break;

        default:
                massFlowRate = 0;
                break;
        }



        return massFlowRate;
    }*/

    public double CalculateSpecificImpulse(int currentActiveStage, double currentAltitude) //Forms part of the thrust calculation specific impulse increases with altitude as the strength of gravity decreases
    {
        double specificImpulse;
        if (currentActiveStage == 1) //Stage 1 uses solid rocket fuel so this is the specific impulse calculation for that fuel
        {
            if (currentAltitude < 30000)
            {
                specificImpulse = ((1.0589e-3) * (currentAltitude) + 220); //The impulse is essentially calculated with a linear function with a gradient of 1.0589e-3
                return specificImpulse;
            }
            else
            {
                return specificImpulse = 302;
            }
            
        }
        else if (currentActiveStage == 2 || currentActiveStage == 3) //Stage 2 and 3 use similar fuel compounds so these are used to calculate the specific impulse for these stages
        {
            if (currentAltitude <30000)
            {
                specificImpulse = ((2.652e-3) * (currentAltitude) + 200);
                return specificImpulse;
            }
            else
            {
                return specificImpulse = 425;
            }


        }
        else if (currentActiveStage == 4) //The lander uses a very lightweight and low powered fuel, this fuel produces a very low thrust force but since there is no atmoshere this is acceptable
        {
            specificImpulse = 311;
            return specificImpulse;
        }
        else
        {
            Debug.Log("Error - No stages currently active"); //This is called if all stages are out of fuel
            return 0;
        }
    }

    public double CalculateDragCoefficient(double verticalVelocity) //The drag coefficient in reality is a curve, but since is does not really follow any equation it is much easier to model it is a series of linear lines (objective 3.1.2)
    {
        ;
        double machSpeed = verticalVelocity / speedOfSound; //The speed in meters per second must be convered to a mach speed, showing the speed of the spacecraft relative to the speed of sound
        int machSpeedCalc = (int)machSpeed * 10;

        if (machSpeedCalc >= 0 && machSpeedCalc <= 5)
        {
            return -0.008 * (machSpeed) + 0.3;
        }
        else if (machSpeedCalc > 5 && machSpeedCalc <= 15)
        {
            return 0.32222 * (machSpeed) + 0.115; //0.27
        }
        else if (machSpeedCalc > 15 && machSpeedCalc <= 45)
        {
            return -0.11667 * (machSpeed) + 0.725005; //0.55
        }
        else if (machSpeedCalc > 45 && machSpeedCalc <= 90)
        {
            return 0.011111 * machSpeed + 0.15005; //0.2
        }
        else if (machSpeedCalc > 90)
        {
            return 0.25; //Above mach 90 the coefficient of drag remains constant
        }

        else return 0;
    }
}
