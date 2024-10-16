using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] public GameObject MainUI; //The gameobjects for the three parts of the main menu are loaded
    [SerializeField] public GameObject SettingUI;
    [SerializeField] public GameObject LeaderboardUI;

    [SerializeField] public Slider VolumeSlider; //Refrence to the volume slider
    [SerializeField] public TMP_Text VolumeText;

    public void StartSimulation() //If the user presses the "start simulation" button then the scene is switched to that of the main menu
    {
        SceneManager.LoadScene("MainSimulation");
    }

    public void OpenSettingsScreen() //Upon pressing the settings button, the user is directed to the settings screen
    {
        MainUI.SetActive(false);
        LeaderboardUI.SetActive(false);
        SettingUI.SetActive(true);
    }

    public void OpenLeaderboardScreen() //Upon pressing the button marked "leaderboard", the user is directed to the leaderboard screen
    {
        MainUI.SetActive(false);
        LeaderboardUI.SetActive(true);
        SettingUI.SetActive(false);
    }

    public void ReturnToMainMenu() //Pressing the "back" button in any of the menues brings the user back to the main menu
    {
        MainUI.SetActive(true);
        LeaderboardUI.SetActive(false);
        SettingUI.SetActive(false);
    }

    public void VolumeControl(float Volume) //Displays the text to the user, showing the level that the volume is set at
    {
        VolumeText.text = Volume.ToString();
    }

    public void SaveVolume() //This method stores the volume set by the user to memory for the local game session
    {
        float value = VolumeSlider.value;
        PlayerPrefs.SetFloat("Volume", value);
        LoadVolumeValue(); //This sets the volume if there is already a value stored within the memory

    }

    void LoadVolumeValue() //This method retrieves the value from memory for the local session
    {
        float volumeVal = PlayerPrefs.GetFloat("Volume");
        VolumeSlider.value = volumeVal;
        AudioListener.volume = volumeVal;
    }
}
