using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Stopwatch : MonoBehaviour
{
    bool stopwatchActive = false;
    float currentTime; 
    public Text currentTimeText;

    void Start()
    {
        // Initializes the time.
        currentTime = 0;

        // Starts the time when the game opens. 
        StartStopwatch();
    }

    void Update()
    {
        if (stopwatchActive == true) 
        {
            // Increments the time up, can be switched to a countdown by changing the + to a -.
            currentTime = currentTime + Time.deltaTime;

        }
        // Formats the time into 00:00.
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = time.ToString(@"mm\:ss");
    }

    // Method to start the time.
    public void StartStopwatch() 
    {
        stopwatchActive = true;
    }

    // Method to stop the time.
    public void StopStopwatch() 
    {
        stopwatchActive = false;
    }
}
