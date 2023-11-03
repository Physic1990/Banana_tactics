using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEditor.Profiling;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GridManager : MonoBehaviour
{
    /*************************************************************************
                             Serialized Variables
    ************************************************************************/

    //Features which are initiated one a scene by scene basis
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _cam;
    //The minimum number of frames that need to pass before a player's next input is read
    [SerializeField] private int _cursorDelay;

    /*************************************************************************
                              Imported Scripts
    ************************************************************************/

    AI aiManager;
    ActionEventManager actionEvent;
    CursorController cursor;
    UnitMenus gameUI;

    /*************************************************************************
                             Dictionarys & Lists
    ************************************************************************/

    //Stores all the generated tiles
    private Dictionary<Vector2, Tile> _tiles;
    //List of all "player" Game Objects
    private List<GameObject> _playerUnits = new List<GameObject>();
    //List of all "enemy" Game Objects
    private List<GameObject> _enemyUnits = new List<GameObject>();

    /*************************************************************************
                                Tile Variables
    ************************************************************************/

    //Publically referencial cursor tile. Tracks which tile the cursor object is currently hovering over 
    public Tile _cursorTile;
    //Private select tile. Tracks the tile the cursor has selected. Only active while selection mode is active
    private Tile _selectedTile;
    //Private second selection tile. When in selection mode, this tracks which tile the cursor next selects.
    private Tile _moveToTile;

    /*************************************************************************
                            Timer/Delay Variables
    ************************************************************************/

    // Tracks how long it's been since the last player input
    private int _cursorTimer;
    //Temporary variable meant to test enemy phase
    private int _Delay;

    /*************************************************************************
                         Status Tracking Variables
    ************************************************************************/

    //Tracks if an object has been selected by the cursor
    public bool _selectionMode = false;
    //Tracks whose turn it is between the player and the CPU
    private bool playerTurnOver;

    /*************************************************************************
                         Initilize and Update Functions
    ************************************************************************/

    //Start function called as soon as Grid Manager is initilized
    private void Start()
    {
        //First Grid Manager finds all partner scripts it will be calling functions from
        aiManager = GameObject.FindGameObjectWithTag("Ai").GetComponent<AI>();
        actionEvent = GameObject.FindGameObjectWithTag("ActionEvent").GetComponent<ActionEventManager>();
        cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<CursorController>();
        gameUI = GameObject.FindGameObjectWithTag("GameUIDocument").GetComponent<UnitMenus>();

        //Varaibles are initilized to their default values
        playerTurnOver = false;
        _cursorTimer = 0;
        _Delay = 400;

        //The grid is created
        GenerateGrid();

        //The default position of the cursor is always set to the bottom left
        _cursorTile = GetTileAtPosition(new Vector2(0, 0));
        _cursorTile.TurnOnHighlight();

        // BEN ADDED THIS
        // Handles the Unit Menu for the Unit (If any) on the cursor's tile
        gameUI.HandleUnitByTile(_cursorTile);
    }

    //Update is called every frame
    private void Update()
    {
        // BEN ADDED THIS
        // Stores the turn status of the previous Update execution
        bool isPrevEnemyTurn = playerTurnOver;

        //checks if player turn has ended
        playerTurnOver = CheckPlayerTurn();

        // BEN ADDED THIS
        // If the previous execution was for the enemies' turn but this execution is for the player's turn.
        // This should only execute when the player's turn first begins.
        if (isPrevEnemyTurn && !playerTurnOver)
        {
            // Gets the adjancent game objects around the cursor tile
            List<GameObject> adjUnits = GetAdjancentObjects(_cursorTile);
            if (adjUnits.Count > 0)
            {
                // Updates the enemy Unit Menu to be the Unit for the first adjancent game object
                gameUI.HandleUnitByGameObject(adjUnits[0]);
            }
            else
            {
                gameUI.SetEnemyUnitMenuVisibility(false);
            }
        }

        if (playerTurnOver)
        {
            //if player turn has ended, call the enemy turn function
            EnemyTurn();
        }
        else
        {
            //check for player input
            CursorControl();
            //move cursor according to player input
            cursor.MoveCursor(new Vector2(_cursorTile.transform.position.x, _cursorTile.transform.position.y));


        }
    }

    //Generates the grid based on the width, height and tile prefab
    void GenerateGrid()
    {
        //creates the tile dictionary, which will store all generated tiles
        _tiles = new Dictionary<Vector2, Tile>();
        //double for loop to create a grid
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Tile spawnedTile = CreateTile(new Vector2(x, y));
                //Placeholder code to place two units. This will be replaced in the future with a call to LevelManager
                if (x == 0 && y == 0)
                {
                    //Finds a game object of tag "player" and places it at the 0,0 tile
                    spawnedTile.SpawnUnit("Player", _playerUnits);
                }
                if (x == _width - 1 && y == _height - 1)
                {
                    //Finds a game object of tag "enemy" and places it at the top right tile
                    spawnedTile.SpawnUnit("Enemy", _enemyUnits);
                    _enemyUnits.Add(GameObject.FindGameObjectWithTag("Enemy"));
                }
                //This code handles the checkered pattern of the grid but coloring offset tiles a darker green
                bool isOffset = ((x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0));
                spawnedTile.Init(isOffset);
                //adds the newly created tile to the dictionary of tiles
                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
        //focus the camera on the center of the grid
        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }

    //PATTERN: Builder
    public Tile CreateTile(Vector2 position)
    {
        var spawnedTile = Instantiate(_tilePrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        spawnedTile.name = $"Tile {position.x} {position.y}";
        return spawnedTile;
    }

    /*************************************************************************
             "Get At" Functions (pretty much just return statemenets)
    ************************************************************************/

    //Takes an Vector2 containing the coordinates of the tile you want and returns that tile object to you
    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }

    //Returns the object currently assigned to a given tile
    public GameObject GetPlayerUnit(Tile tile)
    {
        GameObject unit = tile._unit;
        return unit;
    }

    //Gets all the adjacent game objects from adjancent tiles
    public List<GameObject> GetAdjancentObjects(Tile tile)
    {
        float x = tile.transform.position.x;
        float y = tile.transform.position.y;
        List<GameObject> Units = new List<GameObject>();
        Tile temp = GetTileAtPosition(new Vector2(x, y + 1));
        BuildAdjList(Units, temp);
        temp = GetTileAtPosition(new Vector2(x + 1, y));
        BuildAdjList(Units, temp);
        temp = GetTileAtPosition(new Vector2(x, y - 1));
        BuildAdjList(Units, temp);
        temp = GetTileAtPosition(new Vector2(x - 1, y));
        BuildAdjList(Units, temp);
        return Units;
    }

    //Used to build the list of adjacent units
    public void BuildAdjList(List<GameObject> Units, Tile tile)
    {
        if (tile != null)
        {
            if (tile._occupied)
            {
                Units.Add(tile._unit);
            }
        }
    }

    //These two functions allow for other scripts to access the private _height and _width variables of grid manager
    public int Height
    {
        get { return _height; }
    }

    public int Width
    {
        get { return _width; }
    }

    /*************************************************************************
                       Cursor and Player Input Functions
    ************************************************************************/

    //This function is the lifeblood of the cursor and handles all aspects and function calls related to the cursor (both moving and selection)
    private void CursorControl()
    {
        //increments the time which will allow the cursor to act
        _cursorTimer++;
        if (_cursorTimer > _cursorDelay)
        {
            Tile temp = _cursorTile;
            //Updates the cursorTile's position according to the player's inputs
            _cursorTile = TakePlayerInput();
            //Highlight the selectedTile if selection mode is currently active
            if (_selectionMode && _selectedTile != null)
            {
                _selectedTile.HighlightNoCursor();
            }
            //Checks if the cursor actually changed position
            if (_cursorTile != temp)
            {
                // BEN ADDED THIS
                // If not in selection mode, then any updates to the cursor's position should update the Unit Menu(s) for the new tile
                if (!_selectionMode)
                {
                    // Handles the Unit Menu for the Unit (If any) on the cursor's tile
                    gameUI.HandleUnitByTile(_cursorTile);
                }

                AudioManager.Instance.PlaySFX(AudioManager.Instance.cursorMove);
                _cursorTimer = 0;
            }
        }
        //Calls selection control which handles the cursor selecting a game object, confirming selections and backing out of selections
        SelectionControl();
    }

    //Checks if the player has inputted anything and updates the x and y coordinates of the _cursorTile accordingly
    private Tile TakePlayerInput()
    {
        //New x y coordinates of the cursor
        float x = _cursorTile.transform.position.x;
        float y = _cursorTile.transform.position.y;
        //The right arrow key increments the x position of the cursor
        if (Input.GetKey(KeyCode.RightArrow))
        {
            x++;
        }
        //The left arrow key decrements the x position of the cursor
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            x--;
        }
        //The down arrow key decrements the y position of the cursor
        if (Input.GetKey(KeyCode.DownArrow))
        {
            y--;
        }
        //The up arrow key increments the y position of the cursor
        if (Input.GetKey(KeyCode.UpArrow))
        {
            y++;
        }
        //Calls MoveCursor to actually move the cursor to these new coordinates
        return MoveCursor(new Vector2(x, y));
    }

    //Takes a coordinate Vector2, and checks to see if it is inbounds for the cursor to move there. Returns updated cursor tile
    public Tile MoveCursor(Vector2 pos)
    {
        //fetches the tile at the position we want to go to
        Tile temp = GetTileAtPosition(pos);
        //Checks if x and y coordinates are out of bounds. If yes, plays an error sound
        if ((pos.x < 0 || pos.y < 0 || pos.x >= _width || pos.y >= _height) && !AudioManager.Instance.SFXisPlaying())
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.error);
        }
        //if the cursor would be moving to a valid tile, highlight this new tile,
        //remove the highlight from the old tile, and return this new tile as the cursorTile
        if (temp != null && temp != _cursorTile)
        {
            _cursorTile.TurnOffHighlight();
            temp.TurnOnHighlight();
            return temp;
        }
        //Otherwise the cursor didn't move
        else
        {
            return _cursorTile;
        }
    }

    //Controls taking player inputs to select an object where the cursor is and back out of selections
    private void SelectionControl()
    {
        // The F key is used to select the thing where the cursor is at
        if (Input.GetKeyDown(KeyCode.F))
        {
            //If something hasn't been selected yet, and there is an object occupying the space the cursor is at, we mark that object as selected
            if (!_selectionMode && _cursorTile._occupied)
            {
                //Checks that you're moving a player and they have not acted (so, no moving enemies or players that have already acted)
                if (_cursorTile._unit.CompareTag("Player") && !CheckActed(_cursorTile._unit))
                {
                    _selectedTile = _cursorTile;
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.select);
                    _selectionMode = true;
                }
            }
            //If the player press F while an object is marked as selected, this code will try to move the object to the current cursorTile space
            else if (_selectionMode)
            {
                _moveToTile = _cursorTile;
                //checks that there's not already an object on the tile
                if (!_moveToTile._occupied && ValidMove())
                {
                    //Assigns player unit to new tile and removes them from the old tile. Dehighlight all non-cursor tiles
                    _moveToTile.AssignUnit(_selectedTile._unit);
                    _selectedTile.RemoveUnit();
                    _selectionMode = false;
                    _selectedTile.TurnOffHighlight();
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.placed);

                    // BEN ADDED THIS
                    // Gets the adjancent game objects around the moved to tile
                    List<GameObject> adjUnits = GetAdjancentObjects(_moveToTile);
                    if (adjUnits.Count > 0)
                    {
                        // Updates the enemy Unit Menu to be the Unit for the first adjancent game object
                        gameUI.HandleUnitByGameObject(adjUnits[0]);
                    }
                    else
                    {
                        gameUI.SetEnemyUnitMenuVisibility(false);
                    }

                }
                //if the tile is already occupied
                else
                {
                    //Do not move the Player
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.error);
                }

            }
        }
        // By pressing the V key, the player can unmark a selected unit (back out of a selection)
        if (Input.GetKeyUp(KeyCode.V))
        {
            _selectedTile.TurnOffHighlight();
            _selectionMode = false;
            AudioManager.Instance.PlaySFX(AudioManager.Instance.back);
        }
    }

    public bool ValidMove()
    {
        float fromX = _selectedTile.transform.position.x;
        float fromY = _selectedTile.transform.position.y;
        float toX = _moveToTile.transform.position.x;
        float toY = _moveToTile.transform.position.y;
        float distance = (Math.Abs(fromX - toX) + Math.Abs(fromY - toY));
        //Gets the movement state of the selected unit
        float movement = _selectedTile._unit.GetComponent<UnitAttributes>().GetMovement();
        if (distance <= movement)
        {
            return true;
        }
        return false;
    }

    /*************************************************************************
                         Turn and Acted Control Functions
    ************************************************************************/

    //Checks to see if all Player units have acted or not
    public bool CheckPlayerTurn()
    {
        foreach (GameObject obj in _playerUnits)
        {
            if (!CheckActed(obj))
            {
                return false;
            }
        }
        return true;
    }

    //Currently a debug implementation for Enemy Ai. Called when enemy turn starts
    private void EnemyTurn()
    {
        if (_Delay == 400)
        {
            aiManager.AITurn(_playerUnits, _enemyUnits);
        }
        _Delay--;
        //Once enemy turn has ended, start player turn again
        if (_Delay < 0)
        {
            _Delay = 400;
            ReactivatePlayerUnits();
        }
    }

    //Reactives all player units for the start of player turn
    public void ReactivatePlayerUnits()
    {
        foreach (GameObject obj in _playerUnits)
        {
            UpdateActed(obj, false);
        }
    }

    //Updates if a unit has acted
    public void UpdateActed(GameObject unit, bool acted)
    {
        UnitAttributes unitA = unit.GetComponent<UnitAttributes>();
        // Check if the script component was found
        if (unitA != null)
        {
            unitA.SetActed(acted); // Set to true or false for if they've acted or not
        }
    }

    //Checks if a given unit has already acted or not
    public bool CheckActed(GameObject unit)
    {
        UnitAttributes unitA = unit.GetComponent<UnitAttributes>();
        if (unitA != null)
        {
            return unitA.HasActed(); // Set to true or false as needed
        }
        return true;
    }
}

