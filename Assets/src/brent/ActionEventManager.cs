using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Events;

public class battleCalculator
{
   public int muiltplier()
   {
      return(200);
   }
}

public class specialBattleCalculator : battleCalculator
{
   public  int muiltplier()
   {
      return(100);
   }
}

public class ActionEventManager : MonoBehaviour
{
   public battleCalculator test;

   
   // observer pattern triggers
   public static event Action onDeath;
   public static event Action onEnemyDeath;
   public static event Action onAttack;
   public static event Action onHealth;
   public static event Action onP1;
   public static event Action onP2;
   public static event Action onP3;
   public static event Action onP4;
   public static event Action onP5;
   public static event Action onE1;
   public static event Action onE2;
   public static event Action onE3;
   public static event Action onE4;
   public static event Action onE5;

   // unit's data
   [SerializeField] UnitAttributes _unitAttributes;
   [SerializeField] UnitAttributes _enemyUnitAttributes;
   [SerializeField] UnitAttributes _allyUnitAttributes;
   // unit's animation object data
   [SerializeField] DeathAnimation _deathAnimationUnit;
   [SerializeField] DeathAnimation _deathAnimationEnemy;
   [SerializeField] private UnityEvent _attackOver;

   // singleton locking mechanism
   private static ActionEventManager instance;
   private static readonly object padlock = new object();

   // single instance
   private ActionEventManager() { }
   public static ActionEventManager Instance
   {
      get { return instance; }
   }

   // creating only 1 instance
   void Awake()
   {
      test = new specialBattleCalculator();
      Debug.Log("calculation: " + test.muiltplier());

      // locking mechanism for singleton pattern
      lock (padlock)
      {
         // 1 existing instance
         if (instance == null)
         {
            instance = this;
         }
      }
   }

   // event running status
   private bool status;
   // type of event 
   private int eventType;
   // type of terrain tile
   private int terrain;
   // data structure for player unit
   private playerAttributes player = new playerAttributes();
   // data structure for enemy unit
   private enemyAttributes enemy = new enemyAttributes();
   // data structure for ally unit 
   private allyAttributes ally = new allyAttributes();

   //player attributes
   private class playerAttributes
   {
      // unit's health
      public int health;
      // unit's attack damage for an attack
      public int attackDamage;
      // unit's attack range for an attack
      public int attackRange;
      // unit's hit chance ratio for an critical attack
      public int attackCritChance;
      // unit's hit chance ratio for an attack
      public int attackHitChance;
      // unit's attack damage for a critical attack
      public int attackDamageCrit;
      // unit's healing power
      public int healIncrease;
      // default attributes
      public playerAttributes()
      {
         health = 100;
         attackDamage = 0;
         attackRange = 0;
         attackCritChance = 0;
         attackHitChance = 0;
         attackDamageCrit = 0;
         healIncrease = 0;
         healIncrease = 0;
      }
   }

   // enemy attributes
   private class enemyAttributes
   {
      // enemy's health
      public int health;
      // enemy's attack damage for an attack
      public int attackDamage;
      // enemy's attack range for an attack
      public int attackRange;
      // enemy's hit chance ratio for an critical attack
      public int attackCritChance;
      // enemy's hit chance ratio for an attack
      public int attackHitChance;
      // enemy's attack damage for a critical attack
      public int attackDamageCrit;
      // enemy's healing power
      public int healIncrease;
      // default attributes
      public enemyAttributes()
      {
         health = 100;
         attackDamage = 0;
         attackRange = 0;
         attackCritChance = 0;
         attackHitChance = 0;
         attackDamageCrit = 0;
         healIncrease = 0;
      }
   }

   //ally attributes
   private class allyAttributes
   {
      // ally's health
      public int health;
      // ally's attack damage for an attack
      public int attackDamage;
      // ally's attack range for an attack
      public int attackRange;
      // ally's hit chance ratio for an critical attack
      public int attackCritChance;
      // ally's hit chance ratio for an attack
      public int attackHitChance;
      // ally's attack damage for a critical attack
      public int attackDamageCrit;
      // ally's healing power
      public int healIncrease;
      // default attributes
      public allyAttributes()
      {
         health = 100;
         attackDamage = 0;
         attackRange = 0;
         attackCritChance = 0;
         attackHitChance = 0;
         attackDamageCrit = 0;
         healIncrease = 0;
      }
   }


