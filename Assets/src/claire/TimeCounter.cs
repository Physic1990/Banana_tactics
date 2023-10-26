using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimeCounter : MonoBehaviour
{
    bool timeActive = false;
    public int startMinutes;
    float currentTime; 
    public Text currentTimeText;

    void Start()
    {
        // Initializes the time.
        currentTime = startMinutes * 60;

        // Starts the time when the game opens. 
        StartTime();
    }

    void Update()
    {
        if (timeActive == true) 
        {
            // Increments the time up, can be switched to a countdown by changing the + to a -.
            currentTime = currentTime - Time.deltaTime;

            // If using a timer, it won't go to zero.
            if (currentTime <= 0) 
            {
                timeActive = false; 
                Start();
                Debug.Log("Timer Finished!");
            }

            if (currentTime >= 5999) 
            {
                timeActive = false;
                Start();
                Debug.Log("Stopwatch exceeded maximum of 99:59");
            }
        }
        // Formats the time into 00:00.
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = time.ToString(@"mm\:ss");
    }

    // Method to start the time.
    public void StartTime() 
    {
        timeActive = true;
    }

    // Method to stop the time.
    public void StopTime() 
    {
        timeActive = false;
    }
}
