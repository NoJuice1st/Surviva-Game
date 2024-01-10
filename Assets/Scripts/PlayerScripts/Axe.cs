using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public int durability;

    public void Swing()
    {
        // play swing Animation
        print("swing");
    }

    public void DamageTool()
    {
        durability--;
    }
}
