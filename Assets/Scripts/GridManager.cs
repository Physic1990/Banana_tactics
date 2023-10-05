using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _cam;
    [SerializeField] private int _cursorDelay;

    AudioManager audioManager;
    private Dictionary<Vector2, Tile> _tiles;
    private List<GameObject> _playerUnits = new List<GameObject>();
    private List<GameObject> _enemyUnits = new List<GameObject>();
    private Tile _cursorTile;
    private Tile _selectedTile;
    private Tile _moveToTile;

    private int _cursorTimer;
    public bool _selectionMode = false;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        _cursorTimer = 0;
        GenerateGrid();
        _cursorTile = GetTileAtPosition(new Vector2(0, 0));
        _cursorTile.TurnOnHighlight();
    }

    private void Update()
    {
        CursorControl();
        
    }
    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for(int x = 0; x < _width; x++)
        {
            for(int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x,y,0), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
                if(x == 0 && y == 0)
                {
                    spawnedTile.SpawnUnit("Player", _playerUnits);
                }
                if(x == _width-1 && y == _height-1)
                {
                    spawnedTile.SpawnUnit("Enemy", _enemyUnits);
                }
                bool isOffset = ((x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0));
                spawnedTile.Init(isOffset);
                _tiles[new Vector2 (x,y)] = spawnedTile;
            }
        }

        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if(_tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }

    public GameObject GetPlayerUnit(Tile tile)
    {
        GameObject unit = tile._unit;
        return unit;
    }

    private Tile MoveCursor(Tile cursorTile)
    {
        float x = cursorTile.transform.position.x;
        float y = cursorTile.transform.position.y;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            x++;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            x--;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            y--;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            y++;
        }
        Tile temp = GetTileAtPosition(new Vector2(x, y));
        if ((x < 0 || y < 0 || x >= _width || y >= _height) && !audioManager.SFXisPlaying())
        {
            audioManager.PlaySFX(audioManager.error);
        }
        if (temp != null && temp != cursorTile)
        {
            cursorTile.TurnOffHighlight();
            temp.TurnOnHighlight();
            return temp;
        }
        else
        {
            return cursorTile;
        }
        
;    }

    void CursorControl()
    {
        _cursorTimer++;
        if (_cursorTimer > _cursorDelay)
        {
            Tile temp = _cursorTile;
            _cursorTile = MoveCursor(temp);
            if (_selectionMode && _selectedTile != null)
            {
                _selectedTile.HighlightNoCursor();
            }
            if (_cursorTile != temp)
            {
                audioManager.PlaySFX(audioManager.cursorMove);
                _cursorTimer = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!_selectionMode && _cursorTile._occupied)
            {
                //Checks that you're moving a player and they have not acted
                if (_cursorTile._unit.CompareTag("Player") && !CheckActed(_cursorTile._unit))
                {
                    _selectedTile = _cursorTile;
                    audioManager.PlaySFX(audioManager.select);
                    Debug.Log(_cursorTile._unit.name);
                    _selectionMode = true;
                }
            }
            else if (_selectionMode)
            {
                _moveToTile = _cursorTile;
                if (!_moveToTile._occupied)
                {
                    //Move the Player Unit
                    _moveToTile.AssignUnit(_selectedTile._unit);
                    audioManager.PlaySFX(audioManager.placed);
                    UpdateActed(_selectedTile._unit);
                    _selectedTile.RemoveUnit();
                    _selectionMode = false;
                    _selectedTile.TurnOffHighlight();
                }
                else
                {
                    //Do not move the Player
                    audioManager.PlaySFX(audioManager.error);
                }

            }


            Debug.Log(_playerUnits);
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            _selectedTile.TurnOffHighlight();
            _selectionMode = false;
            audioManager.PlaySFX(audioManager.back);
        }

    }

    public void UpdateActed(GameObject unit)
    {
        UnitAttributes unitA = unit.GetComponent<UnitAttributes>();

        // Check if the script component was found
        if (unitA != null)
        {
            unitA.SetActed(true); // Set to true or false as needed
        }
    }

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
