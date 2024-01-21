using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : MonoBehaviour
{
    public Tool tool;
    public int damage;

    public void useTool(RaycastHit hit)
    {
        //print("Tool used");
        if (hit.transform.gameObject.CompareTag("Rock"))
        {
            Destructable rock = hit.transform.gameObject.GetComponent<Destructable>();
            rock.TakeDamage(damage);
        }
    }
}
