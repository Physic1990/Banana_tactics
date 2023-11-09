/*
Boundary Test Explanation:

The TileTests script contains two boundary test methods, AssignUnit_SetsOccupied and RemoveUnit_ClearsOccupied,
designed to ensure the proper functionality of the AssignUnit and RemoveUnit methods within the Tile class.
These tests focus on the extreme conditions of assigning and removing units to validate the behavior of the Tile class
in scenarios where a unit is assigned or removed, verifying that the _occupied flag and _unit field are updated correctly.
The tests use the NUnit framework to set up, act, and assert the expected outcomes.
*/
using NUnit.Framework;
using UnityEngine;

public class TileTests 
{
    [Test]
    public void AssignUnit_SetsOccupied()
    {
        // Arrange
        // Creating a GameObject to simulate a Tile in the scene.
        GameObject tileObject = new GameObject("Tile");
        // Adding the Tile component to the GameObject.
        Tile tile = tileObject.AddComponent<Tile>();
        // Creating a GameObject to simulate a Unit in the scene.
        GameObject unit = new GameObject();

        // Act
        // Assigning the Unit to the Tile using the AssignUnit method.
        tile.AssignUnit(unit);

        // Assert
        // Verifying that the Tile is marked as occupied after assigning the Unit.
        Assert.IsTrue(tile._occupied);
    }

    [Test]
    public void RemoveUnit_ClearsOccupied()
    {
        // Arrange
        // Creating a GameObject to simulate a Tile in the scene.
        GameObject tileObject = new GameObject("Tile");
        // Adding the Tile component to the GameObject.
        Tile tile = tileObject.AddComponent<Tile>();
        // Creating a GameObject to simulate a Unit in the scene.
        GameObject unit = new GameObject();
        // Assigning the Unit to the Tile to simulate an initially occupied Tile.
        tile.AssignUnit(unit);

        // Act
        // Removing the Unit from the Tile using the RemoveUnit method.
        GameObject removedUnit = tile.RemoveUnit();

        // Assert
        // Verifying that the Tile is no longer occupied after removing the Unit.
        Assert.IsFalse(tile._occupied);
        // Verifying that the _unit field is set to null after removing the Unit.
        Assert.IsNull(tile._unit);
    }
}
