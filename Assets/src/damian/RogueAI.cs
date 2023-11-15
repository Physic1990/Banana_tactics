using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RogueAI : AI
{
    static private bool run = false;
    public override int FindTargetUnit(List<GameObject> playerUnits, GameObject enemyUnit)
    {
        int _targetUnit = 0;
        int j = 0;
        float _min = Mathf.Infinity;
        float _current = 0f;

        if(enemyUnit.GetComponent<UnitAttributes>().GetHealth() < (enemyUnit.GetComponent<UnitAttributes>().GetMaxHealth()/3))
        {
            run = true;
        }
        else
        {
            run = false;
        }

        if (!run)
        {
            for (j = 0; j < playerUnits.Count; j++)
            {
                _current = playerUnits[j].GetComponent<UnitAttributes>().GetHealth();
                if (_min > _current)
                {
                    _targetUnit = j;
                    _min = _current;
                }
            }
        }
        else
        {
            for (j = 0; j < playerUnits.Count; j++)
            {
                // |x1-x2| + |y1-y2| distance formula
                _current = (Math.Abs(enemyUnit.transform.position.x - playerUnits[j].transform.position.x) + Math.Abs(enemyUnit.transform.position.y - playerUnits[j].transform.position.y));
                if (_min > _current)
                {
                    _targetUnit = j;
                    _min = _current;
                }
            }
        }
        return _targetUnit;
    }


    public override void MoveUnitToTarget(GameObject controlledUnit, GameObject targetUnit, Dictionary<Vector2, Tile> tiles) //function to move target to unit
    {

        float _targetX = targetUnit.transform.position.x;
        float _targetY = targetUnit.transform.position.y;
        Tile _controlledTile = tiles[new Vector2(controlledUnit.transform.position.x, controlledUnit.transform.position.y)];
        float _distanceFromTarget = (Math.Abs(controlledUnit.transform.position.x - targetUnit.transform.position.x) + Math.Abs(controlledUnit.transform.position.y - targetUnit.transform.position.y));
        int _height = FindHeightOfGrid(tiles); //height of grid
        int _width = FindWidthOfGrid(tiles); //width of grid

        //extra rogue code____________________________________________________________________________________________

        if (run) //we are running
        {
            _targetX = controlledUnit.transform.position.x - (targetUnit.transform.position.x - controlledUnit.transform.position.x);
            _targetY = controlledUnit.transform.position.y - (targetUnit.transform.position.y - controlledUnit.transform.position.y);
        }

        if (_targetX < 0) //makes sure we dont try to go out of bounds
        {
            _targetX = 0;
        }
        else if(_targetX > _width) {
            _targetX = _width;
        }
        if(_targetY < 0)
        {
            _targetY = 0;
        }
        else if (_targetY > _height)
        {
            _targetY = _height;
        }

        //------------------------------------------------------------------------------------------------------------------

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
        if (_bestPath.Count > 1 && (_targetX <= _width) && (_targetY <= _height) && (_targetX >= 0) && (_targetY >= 0))
        {
            _targetX = _bestPath[controlledUnit.GetComponent<UnitAttributes>().GetMovement() - 1].transform.position.x;
            _targetY = _bestPath[controlledUnit.GetComponent<UnitAttributes>().GetMovement() - 1].transform.position.y;




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
}
