using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorAnimation : MonoBehaviour
{
    private Animation animationComponent;

    private void Start()
    {
        // Get the Animation component
        animationComponent = GetComponent<Animation>();

        // Play the animation
        animationComponent.Play("Cursor");
    }

    private void Update()
    {
        if (!(animationComponent.IsPlaying("Cursor")))
        {
            animationComponent.Play("Cursor");
        }
    }
}
