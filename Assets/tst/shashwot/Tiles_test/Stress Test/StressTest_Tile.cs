/*
   StressTest_Tile.cs

   This script demonstrates a stress test, designed to assess system performance
   under heavy load. The stress test involves spawning a large number of tiles using the SpawnTiles
   method in a controlled loop specified by the stressTestIterations variable.

   Adjust the stressTestIterations variable to control the intensity of the stress test. Higher values
   will put more strain on the system.

   The StressTest method monitors the frame rate during and after the stress test. If the frame rate drops
   below 30 frames per second, a message is logged to indicate potential performance issues.

   Note: This explicit stress test is intended for assessing how well the system handles the tile spawning
   operation and identifying any performance bottlenecks.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressTest_Tile : MonoBehaviour
{
    public GameObject tilePrefab;
    public int gridSizeX = 10;
    public int gridSizeY = 10;
    public int stressTestIterations = 100; // Adjust this number. I have crashed my computer many times so you can lower the value. 

    void Start()
    {
        SpawnTiles();
        StressTest();
    }

    // Method for spawning tiles in a grid pattern. The SpawnTiles method is responsible for creating and initializing a grid of tiles. 
    // It uses nested loops to iterate through the specified gridSizeX and gridSizeY, creating a tile GameObject for each grid position.
    void SpawnTiles()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 tilePosition = new Vector3(x, y, 0);
                GameObject newTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                newTile.GetComponent<Tile>().Init((x + y) % 2 == 0); // Alternate colors based on position
                Debug.Log("Tile Position: " + newTile.transform.position);
            }
        }
    }

    // Method for performing the stress test
    void StressTest()
    {
        for (int i = 0; i < stressTestIterations; i++)
        {
            SpawnTiles();
        }

        // Check and log frame rate after the stress test
        float frameRate = 1.0f / Time.deltaTime;

        if (frameRate < 30.0f)
        {
            Debug.Log("Framerate has dropped below thresh.");
        }
    }
}