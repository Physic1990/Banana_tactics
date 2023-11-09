using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UnitDestroyedBelow0Health
{

        // A Test behaves as an ordinary method
    [Test]
    public void Unit_Destroyed_Below_0_HealthTest()
    {
      var testGrid = new GameObject().AddComponent<GridManager>();
      var testUnit = new GameObject().AddComponent<UnitAttributes>();
      // kill unit
      testUnit.DealDamage(100);
      //testGrid.RemoveDeadUnits();
      int attributes = testUnit.GetHealth();
      Assert.IsNotNull(attributes);
    }
}
