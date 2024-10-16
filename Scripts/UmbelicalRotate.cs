using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbelicalRotate : MonoBehaviour
{
    public bool FuelingOnline = false;
    [SerializeField]GameObject Pivot;
    Quaternion target_angle90 = Quaternion.Euler(0, 0, 90);
    Quaternion target_angle0 = Quaternion.Euler(0, 0, 0);
    public Quaternion currentAngle;

    public void Start()
    {
        Pivot = new GameObject();
        currentAngle = target_angle90;
    }


    public void Update()
    {
        if (FuelingOnline)
        {
            ChangeCurrentAngle();
        }
        Pivot.transform.rotation = Quaternion.Slerp(Pivot.transform.rotation, currentAngle, 0.2f);
    }

    public void ChangeCurrentAngle()
    {
        if (currentAngle.eulerAngles.z == target_angle90.eulerAngles.z)
        {
            currentAngle = target_angle0;
        }
        else
        {
            currentAngle = target_angle90;
        }
    }

}
