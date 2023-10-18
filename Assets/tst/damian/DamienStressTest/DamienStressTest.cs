using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamienStressTest : MonoBehaviour
{

    AI EnemyAi = new AI();
    List<GameObject> UnitsList = new List<GameObject>();
    List<GameObject> OtherUnitsList = new List<GameObject>();
    float incVal = 1f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i <= incVal; i++)
        {
            EnemyAi.AITurn(UnitsList, OtherUnitsList);
        }
        
        
        float frameRate = 1.0f / Time.deltaTime;
    
        if (frameRate < 30.0f)
        {
            Debug.Log("Framerate has dropped below thresh.");
            Debug.Log(incVal);
        }

        incVal += 10000f;
       
    }


}
