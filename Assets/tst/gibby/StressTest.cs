using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressTest : MonoBehaviour
{
    public GameObject Unit;
    public bool runTest = false;

    private float fps;
    private int totalUnits = 1;
    private bool stopUnitMake = false;
    private int frameCheck = 60;
    private float buffer;
    void Update()
    {
        if (runTest == true)
        {
            fps = (int)(1f / Time.unscaledDeltaTime);
            //Debug.Log(fps);

            if (stopUnitMake == false)
            {
                GameObject go = GameObject.Instantiate(Unit);
                go.transform.position = transform.position;
                totalUnits++;
            }

            if ((fps <= frameCheck) && (totalUnits >= 10))
            {
                Debug.Log(totalUnits + " total units made before" + frameCheck + " drop");
                stopUnitMake = true;
            }
        }
    }
}
