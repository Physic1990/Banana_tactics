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
            SetWarrior(); //calls these functions to set varibles to class specific traits
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

    //sets specofoc varibalies
    void SetWarrior()
    {
        health = 100;
        movement = 2;
        attack1 = "Banana Slam";
        attack2 = "Banana Slam";
        //SetAttacks();
    }

    //sets specofoc varibalies
    void SetGunSlinger()
    {
        health = 100;
        movement = 3;
        attack1 = "Shoot";
        attack2 = "Shoot";
        //SetAttacks();
    }

    //when called it will set all attacks stats based on what attack you are refrencing (in code it is called when people want the stats)
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
            SetAttacks(attack);
        }
    }

    //can be called to destroy the game object attached
    public void DestroyUnit()
    {
        Destroy(this.gameObject);
    }

    //sets the healt values to a value between 0 and 100
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

    //subtracts a value from health
    public void DealDamage(int changeHealthAmount)
    {
        //subtract value from health making sure it cant go below min health
        if ((health -= changeHealthAmount) < minHealth)
        {
            health = minHealth;
        }
        else
        {
            health -= changeHealthAmount;
        }
    }

    //adds value to health
    public void GainHealth(int changeHealthAmount)
    {
        //add value to health making sure it cant go ove rthe max health
        if ((health += changeHealthAmount) > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += changeHealthAmount;
        }
    }

    //returns curret health value
    public int GetHealth()
    {
        //current health 
        return health;
    }

    //returns how much the unit can move
    public int GetMovement()
    {
        //give how many tiles the unit can move
        return movement;
    }

    //Dummy Holder So Peoples Code does not return errors
    public double[] GetAttackStats()
    {
        attackValuesOne[0] = 0;
        attackValuesOne[1] = 0;
        attackValuesOne[2] = 0;
        attackValuesOne[3] = 0;
        attackValuesOne[4] = 0;
        return attackValuesOne;
    }

    //get the fist attack stats
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

    //get the second attack stats
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

    //gives both names in order in a string array
    public string[] GetAttackNames()
    {
        //sets attacks string names in order
        attackNames[0] = attack1;
        attackNames[1] = attack2;
        return attackNames;
    }

    //will set if the unit has taken theri attack action
    public void SetActed(bool status)
    {
        hasActed = status;
        if (status) //makes visual style that the unit has moved
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

    //says if action has been done
    public bool HasActed()
    {
        return hasActed;
    }

    //can gray out units
    private void GrayOut()
    {
        //get color
        Color grayColor = new Color(0.5f, 0.5f, 0.5f, spriteRenderer.color.a);
        //make color gray
        spriteRenderer.color = grayColor;
    }

    //if grayed out this returns it to normal color
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
