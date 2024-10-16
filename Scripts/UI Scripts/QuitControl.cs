using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitControl : MonoBehaviour
{
   public void OnQuitButtonPress() //Upon pressing the quit button, the application closes
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
