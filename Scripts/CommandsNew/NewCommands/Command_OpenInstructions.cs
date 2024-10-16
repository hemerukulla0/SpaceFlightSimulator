using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Console
{
    public class Command_OpenInstructions : InputCommand
    {
        public override string Name { get; protected set; }
        public override string CommandSyntax { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }

        private GameObject InstructionsPanel; //The gameobbject that contains the instructions screen
        private GameObject manager;

        public Command_OpenInstructions()
        {
            Name = "OpenInstructions";
            CommandSyntax = "openInstructions";
            Description = "Opens the instructions screen";
            Help = "Use command with no args to open the instructions screen";

            manager = MonoBehaviour.FindObjectOfType<RocketMain>().gameObject;
            InstructionsPanel = manager.GetComponent<RocketMain>().InstructionPanel;

            AddToConsole();
        }

        public override void ExecuteCommad(string[] args)
        {
            InstructionsPanel.SetActive(true); //When command is entered then the instructions screen is opened up
            InputConsole.AddStaticMessageToInputConsole("   Opening Instructions Screen...");
        }

        public static Command_OpenInstructions CreateCommand()
        {
            return new Command_OpenInstructions();
        }

    }
}

