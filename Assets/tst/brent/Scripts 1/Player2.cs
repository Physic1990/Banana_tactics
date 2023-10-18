using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : Paddle
{
    private Vector2 direction;

    private void Update()
    {

        if (Input.GetKey(KeyCode.P))
        {
            direction = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.L))
        {
            direction = Vector2.down;
        }
        else
        {
            direction = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (direction.sqrMagnitude != 0)
        {
            _rigidbody.AddForce(direction * this.speed);
        }
    }
}

