using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions; //The regular expressions library is imported
using UnityEngine;

namespace Console
{
    public class Command_TimeScale : InputCommand //This class inherits from the main InputCommand class
    {
        public GameObject manager; //The class adds its own attribute in this case
     

        public override string Name { get; protected set; } //The original attributes are overwritten with the new values (incorporates polymorphism)
        public override string CommandSyntax { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }


        public Command_TimeScale()
        {
            Name = "TimeScale"; //New values for the attribute
            CommandSyntax = "setTimeScale";
            Description = "sets the speed at which the game progresses";
            Help = "setTimeScale -=<value> where value is between 1 and 10";

            AddToConsole();
            manager = MonoBehaviour.FindObjectOfType<MainTimeScaleController>().gameObject; //The script is loaded into the manager variable
        }

        public override void ExecuteCommad(string[] args) //This ovverrides the original execute command in the InputCommand function
        {
            if (checklist.isUplinkEstablished) // Checks if a prerequisite has been met for the command to be executed
            {
                float timeScale = 0; //initializes the timescale value

                for (int i = 0; i < args.Length; i++)
                {
                    string argument = args[i];
                    string[] splitArgs = Regex.Split(argument, @"\="); //Regex used to split the command into the command and the passed in parameter (objective 3.2.2)

                    timeScale = float.Parse(splitArgs[1]); //In this case, the command is a float between 0 and 1, so the input is parsed into a float variable
                }


                if (timeScale <= 100 && timeScale >=0) //The timescale value must be between 0 and 100
                {
                    manager.GetComponent<MainTimeScaleController>().modifyScale = timeScale; //The timescale value value is altered through another class

                    InputConsole.AddStaticMessageToInputConsole("   Time Compression ratio set to 1 : " + timeScale); //A output message is displayed to the user showing that the timescale has been set
                }
                else
                {
                    InputConsole.AddStaticMessageToInputConsole("   The timescale can only be set to a maximum of 100"); //Output message if the timescale is set to an invalid value
                }

               
            }
        }

        public static Command_TimeScale CreateCommand() //This allows the command to be initialized from a different script.
        {
            return new Command_TimeScale();
        }
    }
}