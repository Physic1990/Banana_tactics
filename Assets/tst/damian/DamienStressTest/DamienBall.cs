using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamienBall : MonoBehaviour
{


    [SerializeField] float speed;
    int direction;

    // Start is called before the first frame update
    void Start()
    {
        direction = 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, 0, 0);

        if (transform.position.x < -7)
        {
            direction = -direction;
        }
        else if (transform.position.x > 7)
        {
            direction = -direction;
        }

    }
}
