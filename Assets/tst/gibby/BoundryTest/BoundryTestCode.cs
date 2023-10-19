using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using static UnityEngine.UI.CanvasScaler;

public class BoundryTestCode
{
    private GameObject Unit = new GameObject();
    private UnitAttributes unitAttributes;
    // A Test behaves as an ordinary method
    [Test]
    public void NoAbove100()
    {
        unitAttributes = Unit.AddComponent<UnitAttributes>();

        unitAttributes.SetHealth(101);
        Assert.AreEqual(100, unitAttributes.GetHealth());
    }

    [Test]
    public void NoBelowZero()
    {
        unitAttributes = Unit.GetComponent<UnitAttributes>();

        unitAttributes.SetHealth(-10);
        Assert.AreEqual(0, unitAttributes.GetHealth());
    }
}
