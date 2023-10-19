using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UnitAttributes : MonoBehaviour
{
    public string whatClass = "Warrior";
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

    double[] attackValues = new double[5];

    ArrayList stats = new ArrayList(8);
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
        attack1 = "Punch";
        SetAttacks();
    }

    void SetGunSlinger()
    {
        health = 100;
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

        } 
        else if (attack1 == "Shoot") 
        {
            attackCritChance = 0.1;
            attackHitChance = 0.7;
            attackRange = 2;
            attackDamage = 6;
            attackDamageCrit = attackDamage * 2;
        } 
        else 
        {
            attack1 = "Punch";
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

    public double[] GetAttackStats()
    {
        //order in which you get the stats
        attackValues[0] = attackDamage;
        attackValues[1] = attackRange;
        attackValues[2] = attackHitChance;
        attackValues[3] = attackCritChance;
        attackValues[4] = attackDamageCrit;
        return attackValues;
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
