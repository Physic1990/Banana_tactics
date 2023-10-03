using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _cam;
    [SerializeField] private int _cursorDelay;

    AudioManager audioManager;
    private Dictionary<Vector2, Tile> _tiles;
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
        _cursorTimer++;
        if(_cursorTimer > _cursorDelay)
        {
            Tile temp = _cursorTile;
            _cursorTile = MoveCursor(temp);
            if (_selectionMode && _selectedTile != null)
            {
                _selectedTile.HighlightNoCursor();
            }
            if(_cursorTile != temp)
            {
                audioManager.PlaySFX(audioManager.cursorMove);
                _cursorTimer = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!_selectionMode && _cursorTile._occupied)
            {
                if (_cursorTile._unit.CompareTag("Player"))
                {
                    _selectedTile = _cursorTile;
                    audioManager.PlaySFX(audioManager.select);
                    _selectionMode = true;
                }
            }
            else if(_selectionMode)
            {
                _moveToTile = _cursorTile;
                if (!_moveToTile._occupied)
                {
                    _moveToTile.AssignUnit(_selectedTile._unit);
                    audioManager.PlaySFX(audioManager.placed);
                    _selectedTile.RemoveUnit();
                    _selectionMode = false;
                    _selectedTile.TurnOffHighlight();
                }
                else
                {
                    audioManager.PlaySFX(audioManager.error);
                }
                
            }


            Debug.Log(_cursorTile.name);
        }
        if(Input.GetKeyUp(KeyCode.V)) 
        {
            _selectedTile.TurnOffHighlight();
            _selectionMode = false;
            audioManager.PlaySFX(audioManager.back);
        }

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
                    spawnedTile.SpawnUnit("Player");
                }
                if(x == _width-1 && y == _height-1)
                {
                    spawnedTile.SpawnUnit("Enemy");
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
}
