using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class APathPrefab : MonoBehaviour
{
    
    private List<Tile> openList;
    private List<Tile> closedList;
    
    /* 
    //Ai turn function(outline the ai turns actions) Example
    public void AITurn(List<GameObject> playerUnits, List<GameObject> enemyUnits, Dictionary<Vector2, Tile> tiles)
    {

        if (playerUnits.Count > 0)
        {
            for (int i = 0; i < enemyUnits.Count; i++)
            {

                int _targetIndex = FindClosestUnit(playerUnits, enemyUnits[i]); //target spotted....GET HIM

                MoveUnitToTarget(enemyUnits[i], playerUnits[_targetIndex], tiles); //approach

                //attackPlayer(enemyUnits[i], playerUnits[_targetIndex]); //swing

                //Any other actions your units can take

            }
        }
    }
    
    private void attackPlayer(GameObject enemyUnit, GameObject playerUnit) //attack player he stole your bananas or something and now you're angry (-n-)
    {
        float _distanceBetweenUnits = Math.Abs(enemyUnit.transform.position.x - playerUnit.transform.position.x) + Math.Abs(enemyUnit.transform.position.y - playerUnit.transform.position.y);
        if(_distanceBetweenUnits <= 1)
        {
            //Your code to attack
        }
    }
    */

    //Finds the closest unit to the current controlled unit
    public int FindClosestUnit(List<GameObject> playerUnits, GameObject enemyUnit) 
    {
        int j = 0;
        float _mindistance = Mathf.Infinity;
        float _distance = 0f;
        int _closestUnit = -1;
        for (j = 0; j < playerUnits.Count; j++)
        {
            // |x1-x2| + |y1-y2| distance formula
            _distance = (Math.Abs(enemyUnit.transform.position.x - playerUnits[j].transform.position.x) + Math.Abs(enemyUnit.transform.position.y - playerUnits[j].transform.position.y)); 
            if (_mindistance > _distance)
            {
                _closestUnit = j;
                _mindistance = _distance;
            }
        }
        return _closestUnit;
    }


    void MoveUnitToTarget(GameObject controlledUnit, GameObject targetUnit, Dictionary<Vector2, Tile> tiles) //function to move target to unit
    {

        float _targetX = targetUnit.transform.position.x;
        float _targetY = targetUnit.transform.position.y;
        Tile _controlledTile = tiles[new Vector2(controlledUnit.transform.position.x, controlledUnit.transform.position.y)];
        float _distanceFromTarget = (Math.Abs(controlledUnit.transform.position.x - targetUnit.transform.position.x) + Math.Abs(controlledUnit.transform.position.y - targetUnit.transform.position.y));
        int _height = FindHeightOfGrid(tiles); //height of grid
        int _width = FindWidthOfGrid(tiles); //width of grid

        //offset the target tile to not delete player unit
        if (controlledUnit.transform.position.x > targetUnit.transform.position.x)
        {
            _targetX++;
        }
        else if (controlledUnit.transform.position.x < targetUnit.transform.position.x)
        {
            _targetX--;
        }
        else if (controlledUnit.transform.position.y > targetUnit.transform.position.y)
        {
            _targetY++;
        }
        else if (controlledUnit.transform.position.y < targetUnit.transform.position.y)
        {
            _targetY--;
        }

        List<Tile> _bestPath = new List<Tile>();

        //make sure the target tile is reachable
        if ((_targetX <= _width) && (_targetY <= _height) && (_targetX >= 0) && (_targetY >= 0) && (!tiles[new Vector2(_targetX, _targetY)]._occupied))
        {
            _bestPath = FindPath(controlledUnit.transform.position.x, controlledUnit.transform.position.y, _targetX, _targetY, tiles);
        }

        if (_bestPath.Count > 1 && (_targetX <= _width) && (_targetY <= _height) && (_targetX >= 0) && (_targetY >= 0))
        {
            _targetX = _bestPath[controlledUnit.GetComponent<UnitAttributes>().GetMovement() - 1].transform.position.x; //!!!!!edit to use your units movement values!!!!!!!!
            _targetY = _bestPath[controlledUnit.GetComponent<UnitAttributes>().GetMovement() - 1].transform.position.y; //!!!!!edit to use your units movement values!!!!!!!!



            controlledUnit.transform.position = new Vector3(_targetX, _targetY, 0);
            _controlledTile.RemoveUnit();
            _controlledTile = tiles[new Vector2(_targetX, _targetY)];
            _controlledTile.AssignUnit(controlledUnit);
        }
        else
        {
            //do Nothing
        }
    }


    private List<Tile> FindPath(float startX, float startY, float endX, float endY, Dictionary<Vector2, Tile> tiles) //find best path with A*
    {
        Tile _startTile = tiles[new Vector2(startX, startY)];
        Tile _endTile = tiles[new Vector2(endX, endY)];
        

        openList = new List<Tile> { _startTile }; //tiles to search
        closedList = new List<Tile>(); //searched tiles

        int _height = FindHeightOfGrid(tiles); //height of grid
        int _width = FindWidthOfGrid(tiles); //width of grid
        

        for (int x = 0; x < _width; x++) //init for pathfinding
        {
            for (int y = 0; y < _height; y++) {
                Tile _pathTile = tiles[new Vector2(x, y)];
                _pathTile.gCost = int.MaxValue;
                _pathTile.fCost = _pathTile.gCost + _pathTile.hCost;
                _pathTile.cameFromNode = null;
            }
        }

        //setting first node values
        _startTile.gCost = 0;
        _startTile.hCost = FindDistanceCost(_startTile, _endTile);
        _startTile.fCost = _startTile.gCost + _startTile.hCost;


        
        while (openList.Count > 0)
        {
            Tile _currentTile = FindLowestFCostTile(openList);
            if (_currentTile == _endTile) //target spotted engaging
            {
                return CalculatePath(_endTile);//gimme that best path
            }

            openList.Remove(_currentTile);
            closedList.Add(_currentTile);

            foreach (Tile neighborTile in GetNeighbors(_currentTile, _height, _width, tiles)) // itterate through neighboring tiles to search further
            {
                
                if (closedList.Contains(neighborTile))
                {
                    continue;
                }

                if (neighborTile._occupied)
                {
                    closedList.Add(neighborTile);
                    continue;
                    
                }

                float _tempGCost = _currentTile.gCost + FindDistanceCost(_currentTile, neighborTile);
                if (_tempGCost < neighborTile.gCost)
                {
                    // the real calculations
                    neighborTile.cameFromNode = _currentTile;
                    neighborTile.gCost = _tempGCost;
                    neighborTile.hCost = FindDistanceCost(neighborTile, _endTile);
                    neighborTile.fCost = (neighborTile.gCost + neighborTile.hCost);

                    if (!openList.Contains(neighborTile)) //we haven't visited this node yet
                    {
                        openList.Add(neighborTile);
                    }
                }
            }

        }

        
        List<Tile> _emptyList = new List<Tile>();
        return _emptyList; // we are done
    }

    private List<Tile> GetNeighbors(Tile currentTile, int height, int width, Dictionary<Vector2, Tile> tiles) //return all neighboring tiles
    {
        List<Tile> neighborList = new List<Tile>();

        if(currentTile.transform.position.x - 1 >= 0)
        {
            neighborList.Add(tiles[new Vector2(currentTile.transform.position.x - 1, currentTile.transform.position.y)]); //Left
            
            if (currentTile.transform.position.y - 1 >= 0)
            {
                neighborList.Add(tiles[new Vector2(currentTile.transform.position.x - 1, currentTile.transform.position.y - 1)]); //Left Down
            }
                
            if (currentTile.transform.position.y + 1 < height)
            {
                neighborList.Add(tiles[new Vector2(currentTile.transform.position.x - 1, currentTile.transform.position.y + 1)]); //Left Up
            }
                
        }
        if(currentTile.transform.position.x + 1 < width)
        {
            neighborList.Add(tiles[new Vector2(currentTile.transform.position.x + 1, currentTile.transform.position.y)]); //Right
            if (currentTile.transform.position.y - 1 >= 0)
            {
                neighborList.Add(tiles[new Vector2(currentTile.transform.position.x + 1, currentTile.transform.position.y -1)]); //Right Down
            }
                
            if (currentTile.transform.position.y + 1 < height)
            {
                neighborList.Add(tiles[new Vector2(currentTile.transform.position.x + 1, currentTile.transform.position.y + 1)]); //Right up
            }
                
        }
        if(currentTile.transform.position.y - 1 >= 0)
        {
            neighborList.Add(tiles[new Vector2(currentTile.transform.position.x, currentTile.transform.position.y - 1)]); //Down
        }
        if(currentTile.transform.position.y + 1 < height)
        {
            neighborList.Add(tiles[new Vector2(currentTile.transform.position.x, currentTile.transform.position.y + 1)]); //Up
        }
        return neighborList;
    }

    private List<Tile> CalculatePath(Tile endTile) //return best path
    {
        List<Tile> path = new List<Tile>();
        path.Add(endTile);
        Tile _currentTile = endTile;
        while (_currentTile.cameFromNode != null) 
        {
            path.Add(_currentTile.cameFromNode);
            _currentTile = _currentTile.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private Tile FindLowestFCostTile(List<Tile> pathList) // find node with lowest f cost
    {
        Tile lowestFCostTile = pathList[0];
        for (int i = 1; i < pathList.Count; i++)
        {
            if (pathList[i].fCost < lowestFCostTile.fCost)
            {
                lowestFCostTile = pathList[i];
            }
        }
        return lowestFCostTile;
    }

    private float FindDistanceCost(Tile a, Tile b) //return distance calc for a* to use
    {
        float _xDist = Mathf.Abs(a.transform.position.x - b.transform.position.x);
        float _yDist = Mathf.Abs(a.transform.position.y - b.transform.position.y);
        float _remaining = MathF.Abs(_xDist - _yDist);
        return(14 * Mathf.Min(_xDist, _yDist) + 10 * _remaining);

    }


    private int FindHeightOfGrid(Dictionary<Vector2, Tile> tiles) //find height of grid
    {
        int _height = 0;
        while(tiles.ContainsKey(new Vector2(0,_height))) 
        { 
            _height++;
        }

        return _height;
    }

    private int FindWidthOfGrid(Dictionary<Vector2, Tile> tiles) //find width of grid
    {
        int _width = 0;
        while (tiles.ContainsKey(new Vector2(_width, 0)))
        {
            _width++;
        }

        return _width;
    }



}
