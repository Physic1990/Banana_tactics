using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathAnimation : DeathAnimation
{
    // subscribe to an event
    private void OnEnable()
    {
        ActionEventManager.onEnemyDeath += killAnimation;
    }
    // unsubscribe to an event
    private void OnDisable()
    {
        ActionEventManager.onEnemyDeath -=killAnimation;
    }
    // signal enemy's death
    public bool killEnemyUnit = false;
    // death animation
    public DeathAnimation deathAnimation;
    public override void killAnimation()
    {
        killEnemyUnit=true;
    }
    // waiting for death signal
    public override void  Update()
    {
        // enemy died
        if(killEnemyUnit == true)
        {
            // play animation
            eventAnimation.enabled = true;
        }
    }
}
