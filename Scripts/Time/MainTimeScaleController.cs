using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTimeScaleController : MonoBehaviour
{
    [SerializeField][Range(1,100)] //The value can be between 1 - 100
    public float modifyScale;

    void Start()
    {
        modifyScale = 1; //by default the timescale is set to 1
    }

    void Update()
    {
        Time.timeScale = modifyScale; //This line of code increases the rate at which the simulation runs, by using the unity timscale function
    }

    public float ReturnTimeScale()
    {
        return modifyScale;
    }
}
