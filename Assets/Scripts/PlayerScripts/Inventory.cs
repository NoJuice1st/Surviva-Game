using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> items = new List<GameObject>();
    public int maxItems = 4;

        // Get

    public List<GameObject> GetAllItems()
    {
        return items;
    }

    public GameObject GetItem(int selectedItem)
    {
        if(items.Count > 0)
        {
            return items[selectedItem];
        }
        return null;
    }

        // Add / Remove

    public void AddItem(GameObject item, int selectedItem = 0)
    {
        items.Insert(selectedItem, item);
    }

    public void RemoveItem(GameObject item)
    {
        if(items.Count == 1)
        {
          items.Clear();  
        }
        else
        {
            items.Remove(item);
        }
    }

        // full / empty

    public bool MaxItems()
    {
        if (items.Count >= maxItems)
            return true;
        else
            return false;
    }

    public bool isEmpty()
    {
        return items.Count == 0;
    }
}
