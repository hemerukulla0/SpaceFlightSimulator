using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

namespace Console
{
    public class Command_StartCountDownTemp : InputCommand //This script starts the countdown timer for liftoff once the prerequisites have been met
    {

        public GameObject manager;
        public GameObject backgroundPanel;
        public override string Name { get; protected set; }
        public override string CommandSyntax { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }

        public string scriptName = "BackGroundColourControl"; //This command also calls this script that starts the colour from changing blue to black
        

        public Command_StartCountDownTemp()
        {
            Name = "StartCountdown";
            CommandSyntax = "startCountdown";
            Description = "Starts the countdown timer";
            Help = "Use command to start the timer";

            AddToConsole();
            manager = MonoBehaviour.FindObjectOfType<MainTimeController>().gameObject;
            backgroundPanel = MonoBehaviour.FindObjectOfType<BackGroundColourControl>().gameObject;
        }

        public override void ExecuteCommad(string[] args)
        {
            if (checklist.GetComponent<CommandChecklist>().isFueled && checklist.GetComponent<CommandChecklist>().isUplinkEstablished) //If the prerequsites have been met:
            {
                (manager.GetComponent("MainTimeController") as MonoBehaviour).enabled = true; //start the countdown script
                (backgroundPanel.GetComponent(scriptName) as MonoBehaviour).enabled = true;//start the colour changing of the sky

                InputConsole.AddStaticMessageToInputConsole("   Countdown Initialized"); //Output this message to the user
            }
            else
            {
                InputConsole.AddStaticMessageToInputConsole("   Launch prerequisites not met"); //If the prerequisites have not been met then output this to the user
            }
            
        }
        
        public static Command_StartCountDownTemp CreateCommand()
        {
            return new Command_StartCountDownTemp();
        }
    }
}

