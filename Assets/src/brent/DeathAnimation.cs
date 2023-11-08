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
        ActionEventManager.onP1 += P1;
        ActionEventManager.onP2 += P2;
        ActionEventManager.onP3 += P3;
        ActionEventManager.onP4 += P4;
        ActionEventManager.onP5 += P5;
        ActionEventManager.onE1 += E1;
        ActionEventManager.onE2 += E2;
        ActionEventManager.onE3 += E3;
        ActionEventManager.onE4 += E4;
        ActionEventManager.onE5 += E5;
    }
    // unsubscribe to an event
    private void OnDisable()
    {
        ActionEventManager.onDeath -= killAnimation;
        ActionEventManager.onAttack -= attackAnimation;
        ActionEventManager.onHealth -= healthAnimation;
        ActionEventManager.onP1 -= P1;
        ActionEventManager.onP2 -= P2;
        ActionEventManager.onP3 -= P3;
        ActionEventManager.onP4 -= P4;
        ActionEventManager.onP5 -= P5;
        ActionEventManager.onE1 -= E1;
        ActionEventManager.onE2 -= E2;
        ActionEventManager.onE3 -= E3;
        ActionEventManager.onE4 -= E4;
        ActionEventManager.onE5 -= E5;
    }

    bool key = false;
    void P1()
    {
        key = Equals(gameObject.name, "player warrior 1");
        Debug.Log(key);
    }

    void P2()
    {
        key = Equals(gameObject.name, "player gunslinger 2");
    }

    void P3()
    {
        key = Equals(gameObject.name, "player rogue 3");
    }

    void P4()
    {
        key = Equals(gameObject.name, "player hero 4");
    }

    void P5()
    {
        key = Equals(gameObject.name, "player lancer 5");
    }

    void E1()
    {
        key = Equals(gameObject.name, "enemy warrior 10");
    }

    void E2()
    {
        key = Equals(gameObject.name, "enemy warrior 11");
    }

    void E3()
    {
        key = Equals(gameObject.name, "enemy warrior 12");
    }

    void E4()
    {
        key = Equals(gameObject.name, "enemy warrior 13");
    }

    void E5()
    {
      key = Equals(gameObject.name, "enemy warrior 14");
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
        if(key)
        {
            eventAnimation.SetBool("die", true);
        }

        key = false;
    }


    public virtual void attackAnimation()
    {
        if(key)
        {
            eventAnimation.SetBool("attack", true);
        }

        key = false;
    }

    public virtual void healthAnimation()
    {
        if(key)
        {
            eventAnimation.SetBool("powerUp", true);
        }

        key = false;
    }
}
