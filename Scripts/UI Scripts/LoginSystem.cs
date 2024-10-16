using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Unity;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class LoginSystem : MonoBehaviour
{
    [SerializeField] public GameObject LoginUI; //Defining the UI elements for the login and register menu
    [SerializeField] public GameObject RegisterUI;

    [Header("Register UI")]
    [SerializeField] public TMP_InputField usernameInputField;
    [SerializeField] public TMP_InputField passwordInputField;
    [SerializeField] public TMP_InputField confirmPasswordInputField;
    [SerializeField] public TMP_Text messageText;

    [Header("Login UI")]
    [SerializeField] public TMP_InputField usernameLoginInputField;
    [SerializeField] public TMP_InputField passwordLoginInputField;
    [SerializeField] public TMP_Text messageLoginText;
   

    ArrayList users; //An array list is used to store all of the users currently in the test file

    public static string username; //Ths username is a static variable since it needs to be accessed from multiple scripts in the simulation

    public void Start()
    {
        if (File.Exists(Application.dataPath + "/logindetails.txt")) //Tests to see if the file exists at the datapath
        {
            users = new ArrayList(File.ReadAllLines(Application.dataPath + "/logindetails.txt")); //all of the entries within the textfile are stored into this arrayList
        }
        else
        {
            File.WriteAllText(Application.dataPath + "/logindetails.txt", ""); //If a file does not exist then a new file is created at the specified location
        }
    }

    public void OnRegisterButtonPress() //This will execute when the "Register" button has been pressed
    {
        LoginUI.SetActive(false);
        RegisterUI.SetActive(true);
    }

    public void OnBackButtonPress() //This will execute when the "Back" button has been pressed
    {
        LoginUI.SetActive(true);
        RegisterUI.SetActive(false);
    }

    public void Register() //This method controls the registration of users into the program
    {
        bool duplicateUser = false; //Boolean variables to ensure that the user input is valid
        bool acceptableInput = true;

        users = new ArrayList(File.ReadAllLines(Application.dataPath + "/logindetails.txt")); //All of the entries in the textfile are read into the arrayList called "users"
        foreach (var user in users) //Cycle through every entry in the textfile to see if a duplicate username exists
        {
            if (user.ToString().Contains(usernameInputField.text)) //If a duplicate user exists then set the boolean value to true
            {
                duplicateUser = true; //
                break; //Breaks from the loop since we have found the duplicate user
            }
        }

        if (usernameInputField.text.Contains(':') || passwordInputField.text.Contains(':')) //The username or password cannot contain the colon character as this is used to separate the username and password within the text file
        {
            acceptableInput = false; //If a colon character is detected then the boolean value is set to false
            messageText.color = Color.red; //Message colour set to red because this is an error message
            messageText.text = "Username or password cannot contain ':' as a character, please try again"; //This is the appropriate output message for this situation
        }

        if (acceptableInput) //If the input from the user does not contain a colon character
        {
            if (duplicateUser) //And also if the input for the username does not already exist within the text file
            {
                messageText.color = Color.red;
                messageText.text = "Username already exists"; //Output a message saying username already exists
            }
            else
            {
                if (passwordInputField.text == confirmPasswordInputField.text) //This ensures that the password and confirm password field match
                {
                    users.Add(usernameInputField.text + ":" + passwordInputField.text); //If all of the checks are completed successfully then the entry is added to the text file in this format
                    File.WriteAllLines(Application.dataPath + "/logindetails.txt", (String[])users.ToArray(typeof(string))); //The text list of users are added back to the text file
                    messageText.color = Color.white;
                    messageText.text = "User is registered"; //Output message describing a successful registration
                }
                else
                {
                    messageText.color = Color.red;
                    messageText.text = "Passwords don't match"; //If the passwords dont match then this is shown to the user
                }

            }
        }
        

    }

    public void Login() //This script controls the login to the application
    {
        bool validUser = false; //Boolean for a check to see if the user exists

        users = new ArrayList(File.ReadAllLines(Application.dataPath + "/logindetails.txt"));

        foreach (var user in users) //For each entry in the textfile
        {
            string line = user.ToString();
            if (user.ToString().Substring(0,user.ToString().IndexOf(":")).Equals(usernameLoginInputField.text) && user.ToString().Substring(user.ToString().IndexOf(":") + 1).Equals(passwordLoginInputField.text)) //Checks to see if there is an entry that matches the inputted values
            {
                validUser = true; //If a match of a username and password are found, then the boolean check is set to true
                break;
            }
        }

        if (validUser) //If the boolean is valid then the user is logged in
        {
            Debug.Log($"Logging in '{usernameLoginInputField.text}'"); //Appropriate message displayed to the user
            SceneManager.LoadScene("MainMenu");
            username = usernameLoginInputField.text; //the static variable for the username has its value assigned


        }
        else
        {
            messageLoginText.color = Color.red;
            messageLoginText.text = "Invalid/Incorrect details, if you don't have an account then please register"; //If a valid user is not found then this message is output to the user


        }


    }
}
