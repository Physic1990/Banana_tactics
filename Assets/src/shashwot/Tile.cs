using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;  // Serialized fields for base and offset colors
    [SerializeField] private GameObject _highlight;  // Serialized field for a highlight GameObject
    [SerializeField] public GameObject _unit;  // Serialized field for a game unit GameObject
    
    //Damien's Variables for Pathfinding
    public float gCost = 0;
    public float hCost = 0;
    public float fCost = 0;
    public Tile cameFromNode;

    public bool _occupied = false;  // Flag indicating if the tile is occupied by a unit
    private SpriteRenderer _renderer;  // Reference to the SpriteRenderer component

    public bool isHighlighted = false;  // Flag indicating if the tile is currently highlighted
    public bool isOccupied = false;  // Flag indicating if the tile is occupied (possibly redundant)

    private void Awake()
    {
        // Automatically find and assign the SpriteRenderer component
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void Init(bool isOffset)
    {
        // Initialize the tile's color based on whether it's an offset tile or not
        if (isOffset)
        {
            _renderer.color = _offsetColor;
        }
        else
        {
            _renderer.color = _baseColor;
        }
    }

    public void AssignUnit(GameObject passedUnit)
    {
        // Assign a game unit to the tile, mark it as occupied, and position it at the tile's position
        _occupied = true;
        _unit = passedUnit;
        _unit.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
    }

    public GameObject RemoveUnit()
    {
        // Remove the game unit from the tile, mark it as unoccupied, and return the removed unit
        _occupied = false;
        GameObject temp = _unit;
        _unit = null;
        return temp;
    }

    public void SpawnUnit(string tag, List<GameObject> unitLog)
    {
        // Spawn a game unit with the specified tag on the tile, mark it as occupied, and position it at the tile's position
        _occupied = true;
        _unit = GameObject.FindWithTag(tag);
        _unit.SetActive(true);
        _unit.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
        
        if (tag == "Player")
        {
            unitLog.Add(_unit);  // If the unit belongs to the player, add it to the unit log (assuming it's a list).
        }
    }

    public void TurnOnHighlight()
    {
        // Turn on the tile's highlight
        _highlight.SetActive(true);
        isHighlighted = true;
    }

    public void HighlightNoCursor()
    {
        // Similar to TurnOnHighlight, but doesn't mention a cursor (possibly a minor variation).
        _highlight.SetActive(true);
        isHighlighted = true;
    }

    public void TurnOffHighlight()
    {
        // Turn off the tile's highlight if it exists, and mark it as not highlighted
        _highlight?.SetActive(false);  // The ?. operator ensures null safety
        isHighlighted = false;
    }
}
