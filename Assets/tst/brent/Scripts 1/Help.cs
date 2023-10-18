using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameObject theBall;

    int PlayerScore2 = 10;

    public GUISkin layout;


    // Use this for initialization
    void Start()
    {
        theBall = GameObject.FindGameObjectWithTag("Ball");
    }

  

    void OnGUI()
    {
        //GUI.skin = layout;
        GUI.Label(new Rect(Screen.width /2 - 120, 45, 280, 53), "Number Of Dimension Battles:");
        GUI.Label(new Rect(Screen.width/2  + 80, 45, 100, 100), "" + PlayerScore2);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
       
    }
}

