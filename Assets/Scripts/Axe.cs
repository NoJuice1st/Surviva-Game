using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public Tool tool;
    public int damage;

    public void useTool(RaycastHit hit)
    {
        print("Tool used");
        if (hit.transform.gameObject.CompareTag("Tree"))
        {
            Destructable tree = hit.transform.gameObject.GetComponent<Destructable>();
            tree.TakeDamage(damage);
        }
        Swing();
    }

    public void Swing()
    {
        // play swing Animation
        print("swing");
    }
}
