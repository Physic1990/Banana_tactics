using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class CursorTest : MonoBehaviour
{

    [UnitySetUp]
    public IEnumerator Setup()
    {
        // Initialize and set up your test environment, such as creating the game objects and controllers.
        // This method runs before each test.
        yield return null; // Wait for a frame to let any potential Start methods execute.
    }

    [UnityTest]
    public void MoveCursorLeft()
    {
        // Your test logic here
        Assert.IsTrue(true); // Example assertion
    }
}
