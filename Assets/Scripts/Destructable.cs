using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public bool dropLoot = false;
    public bool dropOnHit = false;
    public bool randomiseLootAmt = false;

    public Vector2 randomLootRange;

    public int health = 1;
    public int lootAmount;
    public int onHitLootAmt;

    public GameObject lootDrop;

    public void TakeDamage(int dmg = 1)
    {
        health -= dmg;
        if(health <= 0)
        {
            if(dropLoot)
            {
                DropLoot(lootAmount);
            }
            Destroy(gameObject);
            return;
        }

        if (dropOnHit)
        {
            DropLoot(onHitLootAmt);
        }
    }

    private void DropLoot(int loot)
    {
        if (randomiseLootAmt)
        {
            loot = Random.Range((int)randomLootRange.x, (int)randomLootRange.y + 1);
        }
        for (int i = 1; i < loot + 1; i++)
        {
            Instantiate(lootDrop, gameObject.transform.position + new Vector3(0, lootDrop.transform.localScale.y * i + 1f, 0), gameObject.transform.rotation);
        }
    }
}
