using NUnit.Framework;
using UnityEngine;

public class TileTests 
{
    [Test]
    public void AssignUnit_SetsOccupied()
    {
        // Arrange
        GameObject tileObject = new GameObject("Tile");
        Tile tile = tileObject.AddComponent<Tile>();
        GameObject unit = new GameObject();

        // Act
        tile.AssignUnit(unit);

        // Assert
        Assert.IsTrue(tile._occupied);
    }

    [Test]
    public void RemoveUnit_ClearsOccupied()
    {
        // Arrange
        GameObject tileObject = new GameObject("Tile");
        Tile tile = tileObject.AddComponent<Tile>();
        GameObject unit = new GameObject();
        tile.AssignUnit(unit);

        // Act
        GameObject removedUnit = tile.RemoveUnit();

        // Assert
        Assert.IsFalse(tile._occupied);
        Assert.IsNull(tile._unit);
    }
}