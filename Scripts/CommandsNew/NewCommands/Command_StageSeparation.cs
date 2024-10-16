using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Console
{
    public class Command_StageSeparation : InputCommand
    {
        public GameObject manager;
        public GameObject checklist;
        public GameObject timeManager;
        public override string Name { get; protected set; }
        public override string CommandSyntax { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }

        public Command_StageSeparation()
        {
            Name = "StageSeparation";
            CommandSyntax = "stageSeparation";
            Description = "Detatches the currently active state, cannot be used if the lander is active";
            Help = "Use command with no args to detatch the next stage of the spacecraft";

            AddToConsole();
            manager = MonoBehaviour.FindObjectOfType<RocketMain>().gameObject;
            checklist = MonoBehaviour.FindObjectOfType<CommandChecklist>().gameObject;
            timeManager = MonoBehaviour.FindObjectOfType<MainTimeScaleController>().gameObject;
        }

        public override void ExecuteCommad(string[] args)
        {
            if (checklist.GetComponent<CommandChecklist>().isFueled == true && checklist.GetComponent<CommandChecklist>().isLaunched == true) //Prerequisites for the command include the spacecraft taking off and it being fueled
            {
                if (checklist.GetComponent<CommandChecklist>().isLanderActive == false)
                {
                    manager.GetComponent<RocketMain>().currentActiveStage++; //Increases the value from the enum in rocketMain by 1
                    InputConsole.AddStaticMessageToInputConsole("   separating stage..."); //The outputs displayed when the command is executed
                    InputConsole.AddStaticMessageToInputConsole("   Timescale set to 1");
                    timeManager.GetComponent<MainTimeScaleController>().modifyScale =1; //Timescale is set back to one
                }
                else
                {
                    InputConsole.AddStaticMessageToInputConsole("   command not valid while lander is active"); //If the lander is active there are no more stages to separate, so this is output to the user
                }
                
            }
            else
            {
                InputConsole.AddStaticMessageToInputConsole("   Command not valid while spacecraft is grounded"); //Since the command needs the spacecraft to be airborne, this will be output if the user tries to execute the command on the ground
            }
            
            
        }

        public static Command_StageSeparation CreateCommand()
        {
            return new Command_StageSeparation();
        }
    }
}

