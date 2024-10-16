using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Console
{
    public class Command_SetThrottle : InputCommand
    {
        private GameObject manager;

        public override string Name { get; protected set; }
        public override string CommandSyntax { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }


        public Command_SetThrottle()
        {
            Name = "SetThrottle";
            CommandSyntax = "setThrottle";
            Description = "sets the throttle of the engine currently active";
            Help = "setThrottle -=<value> where value is between 0 and 1";

            AddToConsole();

            manager = MonoBehaviour.FindObjectOfType<RocketMain>().gameObject;
        }

        public override void ExecuteCommad(string[] args)
        {
            while (!checklist.GetComponent<CommandChecklist>().isOutOfFuel) //The prerequisite is that the spacecraft must not be out of fuel
            {
                float throttle = 0;

                for (int i = 0; i < args.Length; i++)
                {
                    string argument = args[i];
                    string[] splitArgs = Regex.Split(argument, @"\=");

                    throttle = float.Parse(splitArgs[1]);
                }
                if (throttle >= 0 && throttle <= 1) //Throttle must be between 0 and 1
                {
                    manager.GetComponent<RocketMain>().ReturnThrottleLevel(throttle); //The throttle is returned to the MainRocket script via the method
                    InputConsole.AddStaticMessageToInputConsole("   Throttle set to " + throttle);
                }
                else
                {
                    InputConsole.AddStaticMessageToInputConsole("   Throttle level must be between 0 and 1"); //Error message if the throttle level is not between 0 and 1
                }

                return;
            }
            
            InputConsole.AddStaticMessageToInputConsole("   Cannot change throttle while out of fuel"); //if the spacecraft is out of fuel then the command will not execute and this message will display
            

            
            
        }

        public static Command_SetThrottle CreateCommand()
        {
            return new Command_SetThrottle();
        }
    }
}