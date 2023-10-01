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

    private Dictionary<Vector2, Tile> _tiles;
    private Tile _cursorTile;

    private int _cursorTimer;
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
            if(_cursorTile != temp)
            {
                _cursorTimer = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(_cursorTile.name);
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

    public Tile GetHighlightedTile()
    {
        foreach (var member in _tiles)
        {
            Tile tile = member.Value;
            if (tile.isHighlighted)
            {
                return tile;
            }
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
        if(temp != null && temp != cursorTile)
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
