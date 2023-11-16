using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimeCounter : MonoBehaviour
{
    bool timeActive = false;
    public bool countUp = true;
    public int startMinutes;
    public float currentTime; 
    public Text currentTimeText;


    void Start()
    {
        // Starts the time when the game opens. 
        StartTime();
    }

    void Update()
    {
        if (timeActive == true) 
        {
            // Increments the time up, can be switched to a countdown by changing the + to a -.
            IncrementTime();
        }

        // Formats the time into 00:00.
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = time.ToString(@"mm\:ss");
    }

    // Method to start the time.
    public void StartTime() 
    {
        currentTime = startMinutes * 60;
        timeActive = true;
    }

    // Method to stop the time.
    public void StopTime() 
    {
        timeActive = false;
    }

    public void IncrementTime() 
    {
        if (countUp == true) 
        {
            currentTime = currentTime + Time.deltaTime;
        } 
        else 
        {
            currentTime = currentTime - Time.deltaTime;
        }

        if (currentTime >= 5999) 
            {
                timeActive = false;
                startMinutes = 0;
                StartTime();
                Debug.Log("Stopwatch exceeded maximum of 99:59");
            }
        if (currentTime <= 0) 
            {
                timeActive = false; 
                StartTime();
                Debug.Log("Timer Finished!");
            }
    }

}
