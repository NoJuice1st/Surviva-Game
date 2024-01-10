using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaleManager : MonoBehaviour
{
    public float currency;

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.TryGetComponent<Item>(out Item item))
        {
            currency += item.itemValue;    
        }

        Destroy(other.gameObject);
    }

    private void Update() 
    {
        print(currency);
    }
}
