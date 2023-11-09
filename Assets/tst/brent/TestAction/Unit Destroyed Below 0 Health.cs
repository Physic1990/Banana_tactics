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
      var test1 = new GameObject().AddComponent<UnitAttributes>();
      double [] attributes = test1.GetAttackOneStats();
      Assert.IsNotNull(attributes[5]);
      
    }
}
