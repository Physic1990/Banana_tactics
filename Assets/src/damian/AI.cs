using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AI : MonoBehaviour
{
    //Singleton Section
    private static AI instance;
    private static readonly object lockObject = new object();


    public static AI Instance
    {
        get
        {
            lock (lockObject) //ensures that only one thread can enter the critical section of code at a time.
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AI>();

                    if (instance == null)
                    {
                        GameObject singletonAI = new GameObject(typeof(AI).Name);
                        instance = singletonAI.AddComponent<AI>();
                    }
                }
            }
            return instance;
        }
    }
    

    ActionEventManager Event;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as AI;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Event = GameObject.FindGameObjectWithTag("ActionEvent").GetComponent<ActionEventManager>();
    }
    
    private List<Tile> openList;
    private List<Tile> closedList;
    public bool enemyTurnEnded = false;

    public IEnumerator AITurn(List<GameObject> playerUnits, List<GameObject> enemyUnits, Dictionary<Vector2, Tile> tiles)
    {
        enemyTurnEnded = false;
        //Debug.Log("enemy is taking turn");
        if (playerUnits.Count > 0)
        {
            for (int i = 0; i < enemyUnits.Count; i++)
            {

                int _targetIndex = FindTargetUnit(playerUnits, enemyUnits[i]); //target spotted....GET HIM
                MoveUnitToTarget(enemyUnits[i], playerUnits[_targetIndex], tiles); //approach

                yield return new WaitForSeconds(0.3f);

                //Debug.Log(_targetIndex);
 
                attackPlayer(enemyUnits[i], playerUnits[_targetIndex]); //swing

                yield return new WaitForSeconds(0.3f);
            }
            //yield return new WaitForSeconds(1);
        }
        enemyTurnEnded = true;
    }

    private void attackPlayer(GameObject enemyUnit, GameObject playerUnit) //attack player he stole your bananas or something and now you're angry (-n-)
    {
        float _distanceBetweenUnits = Math.Abs(enemyUnit.transform.position.x - playerUnit.transform.position.x) + Math.Abs(enemyUnit.transform.position.y - playerUnit.transform.position.y);
        if(_distanceBetweenUnits <= 1)
        {
            Event.attackBattle(enemyUnit, playerUnit); //Throw hands
        }
    }
    
    //Finds the closest unit to the current controlled unit
    virtual public int FindTargetUnit(List<GameObject> playerUnits, GameObject enemyUnit) 
    {
        int j = 0;
        float _mindistance = Mathf.Infinity;
        float _distance = 0f;
        //Debug.Log(playerUnits.Count);
        int _closestUnit = 0;
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


    public void MoveUnitToTarget(GameObject controlledUnit, GameObject targetUnit, Dictionary<Vector2, Tile> tiles) //function to move target to unit
    {

        float _targetX = targetUnit.transform.position.x;
        float _targetY = targetUnit.transform.position.y;
        Tile _controlledTile = tiles[new Vector2(controlledUnit.transform.position.x, controlledUnit.transform.position.y)];
        float _distanceFromTarget = (Math.Abs(controlledUnit.transform.position.x - targetUnit.transform.position.x) + Math.Abs(controlledUnit.transform.position.y - targetUnit.transform.position.y));
        int _height = FindHeightOfGrid(tiles); //height of grid
        int _width = FindWidthOfGrid(tiles); //width of grid


        //Debug.Log(controlledUnit + "offset");
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

        if ((_targetX <= _width) && (_targetY <= _height) && (_targetX >= 0) && (_targetY >= 0) && (!tiles[new Vector2(_targetX, _targetY)]._occupied))
        {
            //Debug.Log("Finding path");
            _bestPath = FindPath(controlledUnit.transform.position.x, controlledUnit.transform.position.y, _targetX, _targetY, tiles);
            //Debug.Log("path found");
        }

        //Debug.Log("the best is is this long " + _bestPath.Count);
        if (_bestPath.Count > controlledUnit.GetComponent<UnitAttributes>().GetMovement() && (_targetX <= _width) && (_targetY <= _height) && (_targetX >= 0) && (_targetY >= 0))
        {
            _targetX = _bestPath[controlledUnit.GetComponent<UnitAttributes>().GetMovement()].transform.position.x;
            _targetY = _bestPath[controlledUnit.GetComponent<UnitAttributes>().GetMovement()].transform.position.y;



            controlledUnit.transform.position = new Vector3(_targetX, _targetY, 0);
            _controlledTile.RemoveUnit();
            _controlledTile = tiles[new Vector2(_targetX, _targetY)];
            _controlledTile.AssignUnit(controlledUnit);
        }
        else if (_bestPath.Count == 2 && (_targetX <= _width) && (_targetY <= _height) && (_targetX >= 0) && (_targetY >= 0))
        {
            _targetX = _bestPath[1].transform.position.x;
            _targetY = _bestPath[1].transform.position.y;



            controlledUnit.transform.position = new Vector3(_targetX, _targetY, 0);
            _controlledTile.RemoveUnit();
            _controlledTile = tiles[new Vector2(_targetX, _targetY)];
            _controlledTile.AssignUnit(controlledUnit);
        }
        else
        {
            //Event.doNothingTurn(controlledUnit);
        }
        //Debug.Log(_targetY);
        //Debug.Log(_targetX);
    }


    public List<Tile> FindPath(float startX, float startY, float endX, float endY, Dictionary<Vector2, Tile> tiles) //find best path with A*
    {
        Tile _startTile = tiles[new Vector2(startX, startY)];
        Tile _endTile = tiles[new Vector2(endX, endY)];
        

        openList = new List<Tile> { _startTile }; //tiles to search
        closedList = new List<Tile>(); //searched tiles

        int _height = FindHeightOfGrid(tiles); //height of grid
        int _width = FindWidthOfGrid(tiles); //width of grid
        
        //Debug.Log(_height);
        //Debug.Log(_width);

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
            //Debug.Log(openList.Count);
            Tile _currentTile = FindLowestFCostTile(openList);
            if (_currentTile == _endTile) //target spotted engaging
            {
                return CalculatePath(_endTile);//gimme that best path
            }

            openList.Remove(_currentTile);
            closedList.Add(_currentTile);

            foreach (Tile neighborTile in GetNeighbors(_currentTile, _height, _width, tiles)) // itterate through neighboring tiles to search further
            {
                //Debug.Log(neighborTile);
                
                if (closedList.Contains(neighborTile))
                {
                    //Debug.Log("closed list contains tile");
                    continue;
                }

                if (neighborTile._occupied)
                {
                    //Debug.Log(neighborTile + "isoccu");
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
        //Debug.Log("returning empty list of length" + _emptyList.Count);
        return _emptyList; // we are done
    }

    private List<Tile> GetNeighbors(Tile currentTile, int height, int width, Dictionary<Vector2, Tile> tiles) //return all neighboring tiles
    {
        List<Tile> neighborList = new List<Tile>();

        if(currentTile.transform.position.x - 1 >= 0)
        {
            //Debug.Log("Left");
            neighborList.Add(tiles[new Vector2(currentTile.transform.position.x - 1, currentTile.transform.position.y)]); //Left
            
            if (currentTile.transform.position.y - 1 >= 0)
            {
                //Debug.Log("Left down");
                neighborList.Add(tiles[new Vector2(currentTile.transform.position.x - 1, currentTile.transform.position.y - 1)]); //Left Down
            }
                
            if (currentTile.transform.position.y + 1 < height)
            {
                //Debug.Log("Left up");
                neighborList.Add(tiles[new Vector2(currentTile.transform.position.x - 1, currentTile.transform.position.y + 1)]); //Left Up
            }
                
        }
        if(currentTile.transform.position.x + 1 < width)
        {
            //Debug.Log("right");
            neighborList.Add(tiles[new Vector2(currentTile.transform.position.x + 1, currentTile.transform.position.y)]); //Right
            if (currentTile.transform.position.y - 1 >= 0)
            {
                //Debug.Log("right down");
                neighborList.Add(tiles[new Vector2(currentTile.transform.position.x + 1, currentTile.transform.position.y -1)]); //Right Down
            }
                
            if (currentTile.transform.position.y + 1 < height)
            {
                //Debug.Log("right up");
                neighborList.Add(tiles[new Vector2(currentTile.transform.position.x + 1, currentTile.transform.position.y + 1)]); //Right up
            }
                
        }
        if(currentTile.transform.position.y - 1 >= 0)
        {
            //Debug.Log("down");
            neighborList.Add(tiles[new Vector2(currentTile.transform.position.x, currentTile.transform.position.y - 1)]); //Down
        }
        if(currentTile.transform.position.y + 1 < height)
        {
            //Debug.Log("up");
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
        //Debug.Log(path.Count);
        return path;
    }

    private Tile FindLowestFCostTile(List<Tile> pathList) // find node with lowest f cost
    {
        Tile lowestFCostTile = pathList[0];
        //Debug.Log(pathList.Count);
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
        return(20 * Mathf.Min(_xDist, _yDist) + 10 * _remaining);

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
