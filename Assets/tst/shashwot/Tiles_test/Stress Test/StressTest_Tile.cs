using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressTest_Tile : MonoBehaviour
{
    public GameObject tilePrefab;
    public int gridSizeX = 10;
    public int gridSizeY = 10;
    public int stressTestIterations = 100; // Adjust this number

    void Start()
    {
        SpawnTiles();
        StressTest();
    }

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

    void StressTest()
    {
        for (int i = 0; i < stressTestIterations; i++)
        {
            SpawnTiles();
        }

        float frameRate = 1.0f / Time.deltaTime;

        if (frameRate < 30.0f)
        {
            Debug.Log("Framerate has dropped below thresh.");
        }
    }
}
