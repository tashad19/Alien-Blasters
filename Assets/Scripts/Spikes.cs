using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spikes : MonoBehaviour
{
    // collision2d (used in OnCollisionEnter2D method) -> it's a class in unity. used to get more info about the collision
    //  vs 
    // collider2d (used in OnTriggerEnter2D method) -> simply the collider2d component of the gameobject which collided

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
            Debug.Log("spike touched!");
            SceneManager.LoadScene(0);

    }
}
