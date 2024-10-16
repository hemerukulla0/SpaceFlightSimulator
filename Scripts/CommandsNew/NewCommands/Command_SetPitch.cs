using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Console
{
    public class Command_SetPitch : InputCommand
    {
        public GameObject manager;
        public override string Name { get; protected set; }
        public override string CommandSyntax { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }


        public Command_SetPitch()
        {
            Name = "setPitch";
            CommandSyntax = "setPitch";
            Description = "Set the angle of the rocket with respect to the vertical";
            Help = "Use command with one arg to change the angle of the spacecraft with respect to the vertical";

            AddToConsole();
            manager = MonoBehaviour.FindObjectOfType<RocketMain>().gameObject;
        }

        public override void ExecuteCommad(string[] args)
        {
            if (checklist.isUplinkEstablished)
            {
                while (manager.GetComponent<RocketMain>().currentAltitude != 0) //This command has two prerequisites, the uplink being established and the altitude must be greater than zero
                {
                    float pitchAngle = 0;

                    for (int i = 0; i < args.Length; i++)
                    {
                        string argument = args[i];
                        string[] splitArgs = Regex.Split(argument, @"\=");

                        pitchAngle = float.Parse(splitArgs[1]);
                    }

                    if (pitchAngle > 360 || pitchAngle <= -360) //The pitch angle can only be between -360 and 360 degrees
                    {
                        InputConsole.AddStaticMessageToInputConsole("   Invalid angle: angle can only between -360 and 360"); //Output message if the pitch angle is not valid
                    }
                    else
                    {
                        manager.GetComponent<RocketMain>().Rocket.transform.rotation = Quaternion.Euler(0.0f, 0.0f, pitchAngle);
                        manager.GetComponent<RocketMain>().Camera.transform.rotation = Quaternion.Euler(0f, 0f, -manager.GetComponent<RocketMain>().Rocket.transform.rotation.z + 1); //Camera must be set to the opposite pitch to the rocket, so that you can see the rotation




                        InputConsole.AddStaticMessageToInputConsole("   Rotation set to " + pitchAngle); //Output message if the angle of the rocket is set
                    }
                   
                    return;
                }

                InputConsole.AddStaticMessageToInputConsole("   Command not valid at zero altitude"); //Output message if the altitude is still zero
            }
            else
            {
                InputConsole.AddStaticMessageToInputConsole("   Need to establish uplink first"); //Output message if the uplink is not established
            }
            
            
        }

        

        public static Command_SetPitch CreateCommand()
        {
            return new Command_SetPitch();
        }

        

    }
}
