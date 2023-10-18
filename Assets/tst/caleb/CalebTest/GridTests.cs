using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework.Internal;
using System;

public class GridTest
{
    private GridManager grid;
    [UnitySetUp]
    public IEnumerator Setup()
    {
        // Load the desired scene for testing
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);

        yield return null; // Wait for a frame to let any potential Start methods execute
    }

    //Tests that grid manager does not accept movement beyond where the grid's right most bound is
    [Test]
    public void BoundaryRight()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridManager>();
        grid._cursorTile.transform.position = new Vector3(0,0,0);
        Tile temp = grid.MoveCursor(new Vector2(-1, 0));
        Assert.IsTrue(temp == grid._cursorTile);
    }
    //Tests that grid manager does not accept movement to the left where the grid's left most bound is

    [Test]
    public void BoundaryLeft()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridManager>();
        grid._cursorTile.transform.position = new Vector3(grid.Width-1, 0, 0);
        Tile temp = grid.MoveCursor(new Vector2(grid.Width+1, 0));
        Assert.IsTrue(temp == grid._cursorTile);
    }

    //Tests that grid manager does not accept movement above where the grid's upper bound is
    [Test]
    public void BoundaryUp()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridManager>();
        grid._cursorTile.transform.position = new Vector3(0, grid.Height, 0);
        Tile temp = grid.MoveCursor(new Vector2(0, grid.Height+1));
        Assert.IsTrue(temp == grid._cursorTile);
    }

    //Tests that grid manager does not accept movement below where the grid's lower bound is
    [Test]
    public void BoundaryDown()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridManager>();
        grid._cursorTile.transform.position = new Vector3(0, 0, 0);
        Tile temp = grid.MoveCursor(new Vector2(0, -1));
        Assert.IsTrue(temp == grid._cursorTile);
    }


    [Test]
    public void TestAlwaysPasses()
    {
        // This test always passes because we are asserting true.
        Assert.IsTrue(true);
    }

    //Stess test that tests efficency of Grid Manager class's ability to change tiles
    [UnityTest]
    public IEnumerator IncrementalStressTestTileSelection()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridManager>();

        float frameRateThreshold = 40.0f; //Will fail if the frame rate dips below 40 fps
        int delay = 10;
        int counter = 0;
        int maxIterations = 1000; // Maximum number of iterations
        int increment = 100; // Increment value

        for (int numIterations = increment; numIterations <= maxIterations; numIterations += increment)
        {
            while(counter < delay)
            {
                counter++;
            }
            for (int i = 0; i < numIterations; i++)
            {
                Vector2 randomCursorPosition = new Vector2(UnityEngine.Random.Range(0, grid.Width+10), UnityEngine.Random.Range(0, grid.Height+10));

                // Move the cursor to a random position
                grid.MoveCursor(randomCursorPosition);

                // Check the frame rate
                float frameRate = 1.0f / Time.deltaTime;

                // Add an assertion to fail if the frame rate is below the threshold
                Assert.GreaterOrEqual(frameRate, frameRateThreshold, "Frame rate below threshold");

                // Yield a frame to allow any updates to occur
                yield return null;
            }
            counter = 0;
            delay--;
        }
    }
}
