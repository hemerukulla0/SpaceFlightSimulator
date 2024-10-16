using Console;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Console
{
    public class Command_EstablishUplink : InputCommand //All commands inherit from the input command class
    {
        public override string Name { get; protected set; }
        public override string CommandSyntax { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }

        private GameObject ConnectionScreen;
        private GameObject manager;

        public Command_EstablishUplink()
        {
            Name = "EstablishUplink";
            CommandSyntax = "establishUplink";
            Description = "Activates the data connection to the spacecraft";
            Help = "Use command with no args to activate the flow of data to the spacecraft";

            manager = MonoBehaviour.FindObjectOfType<RocketMain>().gameObject;
            ConnectionScreen = manager.GetComponent<RocketMain>().conenctionScreen;

            AddToConsole();
        }

        public override void ExecuteCommad(string[] args)
        {
            if (checklist.GetComponent<CommandChecklist>().isUplinkEstablished == false) //for this command the screen displaying "No connection to spacecraft" is removed upon the command being entered
            {
                ConnectionScreen.SetActive(false);
                checklist.GetComponent<CommandChecklist>().isUplinkEstablished = true;
                InputConsole.AddStaticMessageToInputConsole("   Establishing uplink..."); //Output message if the command is valid

            }
            else
            {
                InputConsole.AddStaticMessageToInputConsole("   Uplink already active");//Output message if the command is invalid
            }


            
        }

        public static Command_EstablishUplink CreateCommand()
        {
            return new Command_EstablishUplink();
        }
    }
}

