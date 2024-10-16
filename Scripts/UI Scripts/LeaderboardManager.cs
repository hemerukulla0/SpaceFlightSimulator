using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine.UIElements;

public class LeaderboardManager : MonoBehaviour
{
    public RowUI rowUI; //Refrence to the rowUI script
    public static string filepath; //A static string containing the address to the file
    public string[] tempArray;
    public List<Entry> entries; //A list of a custom designed data structure that stores a username and a score value for each variable
    public static List<Entry> sortedList; //A list that will eventually contain the sorted leaderboard 
    public static bool isUpdated; 
    
    void Awake()
    {
        filepath = Application.dataPath + "/highscores.txt";
        entries = new List<Entry>(); //Initializing the list of the entries
        isUpdated = true;
    }

    public void LoadEntries()
    {
        while (isUpdated)
        {
            tempArray = File.ReadAllLines(Application.dataPath + "/highscores.txt");

            foreach (string entry in tempArray) //For every record in the leaderboard 
            {
                Entry tempEntry;
                tempEntry = new Entry(entry.Substring(0, entry.IndexOf(":")), Int32.Parse(entry.Substring(entry.IndexOf(":") + 1))); //The username and score are passed into the constructor for the new entry

                entries.Add(tempEntry);
            }

            sortedList = entries.OrderByDescending(x => x.score).ToList(); //lambda expression used for the parameter, the orderbydescending used to sort the list into order


            int count = 0;
            foreach (var entry in sortedList) //For every entry in the sorted list:
            {
                count++;
                var row = Instantiate(rowUI, transform).GetComponent<RowUI>(); //Insert a prefab into the leaderboard 
                row.rank.text = count.ToString(); //Fill the details for the three coloumns: Rank, username and score
                row.score.text = entry.score.ToString();
                row.username.text = entry.username;
            }
            isUpdated = false;
        }
        
        
    }



    public static void EnterNewValue(string concatenatedOutput) //this method adds a new entry to the leaderboard, this method is a static method since it can be accessed from anywhere
    {

        using (StreamWriter sw = File.AppendText(Application.dataPath + "/highscores.txt")) //Streamwriter module used to write the new entry into the leaderboard
        {
            sw.WriteLine(concatenatedOutput); 
        }
    }

    
}

public class Entry //This is a data structure that i have designed myself in order to store both a username and score within a single variable
{
    public string username;
    public int score;

    public Entry(string username, int score) //Constructor for the Entry data type
    {
        this.username = username;
        this.score = score;
    }
}
