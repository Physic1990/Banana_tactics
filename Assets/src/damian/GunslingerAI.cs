using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GunslingerAI : MonoBehaviour
{
    public int GunslingerFindTargetUnit(List<GameObject> playerUnits, GameObject enemyUnit)
    {
        int j = 0;
        float _minHealth = Mathf.Infinity;
        float _health = 0f;
        int _targetUnit = 0;
        for (j = 0; j < playerUnits.Count; j++)
        {
            _health = playerUnits[j].GetComponent<UnitAttributes>().GetHealth();
            if (_minHealth > _health)
            {
                _targetUnit = j;
                _minHealth = _health;
            }
        }
        return _targetUnit;
    }
}
