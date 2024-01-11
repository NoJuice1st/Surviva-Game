using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public Inventory inv;
    //public GameObject[] Slots = new GameObject[4];
    public TMP_Text text;

    // Update is called once per frame
    void Update()
    {
        string invItems = "";
        foreach (var item in inv.GetAllItems())
        {
            invItems += item.GetComponent<Item>().itemName + "\n";
        }
        text.text = "Inventory: \n" + invItems;
    }
}
