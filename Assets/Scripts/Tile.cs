using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _cursorHand;
    [SerializeField] public GameObject _unit;

    public bool _occupied = false;
    private SpriteRenderer _renderer;
    

    public bool isHighlighted = false;
    public bool isOccupied = false;
    private void Awake()
    {
        // Automatically find and assign the SpriteRenderer component
        _renderer = GetComponent<SpriteRenderer>();
    }
    public void Init(bool isOffset)
    {
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
        _occupied = true;
        _unit = passedUnit;
        _unit.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
    }

    public GameObject RemoveUnit()
    {
        _occupied = false;
        GameObject temp = _unit;
        _unit = null;
        return temp;
    }

    public void SpawnUnit(string tag)
    {
        _occupied = true;
        _unit = GameObject.FindWithTag(tag);
        _unit.SetActive(true);
        _unit.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
    }

    public void TurnOnHighlight()
    {
        _highlight.SetActive(true);
        _cursorHand.SetActive(true);
        isHighlighted = true;
    }

    public void HighlightNoCursor()
    {
        _highlight.SetActive(true);
        isHighlighted = true;
    }
    public void TurnOffHighlight()
    {
        _highlight?.SetActive(false);
        _cursorHand.SetActive(false);
        isHighlighted = false;
    }
}
