using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class BCModeTrigger : MonoBehaviour
{
    [SerializeField] public bool BCMode = false;

    public void SwitchMode()
    {
        if(BCMode == false)
        {
            BCMode = true;
        } 
        else
        {
            BCMode = false;
        }
    }
}
