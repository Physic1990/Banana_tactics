using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

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
    
    //Cursor does not go too far right off the grid
    [Test]
    public void BoundaryRight()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridManager>();
        grid._cursorTile.transform.position = new Vector3(0,0,0);
        Tile temp = grid.MoveCursor(new Vector2(-1, 0));
        Assert.IsTrue(temp == grid._cursorTile);
    }

    [Test]
    public void BoundaryLeft()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridManager>();
        grid._cursorTile.transform.position = new Vector3(grid.Width-1, 0, 0);
        Tile temp = grid.MoveCursor(new Vector2(grid.Width+1, 0));
        Assert.IsTrue(temp == grid._cursorTile);
    }

    [Test]
    public void BoundaryUp()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridManager>();
        grid._cursorTile.transform.position = new Vector3(0, grid.Height, 0);
        Tile temp = grid.MoveCursor(new Vector2(0, grid.Height+1));
        Assert.IsTrue(temp == grid._cursorTile);
    }

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
}
