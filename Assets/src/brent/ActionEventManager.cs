using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class ActionEventManager : MonoBehaviour
{
   public static event Action OnDeath;
   public static event Action OnEnemyDeath;

   // unit's data
   [SerializeField] UnitAttributes _unitAttributes;
   [SerializeField] UnitAttributes _enemyUnitAttributes;
   [SerializeField] UnitAttributes _allyUnitAttributes;
   [SerializeField] DeathAnimation _deathAnimationUnit;
   [SerializeField] DeathAnimation _deathAnimationEnemy;
   
   // Singleton Template
   private static ActionEventManager instance; // = new ActionEventManager();
   private static readonly object padlock = new object();

   private ActionEventManager(){}
   public static ActionEventManager Instance
   {
      get{return instance;}
   }

   void Awake()
   {
     
         lock(padlock)
         {
            if(instance == null){
               instance=this;
            }
         }
      
   } 

   // event running status
   private bool status;
   // the type of event 
   private int eventType;
   //private int playerHealth;
   private int terrain;
   private playerAttributes player = new playerAttributes();
   private enemyAttributes  enemy = new enemyAttributes();
   private allyAttributes  ally = new allyAttributes();

   //player attributes
   private class playerAttributes
   {
      public int health;
      public int attackDamage;
      public int attackRange;
      public int attackCritChance;
      public int attackHitChance;
      public int attackDamageCrit;
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
      public int health;
      public int attackDamage;
      public int attackRange;
      public int attackCritChance;
      public int attackHitChance;
      public int attackDamageCrit;
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
      public int health;
      public int attackDamage;
      public int attackRange;
      public int attackCritChance;
      public int attackHitChance;
      public int attackDamageCrit;
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
   public void attackBattle (GameObject unit, GameObject enemyUnit)
   {
      // get players data
      _unitAttributes = unit.GetComponent<UnitAttributes>();
      _deathAnimationUnit = unit.GetComponent<DeathAnimation>();
      // get enemy data
      _enemyUnitAttributes = enemyUnit.GetComponent<UnitAttributes>();
      _deathAnimationEnemy = unit.GetComponent<DeathAnimation>();


      // get unit's attributes
      double [] playerAtt = _unitAttributes.GetAttackOneStats();
      double [] enemyAtt = _enemyUnitAttributes.GetAttackOneStats();
      // intialize attributes health
      player.health = _unitAttributes.GetHealth();
      enemy.health = _enemyUnitAttributes.GetHealth();
      // intialize attributes for damage of attack
      player.attackDamage=(int)playerAtt[0];
      enemy.attackDamage=(int)enemyAtt[0];
      // intialize attributes for damage of attack
      player.attackRange=(int)playerAtt[1];
      enemy.attackRange=(int)enemyAtt[1];
      // intialize attributes for 1st attack hit chance probability
      player.attackHitChance=(int)(playerAtt[2]*100);
      enemy.attackHitChance=(int)(enemyAtt[2]*100);
      // intialize attributes for probality of bonus critical attack
      player.attackCritChance=(int)(playerAtt[3]*100);
      enemy.attackCritChance=(int)(enemyAtt[3]*100);
      // intialize attributes for damage of critical hit attack
      player.attackDamageCrit=(int)(playerAtt[4]);
      enemy.attackDamageCrit=(int)(enemyAtt[4]);


      // player was hit
      if(Random.Range(0, 100) < enemy.attackHitChance)
      {
         _unitAttributes.DealDamage(enemy.attackDamage);
         // critical hit 
         if(Random.Range(0, 100) < (enemy.attackCritChance)){
            Debug.Log("enemy hit critical");
         _unitAttributes.DealDamage(enemy.attackDamageCrit);
         }
      }
      // enemy was hit
      if(Random.Range(0, 100) < player.attackHitChance)
      {
         _enemyUnitAttributes.DealDamage(player.attackDamage);
         // critical hit
         if(Random.Range(0, 100) < (player.attackCritChance)){
            _enemyUnitAttributes.DealDamage(player.attackDamageCrit);
            Debug.Log("player hit critical");
         }
      }
      Debug.Log("Player: After Battle");
      Debug.Log(_unitAttributes.GetHealth());
      //Debug.Log("Enemy: After Battle");
      //Debug.Log(enemyUnitAttributes.GetHealth());
      //Debug.Log("Enemy attack chance and crit chance");
      //Debug.Log(enemy.attackHitChance);
      //Debug.Log(enemy.attackCritChance);

      if(_unitAttributes.GetHealth()<=0)
      {
         OnDeath?.Invoke();
      }
      
      if(_enemyUnitAttributes.GetHealth()<=100)
      {
         OnEnemyDeath?.Invoke();
      }
   }


   // Action Event: heals a hurt unit on players turn
   // unit - is the player that will heal a unit
   // hurtUnit - is the unit that needs to be healed
   public void healAlly (GameObject unit, GameObject hurtUnit)
   {
      // get players data
      _unitAttributes = unit.GetComponent<UnitAttributes>();
      // get ally data
      _allyUnitAttributes = hurtUnit.GetComponent<UnitAttributes>();
      // get unit's attributes
      double [] playerAtt = _unitAttributes.GetAttackOneStats();
      double [] allyAtt = _allyUnitAttributes.GetAttackOneStats();

      // dummy variable for healling 
      player.healIncrease = 15; // must change from Gibbys data
      // heal ally
      _allyUnitAttributes.GainHealth(player.healIncrease);
   }


   // units action type: when they do not want to do nothing but move, will suffer terrain damage
   public void doNothingTurn (GameObject unit)
   {
      // temporay terrain damage, must later update with a tile
      int terrain=5;
      //get units data
      _unitAttributes = unit.GetComponent<UnitAttributes>();
      _deathAnimationUnit = unit.GetComponent<DeathAnimation>();
      // units get terrain damage
      if(Random.Range(0, 100) < 100)
      {
         _unitAttributes.DealDamage(terrain);  
      } 
      Debug.Log("Move");
      Debug.Log(_unitAttributes.GetHealth());
      // animate death
      if(_unitAttributes.GetHealth()<=0)
      {
         OnDeath?.Invoke();
      }
   }

   // Caleb use to eliminate a Unit from the game
   // get the current update of any unit health on the game board
   public int getUpdateUnitHealth(GameObject unit)
   {
      // get current unit attributes
      _unitAttributes = unit.GetComponent<UnitAttributes>();
      // Animation Unit Death
      //if(_unitAttributes.GetHealth()<=0)
      //{
      //  OnDeath?.Invoke();
      //}
      return _unitAttributes.GetHealth();
   }

   // do not use
   // used for testing: player health during the event, health of internal data structure
   public int getPlayerEventHealth()
   {
      return(player.health);
   }

   // do not use
   // used for testing: enemy health during the event, health of internal data structure
   public int getEnemyEventHealth()
   {
      return(enemy.health);
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
