using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Console
{
    public class Command_StartFuelling : InputCommand
    {
        public GameObject manager;
        public override string Name { get; protected set; }
        public override string CommandSyntax { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }


        public Command_StartFuelling()
        {
            Name = "StartFuelling";
            CommandSyntax = "startFuelling";
            Description = "Begins the fuelling process";
            Help = "Use command with no args to start the fuelling process of the spacecraft";

            AddToConsole();
            manager = MonoBehaviour.FindObjectOfType<RocketMain>().gameObject;
          
        }

        public override void ExecuteCommad(string[] args)
        {
            if (checklist.isUplinkEstablished)
            {
                if (checklist.GetComponent<CommandChecklist>().isFueled == false)
                {
                    manager.GetComponent<RocketMain>().BeginFuelling(); //Calls the method that starts the coroutine that initializes the fueling process
                    checklist.GetComponent<CommandChecklist>().isFueled = true; //Value within the checklist of executed commands is set to true.
                    InputConsole.AddStaticMessageToInputConsole("   Fuelling Process Initialized");
                }
                else
                {
                    InputConsole.AddStaticMessageToInputConsole("   The rocket has already been fueled"); //If the spacecraft has already been fueled this is outputted
                }
            }
            else
            {
                InputConsole.AddStaticMessageToInputConsole("   No connection to spacecraft"); //If the uplink is not active then this is output
            }
            
            
        }

        public static Command_StartFuelling CreateCommand()
        {
            return new Command_StartFuelling();
        }
    }
}