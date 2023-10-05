using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
 
    public void AITurn(bool IsPlayerTurn)
    {
        Debug.Log("AI takes its turn");
        IsPlayerTurn = false;
    }



}
