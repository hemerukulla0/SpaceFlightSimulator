using System.Collections.Generic;
using UnityEngine;


public class TerminalWindowController : MonoBehaviour
{


    [SerializeReference] public GameObject tempobject;
     


    public static Command StartFuelling;
    public static Command Launch;
    public static Command<float> Throttle;


    public List<object> commandList;

    

    private void Awake()
    {
       

        StartFuelling = new Command("startFuelling", "Intitializes the fueling process.", "startFuelling", () =>
        {
            //GameObject temp = Instantiate(tempobject, transform.position, transform.rotation);
            //temp.GetComponent<RocketMain>().BeginFuelling();
            Debug.Log("Fueling Starting");       
        });

        Launch = new Command("launchSequenceBegin", "Starts the launch sequence - only use when all prerequisites are completed,", "launchSequence", () =>
        {

        });

        Throttle = new Command<float>("throttle", "Enter a value between 0 and 1 which will set the throttle percentage of the engine", "throttleLevel <throttlePercentage>", (y) =>
        {
            //GameObject temp = Instantiate(tempobject, transform.position, transform.rotation);

            if (y > 1 | y < 0)
            {
                Debug.Log("Invalid Input, throttle Level must be between 0 and 1");
            }
            else
            {
               // temp.GetComponent<RocketMain>().throttlePercentage = y;
                Debug.Log("Throttle set to: " + y);
            }
            
        });

        commandList = new List<object>
        {
            StartFuelling,
            Throttle
        };

    }

}
