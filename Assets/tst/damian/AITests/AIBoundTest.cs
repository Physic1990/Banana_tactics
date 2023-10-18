using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AIBoundTest
{
    private List<GameObject> playerUnits = new List<GameObject>();
    private List<GameObject> enemyUnits = new List<GameObject>();
    AI EnemyAI;

    GameObject unit = new GameObject();
    
    // A Test behaves as an ordinary method
    [Test]
    public void AIClostestUnitFound()
    {
        unit.transform.position = new Vector2(12f, 12f);
        enemyUnits.Add(unit);
        unit.transform.position = new Vector2(10f, 10f);
        playerUnits.Add(unit);

        Assert.AreEqual(EnemyAI.FindClosestUnit(playerUnits, enemyUnits[0]), 0);
        
    }

    [Test]
    public void AIClostestUnitNotFound()
    {
        unit.transform.position = new Vector2(12f, 12f);
        enemyUnits.Add(unit);

        Assert.AreEqual(EnemyAI.FindClosestUnit(playerUnits, enemyUnits[0]), -1);

    }

}
