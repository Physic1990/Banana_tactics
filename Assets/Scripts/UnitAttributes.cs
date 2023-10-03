using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UnitAttributes : MonoBehaviour
{
    public string whatClass = "Warrior";

    public bool IsPlayer = true;
    public bool IsEnemy = false;

    int health;
    private int movement;
    private int attackRange;
    private double attackCritChance;
    private double attackHitChance;
    private int attackDamage;
    private int attackDamageCrit;
    
    string attack1 = "Punch";

    double[] attackValues = new double[5];

    ArrayList stats = new ArrayList(8);
    void SetClassStats()
    {
        if (whatClass == "Warrior")
        {
            SetWarrior();
        } else if (whatClass == "Gunslinger")
        {
            SetGunSlinger();
        } else {
            SetWarrior();
        }
    }
    void SetWarrior()
    {
        health = 50;
        movement = 2;
        attack1 = "Punch";
        SetAttacks();
    }
    void SetGunSlinger()
    {
        health = 40;
        movement = 3;
        attack1 = "Shoot";
        SetAttacks();
    }

    void SetAttacks()
    {
        if (attack1 == "Punch")
        {
            attackCritChance = 0.2;
            attackHitChance = 0.8;
            attackRange = 1;
            attackDamage = 5;
            attackDamageCrit = attackDamage * 2;

        } else if (attack1 == "Shoot") {
            attackCritChance = 0.1;
            attackHitChance = 0.7;
            attackRange = 2;
            attackDamage = 6;
            attackDamageCrit = attackDamage * 2;
        } else {
            attack1 = "Punch";
            SetAttacks();
        }
    }

    public void Sethealth(int changeHealthAmount)
    {
        health += changeHealthAmount;
    }

    public int GetHealth()
    {
        return health;
    }
    public int GetMovement()
    {
        return movement;
    }
    public string GetAttackName()
    {
        return attack1;
    }
    public double[] GetAttackStats()
    {
        attackValues[0] = attackDamage;
        attackValues[1] = attackRange;
        attackValues[2] = attackHitChance;
        attackValues[3] = attackCritChance;
        attackValues[4] = attackDamageCrit;
        return attackValues;
    }
    void Start()
    {
        SetClassStats();
        Debug.Log(health);
        Debug.Log(movement);
        Debug.Log(attackRange);
        Debug.Log(attackCritChance);
        Debug.Log(attackHitChance);
        Debug.Log(attackDamage);
        Debug.Log(attackDamageCrit);
}
}
