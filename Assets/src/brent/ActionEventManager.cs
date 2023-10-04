using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class actionEvent
{
   // event running status
   private bool status;
   // the type of event 
   private int eventType;
   //private int playerHealth;
   private playerAttributes player = new playerAttributes();
   private enemyAttributes  enemy = new enemyAttributes();

   //player attributes
   private class playerAttributes{
      public int health;
      public int attack;
      public playerAttributes(){
         health = 100;
         attack = 50;
      }
      }
   // enemy attributes
   private class enemyAttributes{
      public int health;
      public int attack;
      public enemyAttributes(){
         health = 100;
         attack = 50;
      }
   }

   // Purpose-  will deliver damage of a player and enemy battle
   // arguments- playerHP - health of the player
   //            enemyHP - health of the enemy
   // will update the health with the corresponding damage yielde from a battle
   // basic weapon attack of 20 hp for a hit at a random attack of 60 perecent accuracy

   public void attackBattle (ref int playerHP, ref int enemyHP){
   // damage from attack
   int playerAttackDamage=20;
   int enemyAttackDamge=20;

   // player was hit
   if(Random.Range(0, 100) < 60){
         playerHP=playerHP-enemyAttackDamge;
   }
      // enemy was hit
   if(Random.Range(0, 100) < 60){
         enemyHP=enemyHP-playerAttackDamage;
   }
}
   
   // initialize 
   public actionEvent(){
    //status = true;
    //eventType = 0;
   }

   public int getPlayerHealth(){
      return player.health;
   }

   public int getEnemyHealth(){
      return enemy.health;
   }
}
