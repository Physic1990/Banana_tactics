using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    private TimeCounter timeScript;
    private float finalTime;

    void Start() 
    {

    }

    void Update() 
    {
        //Calcuates the score when the N key is pressed for testing
        if (Input.GetKeyDown(KeyCode.N)) 
        {
            int stars = CalculateScore();
            Debug.Log("Score:" + stars);
        }
    }

    public int CalculateScore()
    {
        int score;
        // Get the time from the stopwatch
        finalTime = timeScript.currentTime;

        // Calculate the score based on the time
        if (finalTime <= 120) 
        {
            score = 3;
        } 
        else if (finalTime <= 240) 
        {
            score = 2;
        }
        else 
        {
            score = 1;
        }
        return score;
    }
}
