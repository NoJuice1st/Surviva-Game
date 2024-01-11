using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject self;
    public string itemName;
    public float itemValue;
    
    private void Start() {
        self = gameObject;
    }
}
