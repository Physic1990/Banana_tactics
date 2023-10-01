using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _cursorHand;
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

    public void TurnOnHighlight()
    {
        _highlight.SetActive(true);
        _cursorHand.SetActive(true);
        isHighlighted = true;
    }

    public void TurnOffHighlight()
    {
        _highlight?.SetActive(false);
        _cursorHand.SetActive(false);
        isHighlighted = false;
    }
}
