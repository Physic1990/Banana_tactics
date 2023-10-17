using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
 
    public void AITurn( List<GameObject> PlayerUnits, List<GameObject> EnemyUnits)
    {

        for (int i = 0; i < EnemyUnits.Count; i++)
        {
            Debug.Log(EnemyUnits[i]);
        }
    }






}
