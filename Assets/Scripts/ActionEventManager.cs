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

}
