using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    
    bool killUnit = false;

    public Animator flame;
    // subscribe to event
    private void OnEnable()
    {
        ActionEventManager.OnDeath +=killAnimation;
    }
    // unsubscribe to an event
    private void OnDisable()
    {
        ActionEventManager.OnDeath -=killAnimation;
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
    void Update()
    {
        if(killUnit == true){
            flame.enabled = true;
        }
    }



    public void killAnimation(){
        killUnit=true;
    }
}
