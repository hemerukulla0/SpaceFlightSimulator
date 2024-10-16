using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CommandChecklist : MonoBehaviour
{
    GameObject manager;

    public bool isUplinkEstablished; //The boolean values that are updated by the individual commands
    public bool isFueled;
    public bool isLaunched;
    public bool isLanderActive;
    public bool isOutOfFuel;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<RocketMain>().gameObject; //Links this class to the RocketMain class
        isFueled = false; //Initialises two of the boolean values to false
        isUplinkEstablished = false;
    }

    // Update is called once per frame
    void Update()
    {
        isLaunched = manager.GetComponent<RocketMain>().hasTakenOff; //updates the isLaunched variable based on whether the spacecraft has taken off

        if (manager.GetComponent<RocketMain>().currentActiveStage == 4)
        {
            isLanderActive = true; //When the current active stage is 4, we know that the laner is active
        }

       
    }
}
