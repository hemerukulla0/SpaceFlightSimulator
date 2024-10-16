using Console;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutoCompleteButtons : MonoBehaviour
{
    [SerializeField]
    public TMP_Text Button1Text;
    public TMP_Text Button2Text;

    GameObject manager;

    private void Start()
    {
        manager = FindObjectOfType<InputConsole>().gameObject;
    }

    public void Button1Click()
    {
        if (Button1Text.text != "-")
        {
            manager.GetComponent<InputConsole>().myCommandInputField.text = Button1Text.text;
        }
        
    }

    public void Button2Click()
    {
        if (Button2Text.text != "-")
        {
            manager.GetComponent<InputConsole>().myCommandInputField.text = Button2Text.text;
        }
        
    }
}
