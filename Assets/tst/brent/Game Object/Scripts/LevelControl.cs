using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControl : MonoBehaviour
{
   void OnTriggerEnter2D(Collider2D other){
    if(other.CompareTag("ball1"))
    {
        SceneManager.LoadScene(2);
    }
    if(other.CompareTag("ball10"))
    {
        SceneManager.LoadScene(3);
    }
    if(other.CompareTag("ball100"))
    {
        SceneManager.LoadScene(4);
    }
    if(other.CompareTag("ball1000"))
    {
        SceneManager.LoadScene(5);
    }
    if(other.CompareTag("ball10000"))
    {
        SceneManager.LoadScene(6);
    }
   }
}
