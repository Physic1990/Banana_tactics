using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball1 : MonoBehaviour
{
    public float speed=3;
    int numberDimensions=1;
    int health=100;
    Rigidbody2D rb;
    Vector2 direction;
    
    int BattleCalculation(int dim, int hp){
        
        for(int i = 0; i<10; i++){
            hp=hp*100;
            hp=hp/100;
        }
        dim--;
        if(dim>0){
            hp=hp+BattleCalculation(dim, hp);
        }
        return(hp);
    }

    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       direction=Vector2.left;
       rb.velocity = direction * speed;
    }

    void Update()
    {
        BattleCalculation(numberDimensions, health);
    }
}