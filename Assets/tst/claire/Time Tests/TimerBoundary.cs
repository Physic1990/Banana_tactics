using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;

public class TimerBoundary : TimeCounter
{
    // A Test behaves as an ordinary method
    [Test]
    public void NoNegativeTime()
    {
        startMinutes = 1;
        countUp = false;
        //StartTime();
        currentTime = 0;
        IncrementTime();
        Assert.AreEqual(60, currentTime);
        //yield return null;
    }


    [UnityTest]
    public IEnumerator NoMaxTime()
    {
        startMinutes = 100;
        countUp = true;
        StartTime();
        IncrementTime();
        Assert.AreEqual(0, currentTime);
        yield return null;
    }
}
