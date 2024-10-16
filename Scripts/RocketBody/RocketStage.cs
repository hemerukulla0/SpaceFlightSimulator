using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketStage //This is my class that is used as a User defined data structure to store information about the spacecraft
{
    public int dryMass; //These are the attributes stored by the program
    public int maxFuel;
    public double currentFuel;
    public double massFlowRate;
    public bool isActive;
    public double crossSectionalArea;

    public GameObject[] body;
    public GameObject[] flames;

    public RocketStage(int dryMass, int maxFuel, double currentFuel, double massFlowRate, GameObject[] body, GameObject[] flames, bool isActive, double crossSectionalArea) //Constructer that initializes these values
    {
        this.dryMass = dryMass;
        this.maxFuel = maxFuel;
        this.currentFuel = currentFuel;
        this.massFlowRate = massFlowRate;
        this.body = body;
        this.flames = flames;
        this.isActive = isActive;
        this.crossSectionalArea = crossSectionalArea;
    }

    public void Deactivate() //The methods that are available for this data type, this method makes the stage invisible, when it is separated for example
    {
        for (int i = 0; i < body.Length; i++)
        {
            body[i].SetActive(false);
        }
    }

    public void SetFlamesActive() //Sets the flames of the current active stage to be enabled
    {
        for (int i = 0; i < flames.Length; i++)
        {
            flames[i].SetActive(true);
        }
    }

    public void DeactivateFlames() //Sets the flames of the current active stage to be disabled
    {
        for (int i = 0; i < flames.Length; i++)
        {
            flames[i].SetActive(false);
        }
    }
}
