using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AI : MonoBehaviour
{
 
<<<<<<< Updated upstream
    public void AITurn(bool IsPlayerTurn)
    {
        Debug.Log("AI takes its turn");
        IsPlayerTurn = false;
=======
    public void AITurn(List<GameObject> PlayerUnits, List<GameObject> EnemyUnits)
    {
        for (int i = 0; i < EnemyUnits.Count; i++)
        {
            FindClosestUnit(PlayerUnits, EnemyUnits[i]);
        }

>>>>>>> Stashed changes
    }

    private int FindClosestUnit(List<GameObject> PlayerUnits, GameObject EnemyUnit)
    {
            Debug.Log(EnemyUnit);
            int j = 0;
            float Mindistance = (Math.Abs(EnemyUnit.transform.position.x - PlayerUnits[j].transform.position.x) + Math.Abs(EnemyUnit.transform.position.y - PlayerUnits[j].transform.position.y));
            float distance = 0f;
            int closestUnit = 0;
            for (j = 1; j < PlayerUnits.Count; j++)
            {
                distance = (Math.Abs(EnemyUnit.transform.position.x - PlayerUnits[j].transform.position.x) + Math.Abs(EnemyUnit.transform.position.y - PlayerUnits[j].transform.position.y));
                if (Mindistance > distance)
                {
                    closestUnit = j;
                    Mindistance = distance;
                }
            }
            return closestUnit;

<<<<<<< Updated upstream
=======
    }
>>>>>>> Stashed changes

}
