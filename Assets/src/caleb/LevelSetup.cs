using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetup : MonoBehaviour
{
    public static LevelSetup Instance { get; private set; }

    // Prefabs For Units:
    [SerializeField] private GameObject warriorPrefab;
    [SerializeField] private GameObject lancerPrefab;
    [SerializeField] private GameObject gunslingerPrefab;
    [SerializeField] private GameObject roguePrefab;
    [SerializeField] private GameObject heroPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private TextAsset textAsset;
    GridManager grid;
    private int height;
    private int width;

    private Dictionary<string, GameObject> classToPrefab; // Map class names to prefabs

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridManager>();
        height = grid.Height;
        width = grid.Width;

        // Initialize the class-to-prefab mapping
        classToPrefab = new Dictionary<string, GameObject>
        {
            { "warrior", warriorPrefab },
            { "lancer", lancerPrefab },
            { "gunslinger", gunslingerPrefab },
            { "rogue", roguePrefab },
            { "hero", heroPrefab }
        };
    }

    /*************************************************************************
      Generates Units on Grid (do not grade)
    ************************************************************************/
    public void GenerateUnits()
    {
        ReadLevelInfo();
    }

    public void SpawnUnitAt(string tag, string className, int x, int y, int id)
    {
        Tile tile = grid.GetTileAtPosition(new Vector2(x, y));
        if (tag == "player" && classToPrefab.ContainsKey(className) && tile != null && !tile._occupied)
        {
            GameObject prefab = classToPrefab[className];
            var unit = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            tile.AssignUnit(unit);
            grid._playerUnits.Add(unit);
            unit.name = $"{tag} {className} {id}";
        }
        else if (tag == "enemy" && tile != null && !tile._occupied)
        {

            var unit = Instantiate(enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            tile.AssignUnit(unit);
            grid._enemyUnits.Add(unit);
            unit.name = $"{tag} {className} {x}-{y}";
        }
        else if(tile == null)
        {
            Debug.LogWarning($"Out of Bounds Tile: {x} , {y}");
        }
        else
        {
            Debug.LogWarning($"Prefab not found for class: {className}");
        }
    }

    public void ReadLevelInfo()
    {
        if (textAsset != null)
        {
            string[] lines = textAsset.text.Split('\n');
            foreach (string line in lines)
            {
                string[] parts = line.Trim().Split('|');

                if (parts.Length == 5)
                {
                    string tag = parts[0].Trim();
                    string classType = parts[1].Trim();
                    int x, y, id;
                    if (int.TryParse(parts[2].Trim(), out x) && int.TryParse(parts[3].Trim(), out y) && int.TryParse(parts[4].Trim(), out id))
                    {
                        if(x >= width)
                        {
                            x = width - 1;
                        }
                        else if (x < 0)
                        {
                            x = 0;
                        }
                        else if (y >= height)
                        {
                            y = height - 1;
                        }
                        else if (y < height)
                        {
                            y = 0;
                        }
                        SpawnUnitAt(tag, classType, x, y, id);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("TextAsset is not assigned. Assign a TextAsset in the Inspector.");
        }
    }
}
