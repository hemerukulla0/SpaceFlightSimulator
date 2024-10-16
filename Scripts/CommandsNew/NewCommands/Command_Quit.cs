using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace Console
{
    public class Command_Quit : InputCommand
    {
        public override string Name { get; protected set; }
        public override string CommandSyntax { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }

        public Command_Quit()
        {
            Name = "Quit";
            CommandSyntax = "quit";
            Description = "Quits the application";
            Help = "Use command with no args to force quit application";

            AddToConsole();
        }

        public override void ExecuteCommad(string[] args)
        {
            if (Application.isEditor) //This is the statement that closes the appllication
            {
                EditorApplication.isPlaying = false;
            }
            else
            {
                Application.Quit();
            }    
        }

        public static Command_Quit CreateCommand()
        {
            return new Command_Quit();
        }
    }
}

