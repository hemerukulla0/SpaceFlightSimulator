using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Windows;
using UnityEngine.UIElements;

namespace Console
{
    public abstract class InputCommand //This is the class that is inherited by all of the other command classes
    {
        public abstract string Name { get; protected set; }
        public abstract string CommandSyntax { get; protected set; }
        public abstract string Description { get; protected set; }
        public abstract string Help { get; protected set; }


        public abstract void ExecuteCommad(string[] args); //These attributes will be overwriten by the individual command

        public void AddToConsole()
        {
            InputConsole.AddCommandsToInputConsole(CommandSyntax, this);  
        }

        public CommandChecklist checklist = MonoBehaviour.FindObjectOfType<CommandChecklist>();
        

    }

    public class InputConsole : MonoBehaviour //This class controls the input of the commands into the window
    {
        public static InputConsole Instance { get; private set; }

        public static Dictionary<string, InputCommand> Commands { get; private set; } //A dictionary is created (objective 3.2.5) for the commands to be stored into

        public List<string> commandList; //This list hold the commands in it so that the autosuggestion feature can work properly

        [Header("UI Components")]
        public Canvas myCanvas;
        public ScrollRect myScrollrect;
        public TMP_Text myOutputText;
        public TMP_InputField myCommandInputField;
        public TMP_Text[] CommandButtonsText;

        public string previousCommand;
        


        private void Awake()
        {
            if (Instance != null)
            {
                return;
            }

            Instance = this;
            Commands = new Dictionary<string, InputCommand>(); //Upon the simulation starting, the dictionary is initialized
            commandList = new List<string>(); //Upon the simulation starting, the list of commands is initialized
        }

        private void Create() //In this method all of the commands are initialized to local variables stored within this class
        {
            
            Command_StartFuelling commandStartFuelling = Command_StartFuelling.CreateCommand();
            Command_SetThrottle commandSetThrottle = Command_SetThrottle.CreateCommand();
            Command_StartCountDownTemp command_StartCountDownTemp = Command_StartCountDownTemp.CreateCommand();
            Command_SetPitch command_setPitch = Command_SetPitch.CreateCommand();
            Command_TimeScale command_timeScale = Command_TimeScale.CreateCommand();
            Command_Quit commandQuit = Command_Quit.CreateCommand();
            Command_StageSeparation command_stageSeparation = Command_StageSeparation.CreateCommand();
            Command_OpenInstructions command_openInstructions = Command_OpenInstructions.CreateCommand();
            Command_EstablishUplink command_establishUplink = Command_EstablishUplink.CreateCommand();

        }

        private void Start() //Upon the simulaiton loading, all of the commands are created
        {
            Create(); //Command that creates the commands
            previousCommand = ""; // the previous command value is used when the backslash key is pressed
        }

        private void Update()
        {
            if (myCommandInputField.text != "" || myCommandInputField.text != " ")
            {
                AutoCompleteCommands(myCommandInputField.text); //For every frame, the autocomplete command method is executed to suggest commands to the user
            }
            
        }

        public void OnReturn() //Upon pressing the enter key, this method will execute
        {
            if (myCommandInputField.text != "")
            {
                AddMessageToInputConsole(myCommandInputField.text);
                InputHandler(myCommandInputField.text); //Input is passed into a method called input handler
                myCommandInputField.text = "";
            }
        }

        public void OnTabPress() //When pressing the tab key, the first autosuggest button is loaded into the input panel
        {
            myCommandInputField.text = CommandButtonsText[0].text; //The first button is the 0th element in the array of buttons
            myCommandInputField.MoveToEndOfLine(false, false); //This line just moves the cursor of the input panel to the end of the line
        }

        public void OnSlashPress() //This is the feature that loads the previously executed command back into the input window
        {
            myCommandInputField.text = previousCommand; //Previously executed command is loaded back into the inputfield
            myCommandInputField.MoveToEndOfLine(false, false);
        }

        public static void AddCommandsToInputConsole(string name, InputCommand command) //This command adds each command into the dictionary
        {
            if (!Commands.ContainsKey(name)) //if the command is not already present in the dictionary, then add it to the dictionary
            {
                Commands.Add(name, command); //Command added to the dictionary
            }
        }

        private void AddMessageToInputConsole(string OutputMessage) //This allows the program to display messages to the user
        {
            myOutputText.text += OutputMessage + "\n";
            myScrollrect.verticalNormalizedPosition = 0f; //force the scroll view to the bottom

        }

        public static void AddStaticMessageToInputConsole(string OutputMessage) //This allows the program to display messages to the user but since it is static this means that it can be called from other scripts
        {
            InputConsole.Instance.myOutputText.text += OutputMessage + "\n";
            InputConsole.Instance.myScrollrect.verticalNormalizedPosition = 0f;
            InputConsole.Instance.myScrollrect.verticalNormalizedPosition = 0f;
        }

        public void AutoCompleteCommands(string input) //This is the autocomplete commands method
        {
            foreach (var item in Commands) //For each record in the dictionary:
            {

                if (item.Key.Contains(input)) //If the key of this item contains the characters passed as an input
                {
                    commandList.Add(item.Key); //Then this/ these commands are added to a list of possible autosuggestions
                }
                
            }

            if (commandList.ToArray().Length > 0)   //If there are items in the list, this means that there are commands present containing the characters inputted
            {
                CommandButtonsText[0].text = commandList.ToArray()[0]; //The first suggestion is assigned to the first button
                if (commandList.ToArray().Length > 1) //If more than one suggestion exists then:
                {
                    CommandButtonsText[1].text = commandList.ToArray()[1]; //Add the second suggestion to the second button
                }
                else
                {
                    CommandButtonsText[1].text = "-"; //If no other suggestions are produced by the algorithm then the second button is set to a dash value
                }
            }
            commandList.Clear(); //The list of suggestions is cleared every time an input is detected, so that it can be updated with new suggestions

            
        }

        private void InputHandler(string input) //This method handles the user input
        {

            try
            {
                string[] tempArray = input.Split(' '); //Split user input into two components, the command and the parameter (if present)
                if (input.Length == 0) //If the input is zero characters long, ie. nothing is inputted into the window:
                {
                    AddMessageToInputConsole("  Invalid Command"); //this message is output to the user
                    return;
                }

                if (!Commands.ContainsKey(tempArray[0])) //If the command is not detected in the dictionary then output invalid command message
                {
                    AddMessageToInputConsole("  Invalid Command");
                }
                else
                {
                    List<string> args = tempArray.ToList(); //the input of the user is converted to a list so that the RemoveAt function can be used

                    args.RemoveAt(0);
                    Commands[tempArray[0]].ExecuteCommad(args.ToArray()); //This command is now executed by accessing the execute command method for the subclass for each command

                    previousCommand = input; //the previous input value is set to this variable so that the back-slash feature can be used
                }
            }
            catch (System.Exception) //Ensures the system does not crash when an invalid value is entered
            {

                AddStaticMessageToInputConsole("    Invalid syntax, try again"); //Appropriate output message displayed if the input is in the wrong format
            }
            
        }
    }
}

