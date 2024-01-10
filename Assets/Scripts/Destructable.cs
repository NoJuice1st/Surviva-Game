using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public int health;

    public void TakeDamage()
    {
        health--;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
