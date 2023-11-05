using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HealEventTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void Unit_has_health_attribute()
    {
      var test1 = new GameObject().AddComponent<UnitAttributes>();
      double [] attributes = test1.GetAttackOneStats();
      Assert.IsNotNull(attributes[5]);
      
    }
    [Test]
    public void Unit_health_attribute_not_negative()
    {
      var test2 = new GameObject().AddComponent<UnitAttributes>();
      double [] attributes = test2.GetAttackOneStats();
      Assert.IsTrue(attributes[5]>=0 , "heal attribute is greater than 0");
    }
}