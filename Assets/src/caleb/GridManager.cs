using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEditor.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;
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
    UnitMenus unitMenus;
    PauseMenu pauseMenu;

    /*************************************************************************
                             Dictionarys & Lists
    ************************************************************************/

    //Stores all the generated tiles
    private Dictionary<Vector2, Tile> _tiles;
    //List of all "player" Game Objects
    public List<GameObject> _playerUnits = new List<GameObject>();
    //List of all "enemy" Game Objects
    public List<GameObject> _enemyUnits = new List<GameObject>();

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
                             Player Movement
    ************************************************************************/
    // BEN ADDED THIS
    private PlayerControls input = null;

    // Tracks the player's movement inputs as a Vector2
    private Vector2 movementInput = Vector2.zero;

    /*************************************************************************
                         Initilize and Update Functions
    ************************************************************************/
    private void Awake()
    {
        input = new PlayerControls();
    }

    private void OnEnable()
    {
        input.Enable();
        // Handles Player Movement
        input.Player.Move.performed += OnMovementPerformed;
        input.Player.Move.canceled += OnMovementCancelled;

        // Handles Unit Selection
        input.Player.SelectUnit.performed += ctx => SelectUnit();
        input.Player.UnselectUnit.performed += ctx => UnselectUnit();
    }

    private void OnDisable()
    {
        input.Disable();
        // Clears Player Movement actions
        input.Player.Move.performed -= OnMovementPerformed;
        input.Player.Move.canceled -= OnMovementCancelled;

        // Clears Unit Selection actions
        input.Player.SelectUnit.performed -= ctx => SelectUnit();
        input.Player.UnselectUnit.performed -= ctx => UnselectUnit();
    }

    //Start function called as soon as Grid Manager is initilized
    private void Start()
    {
        //First Grid Manager finds all partner scripts it will be calling functions from
        aiManager = GameObject.FindGameObjectWithTag("Ai").GetComponent<AI>();
        actionEvent = GameObject.FindGameObjectWithTag("ActionEvent").GetComponent<ActionEventManager>();
        cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<CursorController>();
        unitMenus = GameObject.FindGameObjectWithTag("GameUIDocument").GetComponent<UnitMenus>();
        pauseMenu = GameObject.FindGameObjectWithTag("GameUIDocument").GetComponent<PauseMenu>();

        //Varaibles are initilized to their default values
        playerTurnOver = false;
        _cursorTimer = 0;
        _Delay = 400;
        if (_width < 5)
        {
            _width = 5;
        }
        if (_height < 5)
        {
            _height = 5;
        }
        //The grid is created
        GenerateGrid();
        LevelSetup.Instance.GenerateUnits();
        //The default position of the cursor is always set to the bottom left
        _cursorTile = GetTileAtPosition(new Vector2(0, 0));
        _cursorTile.TurnOnHighlight();

        // BEN ADDED THIS
        // Handles the Unit Menu for the Unit (If any) on the cursor's tile
        unitMenus.HandleUnitByTile(_cursorTile);
    }

    //Update is called every frame
    private void Update()
    {
        bool isPrevEnemyTurn = playerTurnOver;
        //Function that removes dead units from the grid
        RemoveDeadUnits();
        //checks if player turn has ended
        playerTurnOver = CheckPlayerTurn();
        // BEN ADDED THIS
        // If the previous execution was for the enemies' turn but this execution is for the player's turn.
        // This should only execute when the player's turn first begins.
        if (isPrevEnemyTurn && !playerTurnOver)
        {
            // Update the Unit Menus
            HandleUnitMenusByTile(_cursorTile);
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

    // BEN ADDED THIS
    // Returns a List of all the adjacent Enemy Unit game objects around the passed tile
    private List<GameObject> GetAdjacentEnemeyUnits(Tile tile)
    {
        // Gets all adjacent objects around the passed tile
        List<GameObject> adjObjects = GetAdjancentObjects(tile);

        // The list of Enemy Unit game objects that will be returned later
        List<GameObject> enemies = new List<GameObject>();

        // For each adjacent object
        foreach (GameObject adjObject in adjObjects)
        {
            // If the adjacent object isn't an Enemy Unit, continue
            if (!adjObject.CompareTag("Enemy")) continue;

            // Add the Enemy Unit game object to the enemies List
            enemies.Add(adjObject);
        }

        return enemies;
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
    // BEN ADDED THIS
    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        // Reads the vector2 value and assigns it to movementInput
        movementInput = value.ReadValue<Vector2>();
    }

    // BEN ADDED THIS
    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        // Clears movementInput
        movementInput = Vector2.zero;
    }

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
                // If not in selection mode, then any updates to the cursor's position should update the Unit Menus for the new tile
                if (!_selectionMode)
                {
                    HandleUnitMenusByTile(_cursorTile);
                }

                AudioManager.Instance.PlaySFX(AudioManager.Instance.cursorMove);
                _cursorTimer = 0;
            }
        }
    }

    //Checks if the player has inputted anything and updates the x and y coordinates of the _cursorTile accordingly
    private Tile TakePlayerInput()
    {
        // BEN ADDED THIS
        // This is needed because the Input System still registers inputs when the Timescale is 0
        if (pauseMenu.isGamePaused) return _cursorTile;

        //New x y coordinates of the cursor
        float x = _cursorTile.transform.position.x;
        float y = _cursorTile.transform.position.y;

        // BEN ADDED THIS
        //The right arrow key increments the x position of the cursor
        if (movementInput.x == 1.0f)
        {
            x++;
        }
        //The left arrow key decrements the x position of the cursor
        else if (movementInput.x == -1.0f)
        {
            x--;
        }

        //The up arrow key increments the y position of the cursor
        if (movementInput.y == 1.0f)
        {
            y++;
        }
        //The down arrow key decrements the y position of the cursor
        else if (movementInput.y == -1.0f)
        {
            y--;
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

    // BEN MADE THIS A SEPERATE FUNCTION
    // Handles Unit Selection

    private void SelectUnit()
    {
        // This is needed because the Input System still registers inputs when the Timescale is 0
        if (pauseMenu.isGamePaused) return;

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
                _moveToTile._unit.GetComponent<UnitAttributes>().SetActed(true);
                AudioManager.Instance.PlaySFX(AudioManager.Instance.placed);

                // Handles the Unit Menus in response to the unit being placed on a new tile
                HandleUnitMenusByTile(_moveToTile);
            }
            //if the tile is already occupied
            else
            {
                //Do not move the Player
                AudioManager.Instance.PlaySFX(AudioManager.Instance.error);
            }
        }
    }

    // BEN MADE THIS A SEPERATE FUNCTION
    // Unselects the selected Unit.
    private void UnselectUnit()
    {
        // This is needed because the Input System still registers inputs when the Timescale is 0
        if (pauseMenu.isGamePaused) return;

        _selectedTile.TurnOffHighlight();
        _selectionMode = false;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.back);
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
            if (obj != null)
            {
                if (!CheckActed(obj))
                {
                    return false;
                }
            }
            else
            {
                _playerUnits.Remove(obj);
            }
        }
        return true;
    }

    //Currently a debug implementation for Enemy Ai. Called when enemy turn starts
    private void EnemyTurn()
    {
        if (_Delay == 400)
        {
            aiManager.AITurn(_playerUnits, _enemyUnits, _tiles);
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



    //Checks to see if all Player units have acted or not
    public void RemoveDeadUnits()
    {
        foreach (var kvp in _tiles)
        {
            Tile tile = kvp.Value;
            if (tile._occupied)
            {
                if(tile._unit.GetComponent<UnitAttributes>().GetHealth() <= 0)
                {
                    Debug.Log("Removing Dead Unit");
                    if (tile._unit.tag == "Player")
                    {
                        _playerUnits.Remove(tile._unit);
                        Debug.Log("Player Removed");
                    }
                    else if (tile._unit.tag == "Enemy")
                    {
                        _enemyUnits.Remove(tile._unit);
                        Debug.Log("Enemy Removed");
                    }
                    GameObject temp = tile._unit;
                    tile.RemoveUnit();
                    temp.GetComponent<UnitAttributes>().DestroyUnit();
                }
            }
        }
    }

    /*************************************************************************
                             UI HELPER FUNCTIONS
    ************************************************************************/
    // BEN ADDED THIS
    // Handles updating both the Player and Enemy Unit Menus based on the passed tile.
    private void HandleUnitMenusByTile(Tile tile)
    {
        // Handles the Unit Menu for the Unit (If any) on the passed tile
        unitMenus.HandleUnitByTile(tile);

        // BEN ADDED THIS
        // If the passed tile has a Player Unit on it
        if (tile._occupied && tile._unit.CompareTag("Player"))
        {
            // Gets the adjancent enemy unit game objects around the Player Unit's tile (If any)
            List<GameObject> adjEnemyUnits = GetAdjacentEnemeyUnits(tile);
            // If there are any enemy units adjacent to the Player Unit
            if (adjEnemyUnits.Count > 0)
            {
                // Updates the enemy Unit Menu to be the Unit for the first adjancent enemy unit
                unitMenus.HandleUnitByGameObject(adjEnemyUnits[0]);
            }
            else
            {
                // Hide the enemy unit menu
                unitMenus.SetEnemyUnitMenuVisibility(false);
            }
        }
        // If the passed tile has an Enemy Unit on it
        else if (tile._occupied && tile._unit.CompareTag("Enemy"))
        {
            // Hide the Player Unit menu
            unitMenus.SetPlayerUnitMenuVisibility(false);

        }
    }
}

