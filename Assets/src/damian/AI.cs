using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AI : MonoBehaviour
{
 

    public void AITurn(List<GameObject> PlayerUnits, List<GameObject> EnemyUnits)
    {
        for (int i = 0; i < EnemyUnits.Count; i++)
        {
            int TargetUnit = FindClosestUnit(PlayerUnits, EnemyUnits[i]);
        }


    }

    public int FindClosestUnit(List<GameObject> PlayerUnits, GameObject EnemyUnit)
    {
        int j = 0;
        float Mindistance = Mathf.Infinity;
        float distance = 0f;
        Debug.Log(PlayerUnits.Count);
        int closestUnit = -1;
        for (j = 0; j < PlayerUnits.Count; j++)
        {
            distance = (Math.Abs(EnemyUnit.transform.position.x - PlayerUnits[j].transform.position.x) + Math.Abs(EnemyUnit.transform.position.y - PlayerUnits[j].transform.position.y));
            if (Mindistance > distance)
            {
                closestUnit = j;
                Mindistance = distance;
            }
        }
        return closestUnit;


    }


}
