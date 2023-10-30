using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    
    protected bool killUnit = false;
    protected bool attackUnit = false;
    protected bool delay = false;




AnimatorStateInfo animStateInfo;
public float NTime;
bool animationFinished;
 
 




    public Animator flame;
    // subscribe to event
    private void OnEnable()
    {
        ActionEventManager.OnDeath +=killAnimation;
        ActionEventManager.OnAttack +=attackAnimation;
    }
    // unsubscribe to an event
    private void OnDisable()
    {
        ActionEventManager.OnDeath -=killAnimation;
        ActionEventManager.OnAttack -=attackAnimation;
    }

    void Awake()
    {
        flame = gameObject.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //animator.Play("deathfire");
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




    public virtual void killAnimation(){
        flame.SetBool("die", true);
    }

    public virtual void attackAnimation(){
        flame.SetBool("attack", true);
        attackUnit=true;
    }
}
