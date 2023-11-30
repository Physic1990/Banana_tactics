using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetup : MonoBehaviour
{
    public static LevelSetup Instance { get; private set; }

    // Prefabs For Units:
    [SerializeField] public GameObject warriorPrefab;
    [SerializeField] public GameObject lancerPrefab;
    [SerializeField] public GameObject gunslingerPrefab;
    [SerializeField] public GameObject roguePrefab;
    [SerializeField] public GameObject heroPrefab;
    [SerializeField] public GameObject enemyWarriorPrefab;
    [SerializeField] public GameObject enemyLancerPrefab;
    [SerializeField] public GameObject enemyGunslingerPrefab;
    [SerializeField] public GameObject enemyRoguePrefab;
    [SerializeField] public GameObject enemyHeroPrefab;
    //Text file that is read from for spawning units
    [SerializeField] public TextAsset textAsset;
    //References to gridmanager
    GridManager grid;
    private int height;
    private int width;
    //Designated player and enemy tags
    private string playerTag = "Player";
    private string enemyTag = "Enemy";
    //Teleport Tile Colors
    public Tile BlueOne;
    public Tile BlueTwo;
    public Tile RedOne;
    public Tile RedTwo;

    // Property for playerTag
    public string PlayerTag
    {
        get { return playerTag; }
    }

    // Property for enemyTag
    public string EnemyTag
    {
        get { return enemyTag; }
    }

    //Pattern: Singleton
    private void Awake()
    {
        //Make sure there's only one instance of level setup
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        //finds grid manager's script
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridManager>();
        height = grid.Height;
        width = grid.Width;
    }

    /*************************************************************************
      Generates Units on Grid (do not grade)
    ************************************************************************/

    //Generates units by reading information from a .txt related to the unit: Affiliation Class X-Coordinate Y-Coordinate ID
    //Example for generating a Player Warrior at (0,0) with id 1
    // Player | Warrior | 0 | 0 | 1
    //PATTERN: Builder (Reader)
    public void GenerateUnits()
    {
        if (textAsset != null)
        {
            //Reads input from designated .txt asset
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
                        //Checks that position is in bounds (otherwise sets it to be in bounds)
                        if (x >= width)
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
                        else if (y < 0)
                        {
                            y = 0;
                        }
                        SpawnUnitAt(tag, classType, x, y, id);
                    }
                }
                if(parts.Length == 3)
                {
                    string color = parts[0].Trim();
                    int x, y;
                    if (int.TryParse(parts[1].Trim(), out x) && int.TryParse(parts[2].Trim(), out y))
                    {
                        //Checks that position is in bounds (otherwise sets it to be in bounds)
                        if (x >= width)
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
                        else if (y < 0)
                        {
                            y = 0;
                        }
                        SpawnTeleportTile(color, x, y);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("TextAsset is not assigned. Assign a TextAsset in the Inspector.");
        }
        SetTeleportTiles();
    }

    //PATTERN: Builder (Converter)
    // Reader <> -----> Converter
    public void SpawnUnitAt(string tag, string className, int x, int y, int id)
    {
        Tile tile = grid.GetTileAtPosition(new Vector2(x, y));
        UnitPrefab BuildUnit = null;
        if (tag == playerTag)
        {
            BuildUnit = new UnitPrefab();
        }
        else if (tag == enemyTag)
        {
            BuildUnit = new EnemyPrefab();
        }
        else
        {
            Debug.LogWarning("Invalid Unit Tag");
        }
        BuildUnit.SetPrefab(className);
        //if the tile exists and does not contain an object, then assign it.
        if (tile != null && !tile._occupied)
        {
            var unit = Instantiate(BuildUnit.prefab, new Vector3(0, 0, 0), Quaternion.identity);
            tile.AssignUnit(unit);
            unit.name = $"{tag} {className} {id}";
            BuildUnit.AssignToList(unit);
        }
        else if (tile == null)
        {
            Debug.LogWarning($"Out of Bounds Tile: {x} , {y}");
        }
        else
        {
            Debug.LogWarning($"Prefab not found for class: {className}");
        }
    }

    public void SpawnTeleportTile(string color, int x, int y)
    {
        Tile tile = grid.GetTileAtPosition(new Vector2(x, y));
        //if the tile exists and does not contain an object, then assign it.
        if (tile != null)
        {
            if(color == "Blue" && BlueOne == null)
            {
                BlueOne = tile;
            }
            else if(color == "Blue" && BlueOne != tile)
            {
                BlueTwo = tile;
            }
            else if (color == "Red" && RedOne == null)
            {
                RedOne = tile;
            }
            else if (color == "Red" && RedOne != tile)
            {
                RedTwo = tile;
            }
        }
        else if (tile == null)
        {
            Debug.LogWarning($"Out of Bounds Tile: {x} , {y}");
        }
        else
        {
            Debug.LogWarning($"Color not found for tile: {color}");
        }
    }

    public void SetTeleportTiles()
    {
        if(BlueOne != null && BlueTwo != null)
        {
            BlueOne.MakeTeleport("Blue", 1);
            BlueTwo.MakeTeleport("Blue", 2);
        }
        if (RedOne != null && RedTwo != null)
        {
            RedOne.MakeTeleport("Red", 3);
            RedTwo.MakeTeleport("Red", 4);
        }
    }

    //Dynamic and Static Bindings
    public class UnitPrefab
    {
        public GameObject prefab = null;

        public virtual void SetPrefab(string className)
        {
            switch (className)
            {
                case "warrior":
                    prefab = LevelSetup.Instance.warriorPrefab;
                    break;
                case "lancer":
                    prefab = LevelSetup.Instance.lancerPrefab;
                    break;
                case "gunslinger":
                    prefab = LevelSetup.Instance.gunslingerPrefab;
                    break;
                case "rogue":
                    prefab = LevelSetup.Instance.roguePrefab;
                    break;
                case "hero":
                    prefab = LevelSetup.Instance.heroPrefab;
                    break;
                default:
                    Debug.LogWarning($"Prefab not found for Player class: {className}");
                    break;
            }
        }
        public virtual void AssignToList(GameObject unit)
        {
            LevelSetup.Instance.grid._playerUnits.Add(unit);
            Debug.Log("Added to player list");
        }
    }

    public class EnemyPrefab : UnitPrefab
    {
        
        public override void SetPrefab(string className)
        {
            switch (className)
            {
                case "warrior":
                    prefab = LevelSetup.Instance.enemyWarriorPrefab;
                    break;
                case "lancer":
                    prefab = LevelSetup.Instance.enemyLancerPrefab;
                    break;
                case "gunslinger":
                    prefab = LevelSetup.Instance.enemyGunslingerPrefab;
                    break;
                case "rogue":
                    prefab = LevelSetup.Instance.enemyRoguePrefab;
                    break;
                case "hero":
                    prefab = LevelSetup.Instance.enemyHeroPrefab;
                    break;
                default:
                    Debug.LogWarning($"Prefab not found for enemy class: {className}");
                    break;
            }
        }
        public override void AssignToList(GameObject unit)
        {
            LevelSetup.Instance.grid._enemyUnits.Add(unit);
            Debug.Log("Added to enemy list");
        }

    }
}

