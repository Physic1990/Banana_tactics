using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathAnimation : DeathAnimation
{
    private void OnEnable()
    {
        ActionEventManager.OnEnemyDeath +=killAnimation;
    }
    // unsubscribe to an event
    private void OnDisable()
    {
        ActionEventManager.OnEnemyDeath -=killAnimation;
    }
    public bool killEnemyUnit = false;
    public DeathAnimation deathAnimation;
    public override void killAnimation(){
        
        killEnemyUnit=true;
    }
    public override void  Update()
    {
        if(killEnemyUnit == true){
            flame.enabled = true;
        }
    }
}
