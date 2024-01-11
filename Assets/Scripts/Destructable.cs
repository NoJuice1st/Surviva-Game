using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public bool dropLoot;
    public bool randomiseLootAmt;

    public Vector2 randomLootRange;

    public int health;
    public int lootAmount;

    public GameObject lootDrop;

    public void TakeDamage(int dmg = 1)
    {
        health -= dmg;
        if(health <= 0)
        {
            if(dropLoot)
            {
                if(randomiseLootAmt)
                {
                    lootAmount = Random.Range((int)randomLootRange.x, (int)randomLootRange.y);
                }
                for (int i = 0; i < lootAmount; i++)
                {
                    Instantiate(lootDrop, gameObject.transform.position, gameObject.transform.rotation);
                }
            }
            Destroy(gameObject);
        }
    }
}
