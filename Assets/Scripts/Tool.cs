using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    public int durability;
    public Inventory inv;

    public void DamageTool()
    {
        durability--;
        if (durability <= 0)
        { 
            Break();
        }
    }

    public void Break()
    {
        inv.RemoveItem(gameObject);
        Destroy(gameObject);
    }
}
