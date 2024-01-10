using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject self;
    public String itemName;
    public float itemValue;
    
    private void Start() {
        self = gameObject;
    }
}
