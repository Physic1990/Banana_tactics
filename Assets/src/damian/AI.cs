using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AI : MonoBehaviour
{
    private List<Tile> openList;
    private List<Tile> closedList;

    public void AITurn(List<GameObject> playerUnits, List<GameObject> enemyUnits, Dictionary<Vector2, Tile> tiles)
    {

        //Debug.Log(enemyUnits.Count);

        for (int i = 0; i < enemyUnits.Count; i++)
        {
            //find the target to move toward or attack
            int _targetIndex = FindClosestUnit(playerUnits, enemyUnits[i]);

            //move
            //Debug.Log(_targetIndex);
            MoveUnitToTarget(enemyUnits[i], playerUnits[_targetIndex], tiles);


            //attack or heal

        }

    }
    
    
    //Finds the closest unit to the current controlled unit
    public int FindClosestUnit(List<GameObject> playerUnits, GameObject enemyUnit) 
    {
        int j = 0;
        float _mindistance = Mathf.Infinity;
        float _distance = 0f;
        //Debug.Log(playerUnits.Count);
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


    void MoveUnitToTarget(GameObject controlledUnit, GameObject targetUnit, Dictionary<Vector2, Tile> tiles)
    {

        float _targetX = controlledUnit.transform.position.x; 
        float _targetY = controlledUnit.transform.position.y;
        Tile _controlledTile = tiles[new Vector2(controlledUnit.transform.position.x, controlledUnit.transform.position.y)];
        float _distanceFromTarget = (Math.Abs(controlledUnit.transform.position.x - targetUnit.transform.position.x) + Math.Abs(controlledUnit.transform.position.y - targetUnit.transform.position.y));

        Debug.Log(_distanceFromTarget);
        if (_distanceFromTarget <= controlledUnit.GetComponent<UnitAttributes>().GetMovement() + 1) { //if target unit is in movement range go to target
            //if they are at the same y it must be offset by the x
            if (targetUnit.transform.position.y == controlledUnit.transform.position.y)
            {
                _targetY = targetUnit.transform.position.y;
                //Check if target is to the left or the right of controlled unit
                if (targetUnit.transform.position.x > controlledUnit.transform.position.x)
                {
                    _targetX = (targetUnit.transform.position.x - 1f); //offset x by -1
                }
                else if (targetUnit.transform.position.x < controlledUnit.transform.position.x)
                {
                    _targetX = (targetUnit.transform.position.x + 1f); //offset x by +1
                }
            }
            else
            {
                _targetX = targetUnit.transform.position.x;
                //Check if target is above or below controlled unit
                if (targetUnit.transform.position.y > controlledUnit.transform.position.y)
                {
                    _targetY = (targetUnit.transform.position.y - 1f); //offset y by -1
                }
                else if (targetUnit.transform.position.y < controlledUnit.transform.position.y)
                {
                    _targetY = (targetUnit.transform.position.y + 1f); //offset y by +1
                }
            }
        }

        //todo: make sure its in range and can actually move to that tile if it can't only move as far as it can move.


        //Move controlled unit to the new target tile
        controlledUnit.transform.position = new Vector3(_targetX, _targetY, 0);
        _controlledTile.RemoveUnit();
        _controlledTile = tiles[new Vector2(_targetX, _targetY)];
        _controlledTile.AssignUnit(controlledUnit);
        //Debug.Log(_targetY);
        //Debug.Log(_targetX);
    }


    private List<Tile> FindPath(int startX, int startY, int endX, int endY, Dictionary<Vector2, Tile> tiles)
    {
        Tile _startTile = tiles[new Vector2(startX, startY)];

        openList = new List<Tile> { _startTile };
        closedList = new List<Tile>();

        return null;
    }


}
