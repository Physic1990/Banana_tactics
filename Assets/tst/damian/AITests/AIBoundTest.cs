using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AIBoundTest
{
    private List<GameObject> playerUnits = new List<GameObject>();
    private List<GameObject> enemyUnits = new List<GameObject>();
    AI EnemyAI = new AI();

    GameObject unit = GameObject.FindWithTag("Enemy");
    GameObject unit2 = GameObject.FindWithTag("Player");

    // A Test behaves as an ordinary method
    [Test]
    public void AIClostestUnitFound()
    {
        unit.transform.position = new Vector2(10f, 10f);
        enemyUnits.Add(unit);
        unit2.transform.position = new Vector2(12f, 12f);
        playerUnits.Add(unit2);
        int target = EnemyAI.FindClosestUnit(playerUnits, enemyUnits[0]);
        Assert.AreEqual(0, target);
        
    }

    [Test]
    public void AIClostestUnitNotFound()
    {
        unit.transform.position = new Vector2(12f, 12f);
        enemyUnits.Add(unit);

        Assert.AreEqual(-1, EnemyAI.FindClosestUnit(playerUnits, enemyUnits[0]));

    }

}
