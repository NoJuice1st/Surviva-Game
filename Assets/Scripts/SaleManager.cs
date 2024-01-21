using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaleManager : MonoBehaviour
{
    public float currency;
    public TMP_Text curText;

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
        curText.text = "Money: " + currency.ToString();
    }
}
