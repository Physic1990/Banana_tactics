using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class BCModeTrigger : MonoBehaviour
{
    [SerializeField] public bool BCMode = false;

    void Awake()
    {
        BCMode = PlayerPrefs.GetInt("DrBCMode") == 1 ? true : false;
    }

    public void SwitchMode()
    {
        if (BCMode == false)
        {
            BCMode = true;
        }
        else
        {
            BCMode = false;
        }
    }
}
