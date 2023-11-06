using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    // signal unit has died
   // protected bool killUnit = false;
    // signal unit has attacked
    //protected bool attackUnit = false;
    AnimatorStateInfo animStateInfo;
    public float NTime;
    bool animationFinished;
    public Animator eventAnimation;
    // subscribe to event
    private void OnEnable()
    {
        ActionEventManager.onDeath +=killAnimation;
        ActionEventManager.onAttack +=attackAnimation;
        ActionEventManager.onHealth +=healthAnimation;
        ActionEventManager.onEnemyDeath += enemyKillAnimation;
    }
    // unsubscribe to an event
    private void OnDisable()
    {
        ActionEventManager.onDeath -= killAnimation;
        ActionEventManager.onAttack -= attackAnimation;
        ActionEventManager.onHealth -= healthAnimation;
        ActionEventManager.onEnemyDeath -= enemyKillAnimation;
    }

    void Awake()
    {
        eventAnimation = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    public virtual void Update()
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
            eventAnimation.SetBool("powerUp", false);
            //attackUnit=false;
       }
    }

    public virtual void killAnimation()
    {
        eventAnimation.SetBool("die", true);
    }

    public virtual void enemyKillAnimation()
    {
        eventAnimation.SetBool("deathAsEnemy", true);
    }


    public virtual void attackAnimation()
    {
        eventAnimation.SetBool("attack", true);
    }

    public virtual void healthAnimation()
    {
        eventAnimation.SetBool("powerUp", true);
    }
}
