using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionEventManager : MonoBehaviour
{
   UnitAttributes unitAttributes;
   //public GameObject attckingUnit;

   void Awake()
   {
      //unitAttributes = unit.GetComponent<UnitAttributes>();
   }

   // event running status
   private bool status;
   // the type of event 
   private int eventType;
   //private int playerHealth;
   private int terrain;
   private playerAttributes player = new playerAttributes();
   private enemyAttributes  enemy = new enemyAttributes();

   //player attributes
   private class playerAttributes{
      public int health;
      public int attackDamage;
      public double attackRange;
      public double attackCritChance;
      public double attackHitChance;
      public double attackDamageCrit;
      
      public playerAttributes(){
         health = 100;
         attackDamage = 50;
      }
   }

   // enemy attributes
   private class enemyAttributes{
      public int health;
      public int attackDamage;
      public double attackRange;
      public double attackCritChance;
      public double attackHitChance;
      public double attackDamageCrit;

      public enemyAttributes(){
         health = 100;
         attackDamage = 50;
      }
   }

   // Purpose-  will deliver damage of a player and enemy battle
   // arguments- playerHP - health of the player
   //            enemyHP - health of the enemy
   // will update the health with the corresponding damage yielde from a battle
   // basic weapon attack of 20 hp for a hit at a random attack of 60 perecent accuracy

   public void attackBattle (){
   // player was hit
   if(Random.Range(0, 100) < 60){
         player.health=player.health-enemy.attackDamage;
   }
      // enemy was hit
   if(Random.Range(0, 100) < 60){
         enemy.health=enemy.health-player.attackDamage;
   }
   }

   // Purpose-  will deliver damage to a player on movement by terrain
   // will update the health with the corresponding damage yielded from terrain

   public void doNothingTurn (GameObject unit){
        // player was hit
        Debug.Log("No Action taken");
      /*if(Random.Range(0, 100) < 50){
         player.health=player.health-terrain;
      } */
   }
   
   // initialize 
   /*public actionEvent(){
      terrain = 50;
   } */

   public int getUpdatePlayerHealth(){
      return player.health;
   }

   public int getUpdateEnemyHealth(){
      return enemy.health;
   }
}

