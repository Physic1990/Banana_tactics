using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathAnimation : DeathAnimation
{

    public override void killAnimation()
    {
        eventAnimation.SetBool("enemy1die", true);
    }

    // Update is called once per frame
    public override void Update()
    {
        if(eventAnimation.GetCurrentAnimatorStateInfo(0).IsName("attackRoy") && 
        eventAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            eventAnimation.SetBool("attack", false);
            //attackUnit=false;
        }
        if(eventAnimation.GetCurrentAnimatorStateInfo(0).IsName("addHealth") && 
           eventAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
       {
            eventAnimation.SetBool("powerUpEnemy1", false);
            //attackUnit=false;
       }
    }

    // waiting for death signal
    public override void enemyKillAnimation()
    {
        eventAnimation.SetBool("enemy1die", true);
    }

    public override void attackAnimation()
    {
        eventAnimation.SetBool("attackEnemy1", true);
    }

    public override void healthAnimation()
    {
        eventAnimation.SetBool("powerUpEnemy1", true);
    }
}
