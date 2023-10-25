using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AI : MonoBehaviour
{
 

    public void AITurn(List<GameObject> playerUnits, List<GameObject> enemyUnits)
    {
        for (int i = 0; i < enemyUnits.Count; i++)
        {
            //find the target to move toward or attack
            int _targetUnit = FindClosestUnit(playerUnits, enemyUnits[i]); 

            //move

            //attack or heal

        }

    }

    public int FindClosestUnit(List<GameObject> playerUnits, GameObject enemyUnit) //Finds the closest unit to the current controlled unit
    {
        int j = 0;
        float _mindistance = Mathf.Infinity;
        float _distance = 0f;
        Debug.Log(playerUnits.Count);
        int _closestUnit = -1;
        for (j = 0; j < playerUnits.Count; j++)
        {
            // |x1-x2| + |y1-y2| distance formula
            _distance = (Math.Abs(enemyUnit.transform.position.x - playerUnits[j].transform.position.x) + Math.Abs(enemyUnit.transform.position.y - playerUnits[j].transform.position.y)); 
            if (_mindistance > _distance)
            {
                _closestUnit = j;
                _mindistance = _distance;
            }
        }
        return _closestUnit;


    }


}
