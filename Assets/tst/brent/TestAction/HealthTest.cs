using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NewTestScript
{
    // A Test behaves as an ordinary method
    [Test]
    public void Unit_Has_No_Health()
    {
        ActionEventManager lowerBound = new ActionEventManager();
        lowerBound.setPlayerHealth(1);
      
       Assert.IsTrue(lowerBound.getUpdatePlayerHealth()>0, "Health is at lower bounds, greater than 0");
      
    }

    [Test]
    public void Unit_Health_Is_In_Boundary()
    {
        ActionEventManager lowerBound = new ActionEventManager();
        lowerBound.setPlayerHealth(0);

        Assert.IsTrue(lowerBound.getUpdateEnemyHealth()>0, "Health is not in bounds, less than or equal to 0");
    }
}

