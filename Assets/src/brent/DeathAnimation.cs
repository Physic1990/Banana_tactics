using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    // signal unit has died
    protected bool killUnit = false;
    // signal unit has attacked
    protected bool attackUnit = false;
    AnimatorStateInfo animStateInfo;
    public float NTime;
    bool animationFinished;
    public Animator flame;
    // subscribe to event
    private void OnEnable()
    {
        ActionEventManager.onDeath +=killAnimation;
        ActionEventManager.onAttack +=attackAnimation;
    }
    // unsubscribe to an event
    private void OnDisable()
    {
        ActionEventManager.onDeath -= killAnimation;
        ActionEventManager.onAttack -= attackAnimation;
    }

    void Awake()
    {
        flame = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(flame.GetCurrentAnimatorStateInfo(0).IsName("attackRoy") && 
        flame.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            flame.SetBool("attack", false);
            attackUnit=false;
        }
    }

    public virtual void killAnimation()
    {
        flame.SetBool("die", true);
    }

    public virtual void attackAnimation()
    {
        flame.SetBool("attack", true);
        attackUnit=true;
    }
}
