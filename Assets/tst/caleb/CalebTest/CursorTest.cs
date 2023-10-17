using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class CursorTest
{
    private CursorController cursorController;
    [UnitySetUp]
    public IEnumerator Setup()
    {
        // Load the desired scene for testing
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);

        yield return null; // Wait for a frame to let any potential Start methods execute
    }

    //Tests that cursor moves in the anticipated direction
    [UnityTest]
    public IEnumerator CursorMovesRight()
    {
        var cursorController = new GameObject().AddComponent<CursorController>();
        
        Vector3 initialPosition = cursorController.transform.position;
        Vector2 MoveRight = new Vector2(initialPosition.x + 1, initialPosition.y);
        // Simulate moving the cursor right (you may need to expose this functionality in GridManager)
        cursorController.MoveCursor(MoveRight);

        // Wait for a frame to let any updates occur
        yield return null;

        // Get the updated position
        Vector2 updatedPosition = new Vector2(cursorController.transform.position.x, cursorController.transform.position.y);
        Debug.Log(MoveRight.x);
        Debug.Log(updatedPosition.x);
        // Check if the cursor has moved one unit to the right
        Assert.AreEqual(MoveRight, updatedPosition);
    }
    //Tests that cursor moves in the anticipated direction

    [UnityTest]
    public IEnumerator CursorMovesLeft()
    {
        var cursorController = new GameObject().AddComponent<CursorController>();

        Vector3 initialPosition = cursorController.transform.position;
        Vector2 moveLeft = new Vector2(initialPosition.x - 1, initialPosition.y);
        cursorController.MoveCursor(moveLeft);

        // Wait for a frame to let any updates occur
        yield return null;

        // Get the updated position
        Vector2 updatedPosition = new Vector2(cursorController.transform.position.x, cursorController.transform.position.y);

        // Check if the cursor has moved one unit to the left
        Assert.AreEqual(moveLeft, updatedPosition);
    }

    //Tests that cursor moves in the anticipated direction
    [UnityTest]
    public IEnumerator CursorMovesUp()
    {
        var cursorController = new GameObject().AddComponent<CursorController>();

        Vector3 initialPosition = cursorController.transform.position;
        Vector2 moveUp = new Vector2(initialPosition.x, initialPosition.y + 1);
        cursorController.MoveCursor(moveUp);

        // Wait for a frame to let any updates occur
        yield return null;

        // Get the updated position
        Vector2 updatedPosition = new Vector2(cursorController.transform.position.x, cursorController.transform.position.y);

        // Check if the cursor has moved one unit up
        Assert.AreEqual(moveUp, updatedPosition);
    }

    //Tests that cursor moves in the anticipated direction
    [UnityTest]
    public IEnumerator CursorMovesDown()
    {
        var cursorController = new GameObject().AddComponent<CursorController>();

        Vector3 initialPosition = cursorController.transform.position;
        Vector2 moveDown = new Vector2(initialPosition.x, initialPosition.y - 1);
        cursorController.MoveCursor(moveDown);

        // Wait for a frame to let any updates occur
        yield return null;

        // Get the updated position
        Vector2 updatedPosition = new Vector2(cursorController.transform.position.x, cursorController.transform.position.y);

        // Check if the cursor has moved one unit down
        Assert.AreEqual(moveDown, updatedPosition);
    }


    [Test]
    public void TestAlwaysPasses()
    {
        // This test always passes because we are asserting true.
        Assert.IsTrue(true);
    }
}
