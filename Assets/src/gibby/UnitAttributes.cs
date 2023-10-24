using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UnitAttributes : MonoBehaviour
{
    public string whatClass;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public bool IsPlayer = true;
    public bool IsEnemy = false;
    public bool hasActed = false;

    int health;
    private int minHealth = 0;
    private int maxHealth = 100;
    private int movement;
    private int attackRange;
    private double attackCritChance;
    private double attackHitChance;
    private int attackDamage;
    private int attackDamageCrit;

    string attack1 = "Punch";
    string attack2 = "Punch";

    double[] attackValuesOne = new double[5];
    double[] attackValuesTwo = new double[5];
    string[] attackNames = new string[2];

    void SetClassStats()
    {
        //checks what class unit has been set too
        if (whatClass == "Warrior")
        {
            SetWarrior();
        } 
        else if (whatClass == "Gunslinger") 
        {
            SetGunSlinger();
        } 
        else 
        {
            //defult is warrior if wrong string or none had been entered
            SetWarrior();
        }
    }

    void SetWarrior()
    {
        health = 100;
        movement = 2;
        attack1 = "Banana Slam";
        attack2 = "Banana Slam";
        //SetAttacks();
    }

    void SetGunSlinger()
    {
        health = 100;
        movement = 3;
        attack1 = "Shoot";
        attack2 = "Shoot";
        //SetAttacks();
    }

    void SetAttacks(string attack)
    {
        if (attack == "Banana Slam")
        {
            attackCritChance = 0.2;
            attackHitChance = 0.8;
            attackRange = 1;
            attackDamage = 5;
            attackDamageCrit = attackDamage * 2;

        } 
        else if (attack == "Shoot") 
        {
            attackCritChance = 0.1;
            attackHitChance = 0.7;
            attackRange = 2;
            attackDamage = 6;
            attackDamageCrit = attackDamage * 2;
        } 
        else 
        {
            attack = "Banana Slam";
            SetAttacks();
        }
    }

    public void DestroyUnit()
    {
        Destroy(this.gameObject);
    }

    public void SetHealth(int changeHealthAmount)
    {
        //changes health to new health value
        if(changeHealthAmount < 0)
        {
            changeHealthAmount = minHealth;
        }
        else if (changeHealthAmount > 0)
        {
            changeHealthAmount = maxHealth;
        }

        health = changeHealthAmount;
    }

    public void DealDamage(int changeHealthAmount)
    {
        //subtract value from health
        if ((health -= changeHealthAmount) < minHealth)
        {
            health = minHealth;
        }
        else
        {
            health -= changeHealthAmount;
        }
    }

    public void GainHealth(int changeHealthAmount)
    {
        //add value to health
        if ((health += changeHealthAmount) > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += changeHealthAmount;
        }
    }

    public int GetHealth()
    {
        //current health 
        return health;
    }

    public int GetMovement()
    {
        //give how many tiles the unit can move
        return movement;
    }

    public void SetActed(bool status)
    {
        hasActed = status;
        if (status)
        {
            //makes sprite gray
            GrayOut();
        } 
        if (!status) 
        {
            //reverts back to og sprite color
            RevertToOriginalColor();
        }
    }

    public bool HasActed()
    {
        //says if action has been done
        return hasActed;

    }

    public string GetAttackName()
    {
        //return attack name
        return attack1;
    }

    public double[] GetAttackOneStats()
    {
        SetAttacks(attack1); //Sets the current value based on which one you are asking for
        //order in which you get the stats
        attackValuesOne[0] = attackDamage;
        attackValuesOne[1] = attackRange;
        attackValuesOne[2] = attackHitChance;
        attackValuesOne[3] = attackCritChance;
        attackValuesOne[4] = attackDamageCrit;
        return attackValuesOne;
    }

    public double[] GetAttackTwoStats()
    {
        SetAttacks(attack2); //Sets the current value based on which one you are asking for
        //order in which you get the stats
        attackValuesTwo[0] = attackDamage;
        attackValuesTwo[1] = attackRange;
        attackValuesTwo[2] = attackHitChance;
        attackValuesTwo[3] = attackCritChance;
        attackValuesTwo[4] = attackDamageCrit;
        return attackValuesTwo;
    }

    public string[] GetAttackNames()
    {
        //sets attacks string names in order
        attackNames[0] = attack1;
        attackNames[1] = attack2;
        return attackNames;
    }

    private void GrayOut()
    {
        //get color
        Color grayColor = new Color(0.5f, 0.5f, 0.5f, spriteRenderer.color.a);
        //make color gray
        spriteRenderer.color = grayColor;
    }

    private void RevertToOriginalColor()
    {
        //normal color set       
        spriteRenderer.color = originalColor;
    }

    void Start()
    {
        SetClassStats();
        //Debug.Log(health);
        //Debug.Log(movement);
        //Debug.Log(attackRange);
        //Debug.Log(attackCritChance);
        //Debug.Log(attackHitChance);
        //Debug.Log(attackDamage);
        //Debug.Log(attackDamageCrit);

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }
}
