using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class UnitAttributes : MonoBehaviour
{
    public string whatClass;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    [SerializeField] bool IsPlayer = true;
    [SerializeField] bool IsEnemy = false;
    [SerializeField] bool hasActed = false;

    public bool hasTeleported = false;

    private bool modeBC = false;

    private int health;
    private int minHealth = 0;
    private int maxHealth = 100;
    private int movement;
    private int attackRange;
    private double attackCritChance;
    private double attackHitChance;
    private int attackDamage;
    private int attackDamageCrit;
    private int instanceIDObject;
    private int instanceIDScript;

    private string attack1;
    private string attack2;

    private double[] attackValuesOne = new double[5];
    private double[] attackValuesTwo = new double[5];
    private string[] attackNames = new string[2];

    /// <summary>
    /// Start Of Data Set Functions
    /// </summary>

    private void SetClassStats()
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
        else if (whatClass == "Lancer")
        {
            SetLancer();
        }
        else if (whatClass == "Rogue")
        {
            SetRogue();
        }
        else if (whatClass == "Hero")
        {
            SetHero();
        }
        else
        {
            //defult is warrior if wrong string or none had been entered
            SetWarrior();
        }
    }

    //sets specific varibalies
    private void SetWarrior()
    {
        health = 100;
        maxHealth = health;
        movement = 2;
        attack1 = "Banana Slam";
        attack2 = "Banana Punch";
        //SetAttacks();

        if (modeBC == true && IsEnemy == true)
        {
            health = 1;
        }
        else if (modeBC == true && IsPlayer == true)
        {
            health = 999;
        }
    }

    //sets specific varibalies
    private void SetGunSlinger()
    {
        health = 100;
        maxHealth = health;
        movement = 1;
        attack1 = "Shoot";
        attack2 = "Banana Punch";
        //SetAttacks();

        if (modeBC == true && IsEnemy == true)
        {
            health = 1;
        }
        else if (modeBC == true && IsPlayer == true)
        {
            health = 999;
        }
    }

    private void SetLancer()
    {
        health = 100;
        maxHealth = health;
        movement = 3;
        attack1 = "Banana Thrust";
        attack2 = "Banana Punch";
        //SetAttacks();

        if (modeBC == true && IsEnemy == true)
        {
            health = 1;
        }
        else if (modeBC == true && IsPlayer == true)
        {
            health = 999;
        }
    }

    private void SetRogue()
    {
        health = 100;
        maxHealth = health;
        movement = 2;
        attack1 = "Banana Backstab";
        attack2 = "Banana Punch";
        //SetAttacks();

        if (modeBC == true && IsEnemy == true)
        {
            health = 1;
        }
        else if (modeBC == true && IsPlayer == true)
        {
            health = 999;
        }
    }

    private void SetHero()
    {
        health = 100;
        maxHealth = health;
        movement = 1;
        attack1 = "Banana Ultimate Punch";
        attack2 = "Banana Punch";
        //SetAttacks();

        if (modeBC == true && IsEnemy == true)
        {
            health = 1;
        }
        else if (modeBC == true && IsPlayer == true)
        {
            health = 999;
        }
    }

    //when called it will set all attacks stats based on what attack you are refrencing (in code it is called when people want the stats)
    private void SetAttacks(string attack)
    {
        if (attack == "Banana Punch")
        {
            attackCritChance = 0.2;
            attackHitChance = 0.99;
            attackRange = 10;
            attackDamage = 5;
            attackDamageCrit = attackDamage * 2;

        }
        else if (attack == "Banana Slam")
        {
            attackCritChance = 0.35;
            attackHitChance = 0.75;
            attackRange = 1;
            attackDamage = 20;
            attackDamageCrit = attackDamage * 2;
        }
        else if (attack == "Shoot")
        {
            attackCritChance = 0.1;
            attackHitChance = 0.70;
            attackRange = 4;
            attackDamage = 20;
            attackDamageCrit = attackDamage * 2;
        }
        else if (attack == "Banana Thrust")
        {
            attackCritChance = 0.3;
            attackHitChance = 0.85;
            attackRange = 2;
            attackDamage = 20;
            attackDamageCrit = attackDamage * 2;
        }
        else if (attack == "Banana Backstab")
        {
            attackCritChance = 0.5;
            attackHitChance = .8;
            attackRange = 1;
            attackDamage = 12;
            attackDamageCrit = attackDamage * 3;
        }
        else if (attack == "Banana Ultimate Punch")
        {
            attackCritChance = 0.1;
            attackHitChance = 0.7;
            attackRange = 1;
            attackDamage = 35;
            attackDamageCrit = attackDamage * 2;
        }
        //makes all player characters do insane damage and enemys zero if bc mode is on
        else if (modeBC == true)
        {
            if (IsPlayer == true)
            {
                attackCritChance = 1;
                attackHitChance = 1;
                attackRange = 999;
                attackDamage = 999;
                attackDamageCrit = attackDamage * 10;
            }
            else if (IsEnemy == true)
            {
                attackCritChance = 0;
                attackHitChance = 0;
                attackRange = 0;
                attackDamage = 0;
                attackDamageCrit = attackDamage * 0;
            }
        }
        else
        {
            attack = "Banana Punch";
            SetAttacks(attack);
        }
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

    //cahnges the sprite to player or enemy one
    private void ChangeUnitSprite()
    {
        // Get the SpriteRenderer component of the GameObject
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        //set path to player sprite
        string spritePathPlayer = "Assets/Artwork/playerMonkey.png";
        Sprite playerSprite = Resources.Load<Sprite>(spritePathPlayer);

        //set path to enemy sprite
        string spritePathEnemy = "Assets/Artwork/alt_enemySprite.png";
        Sprite enemySprite = Resources.Load<Sprite>(spritePathEnemy);

        if (spriteRenderer != null)
        {
            // Change the sprite based on the unit type
            if (IsPlayer == true && playerSprite != null)
            {
                spriteRenderer.sprite = playerSprite;
            }
            else if (IsEnemy == true && enemySprite != null)
            {
                spriteRenderer.sprite = enemySprite;
            }
            else
            {
                Debug.LogWarning("Sprite not assigned for the unit type.");
            }
        }
        else
        {
            Debug.LogWarning("SpriteRenderer component not found.");
        }
    }

    /// <summary>
    /// START OF PUBLIC FUNCTION
    /// </summary>

    //can be called to destroy the game object attached
    public void DestroyUnit()
    {
        Destroy(this.gameObject);
    }

    //sets the healt values to a value between 0 and 100
    public virtual void SetHealth(int changeHealthAmount)
    {
        if (changeHealthAmount != 0)
        {
            //changes health to new health value
            if (changeHealthAmount < 0)
            {
                changeHealthAmount = minHealth;
            }
            else if (changeHealthAmount > 0)
            {
                changeHealthAmount = maxHealth;
            }

            health = changeHealthAmount;

            if (modeBC == true)
            {
                if (IsPlayer == true)
                {
                    changeHealthAmount = 999;
                }
                else if (IsEnemy == true)
                {
                    changeHealthAmount = 1;
                }
            }
        }
        else
        {
            Debug.Log("Not A Valid Input For Set Health");
        }
    }

    public virtual void SetHealth(float changeHealthAmount)
    {
        //changes float to an int
        int roundedValue = (int)changeHealthAmount;
        SetHealth(roundedValue);
    }

    //subtracts a value from health
    public void DealDamage(int changeHealthAmount)
    {
        if (changeHealthAmount != 0)
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

            if (modeBC == true)
            {
                if (IsPlayer == true)
                {
                    changeHealthAmount = 999;
                }
                else if (IsEnemy == true)
                {
                    changeHealthAmount = 1;
                }
            }
        }
        else
        {
            Debug.Log("Not A Valid Input For Deal Damage");
        }
    }

    //adds value to health
    public void GainHealth(int changeHealthAmount)
    {
        if (changeHealthAmount != 0)
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

            if (modeBC == true)
            {
                if (IsPlayer == true)
                {
                    changeHealthAmount = 999;
                }
                else if (IsEnemy == true)
                {
                    changeHealthAmount = 1;
                }
            }
        }
        else
        {
            Debug.Log("Not A Valid Input For Gain Health");
        }
    }

    //returns curret health value
    public int GetHealth()
    {
        //current health 
        return health;
    }

    //returns maximum health value
    public int GetMaxHealth()
    {
        //max health 
        return maxHealth;
    }

    //returns how much the unit can move
    public int GetMovement()
    {
        //give how many tiles the unit can move
        return movement;
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

    //triggers bc mode which makes all player units invinciblie and all enemy units one shot
    public void TriggerBC()
    {
        if (modeBC == false)
        {
            modeBC = true;
        }
        else
        {
            modeBC = false;
        }


    }

    //Gets Instance ID of object attached to script
    public int GetInstnaceIDOfGameObject()
    {
        return instanceIDObject;
    }

    //Gets Instance ID of the script
    public int GetInstnaceIDOfScript()
    {
        return instanceIDScript;
    }


    /// <summary>
    /// Intilization
    /// </summary>

    void Awake()
    {
        SetClassStats();
        instanceIDObject = gameObject.GetInstanceID();
        instanceIDScript = GetHashCode();

        // Debug.Log statements for testing
        // Debug.Log(health);
        // Debug.Log(movement);
        // Debug.Log(attackRange);
        // Debug.Log(attackCritChance);
        // Debug.Log(attackHitChance);
        // Debug.Log(attackDamage);
        // Debug.Log(attackDamageCrit);

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        // You have to manually toggle enemy so if it's true then it's supposed to be an enemy unit
        if (IsEnemy == true && IsPlayer == true)
        {
            IsPlayer = false;
        }

        ChangeUnitSprite();

        GameObject bcModeTriggerObject = GameObject.Find("BCModeManager");

        if (bcModeTriggerObject != null)
        {
            // Get the BCModeTrigger script from the GameObject
            BCModeTrigger bcModeTriggerScript = bcModeTriggerObject.GetComponent<BCModeTrigger>();

            if (bcModeTriggerScript != null)
            {
                // Access the BCMode variable from BCModeTrigger script
                bool BCMode = bcModeTriggerScript.BCMode;

                if (BCMode == true)
                {
                    modeBC = true;
                    Debug.Log("BCMode is true!");
                }
                else
                {
                    modeBC = false;
                    Debug.Log("BCMode is false!");
                }
            }
            else
            {
                Debug.LogError("BCModeTrigger script not found on the GameObject.");
            }
        }
        else
        {
            Debug.LogError("GameObject with BCModeTrigger script not found.");
        }
    }
}