   // units action type: when they engage an enemy for battle
   public void attackBattle(GameObject unit, GameObject enemyUnit)
   {
      // get players data
      _unitAttributes = unit.GetComponent<UnitAttributes>();
      _deathAnimationUnit = unit.GetComponent<DeathAnimation>();
      // get enemy data
      _enemyUnitAttributes = enemyUnit.GetComponent<UnitAttributes>();
      _deathAnimationEnemy = unit.GetComponent<DeathAnimation>();

      // get unit's attributes
      double[] playerAtt = _unitAttributes.GetAttackOneStats();
      double[] enemyAtt = _enemyUnitAttributes.GetAttackOneStats();
      // intialize attributes health
      player.health = _unitAttributes.GetHealth();
      enemy.health = _enemyUnitAttributes.GetHealth();
      // intialize attributes for damage of attack
      player.attackDamage = (int)playerAtt[0];
      enemy.attackDamage = (int)enemyAtt[0];
      // intialize attributes for damage of attack
      player.attackRange = (int)playerAtt[1];
      enemy.attackRange = (int)enemyAtt[1];
      // intialize attributes for 1st attack hit chance probability
      player.attackHitChance = (int)(playerAtt[2] * 100);
      enemy.attackHitChance = (int)(enemyAtt[2] * 100);
      // intialize attributes for probality of bonus critical attack
      player.attackCritChance = (int)(playerAtt[3] * 100);
      enemy.attackCritChance = (int)(enemyAtt[3] * 100);
      // intialize attributes for damage of critical hit attack
      player.attackDamageCrit = (int)(playerAtt[4]);
      enemy.attackDamageCrit = (int)(enemyAtt[4]);

      // attack animation
      if (Equals(unit.name, "player warrior 1") == true)
      {
         onP1?.Invoke();
         onAttack?.Invoke();
      }

      if (Equals(unit.name, "player gunslinger 2") == true)
      {
         onP2?.Invoke();
         onAttack?.Invoke();
      }

      if (Equals(unit.name, "player rogue 3") == true)
      {
         onP3?.Invoke();
         onAttack?.Invoke();
      }

      if (Equals(unit.name, "player hero 4") == true)
      {
         onP4?.Invoke();
         onAttack?.Invoke();
      }

      if (Equals(unit.name, "player lancer 5") == true)
      {
         onP5?.Invoke();
         onAttack?.Invoke();
      }

      if (Equals(unit.name, "enemy warrior 10") == true)
      {
         onE1?.Invoke();
         onAttack?.Invoke();
      }

      if (Equals(unit.name, "enemy warrior 11") == true)
      {
         onE2?.Invoke();
         onAttack?.Invoke();
      }

      if (Equals(unit.name, "enemy warrior 12") == true)
      {
         onE3?.Invoke();
         onAttack?.Invoke();
      }

      if (Equals(unit.name, "enemy warrior 13") == true)
      {
         onE4?.Invoke();
         onAttack?.Invoke();
      }

      if (Equals(unit.name, "enemy warrior 14") == true)
      {
         onE5?.Invoke();
         onAttack?.Invoke();
      }

      // player was hit
      if (Random.Range(0, 100) < enemy.attackHitChance)
      {
         _unitAttributes.DealDamage(enemy.attackDamage);
         // critical hit 
         if (Random.Range(0, 100) < (enemy.attackCritChance))
         {
            _unitAttributes.DealDamage(enemy.attackDamageCrit);
         }
      }
      // enemy was hit
      if (Random.Range(0, 100) < player.attackHitChance)
      {
         _enemyUnitAttributes.DealDamage(player.attackDamage);
         // critical hit
         if (Random.Range(0, 100) < (player.attackCritChance))
         {
            _enemyUnitAttributes.DealDamage(player.attackDamageCrit);
         }
      }

      // player unit has died
      if (_unitAttributes.GetHealth() < 1)
      {
         // 
         if (Equals(unit.name, "player warrior 1") == true)
         {
            onP1?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "player gunslinger 2") == true)
         {
            onP2?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "player rogue 3") == true)
         {
            onP3?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "player hero 4") == true)
         {
            onP4?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "player lancer 5") == true)
         {
            onP5?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "enemy warrior 10") == true)
         {
            onE1?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "enemy warrior 11") == true)
         {
            onE2?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "enemy warrior 12") == true)
         {
            onE3?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "enemy warrior 13") == true)
         {
            onE4?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "enemy warrior 14") == true)
         {
            onE5?.Invoke();
            onDeath?.Invoke();
         }
      }

      // enemy unit has died
      if (_enemyUnitAttributes.GetHealth() < 1)
      {
         if (Equals(unit.name, "player warrior 1") == true)
         {
            onP1?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "player gunslinger 2") == true)
         {
            onP2?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "player rogue 3") == true)
         {
            onP3?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "player hero 4") == true)
         {
            onP4?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "player lancer 5") == true)
         {
            onP5?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "enemy warrior 10") == true)
         {
            onE1?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "enemy warrior 11") == true)
         {
            onE2?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "enemy warrior 12") == true)
         {
            onE3?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "enemy warrior 13") == true)
         {
            onE4?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "enemy warrior 14") == true)
         {
            onE5?.Invoke();
            onDeath?.Invoke();
         }

      }
      
   }


   // Action Event: heals a hurt unit on players turn
   // unit - is the player that will heal a unit
   // hurtUnit - is the unit that needs to be healed
   public void healSelf(GameObject unit)
   {
      // get players data
      _unitAttributes = unit.GetComponent<UnitAttributes>();
      player.healIncrease = 5;
      // heal ally
      _unitAttributes.GainHealth(player.healIncrease);
      // observation signal
      if (Equals(unit.name, "player warrior 1") == true)
      {
         onP1?.Invoke();
         onHealth?.Invoke();
      }

      if (Equals(unit.name, "player gunslinger 2") == true)
      {
         onP2?.Invoke();
         onHealth?.Invoke();
      }

      if (Equals(unit.name, "player rogue 3") == true)
      {
         onP3?.Invoke();
         onHealth?.Invoke();
      }

      if (Equals(unit.name, "player hero 4") == true)
      {
         onP4?.Invoke();
         onHealth?.Invoke();
      }

      if (Equals(unit.name, "player lancer 5") == true)
      {
         onP5?.Invoke();
         onHealth?.Invoke();
      }

      if (Equals(unit.name, "enemy warrior 10") == true)
      {
         onE1?.Invoke();
         onHealth?.Invoke();
      }

      if (Equals(unit.name, "enemy warrior 11") == true)
      {
         onE2?.Invoke();
         onHealth?.Invoke();
      }

      if (Equals(unit.name, "enemy warrior 12") == true)
      {
         onE3?.Invoke();
         onHealth?.Invoke();
      }

      if (Equals(unit.name, "enemy warrior 13") == true)
      {
         onE4?.Invoke();
         onHealth?.Invoke();
      }

      if (Equals(unit.name, "enemy warrior 14") == true)
      {
         onE5?.Invoke();
         onHealth?.Invoke();
      }
   }



   // Action Event: a action that is used for general movement 
   // with no other action, unit will suffer terrain damage
   public void doNothingTurn(GameObject unit)
   {
      // temporay terrain damage
      int terrain = 1;
      //get unit's data
      _unitAttributes = unit.GetComponent<UnitAttributes>();
      _deathAnimationUnit = unit.GetComponent<DeathAnimation>();
      // unit suffers terrain damage
      if (Random.Range(0, 100) < 100)
      {
         _unitAttributes.DealDamage(terrain);
      }
      // animate death when health is less than 1
      if (_unitAttributes.GetHealth() <= 0)
      {
         // death signal
         if (Equals(unit.name, "player warrior 1") == true)
         {
            onP1?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "player gunslinger 2") == true)
         {
            onP2?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "player rogue 3") == true)
         {
            onP3?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "player hero 4") == true)
         {
            onP4?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "player lancer 5") == true)
         {
            onP5?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "enemy warrior 10") == true)
         {
            onE1?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "enemy warrior 11") == true)
         {
            onE2?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "enemy warrior 12") == true)
         {
            onE3?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "enemy warrior 13") == true)
         {
            onE4?.Invoke();
            onDeath?.Invoke();
         }

         if (Equals(unit.name, "enemy warrior 14") == true)
         {
            onE5?.Invoke();
            onDeath?.Invoke();
         }
      }
   }

   // get the current update of any unit health on the game board
   public int getUpdateUnitHealth(GameObject unit)
   {
      // get current unit attributes
      _unitAttributes = unit.GetComponent<UnitAttributes>();
      return _unitAttributes.GetHealth();
   }

   // used for testing: player health during the event, health of internal data structure
   public int getPlayerEventHealth()
   {
      return (player.health);
   }

   // used for testing: enemy health during the event, health of internal data structure
   public int getEnemyEventHealth()
   {
      return (enemy.health);
   }

   // set a temporary data structure for player health
   public void setPlayerHealth(int hp)
   {
      player.health = hp;
   }

   // set a temporary data structure for enemy health
   public void setEnemyHealth(int hp)
   {
      enemy.health = hp;
   }
}
