using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{
    public float pickUpDistance = 10f;

    public Transform hand;
    public List<GameObject> Items;
    public int maxItems = 4;

    void FixedUpdate()
    {
        PickUp();
    }

    private void PickUp()
    {
        Vector3 forward = transform.forward;

        RaycastHit hit;
        if (Input.GetKey(KeyCode.Mouse0) && Physics.Raycast(transform.position, forward, out hit, pickUpDistance))
        {
            if (hit.transform.CompareTag("Pickable"))
            {
                GameObject item = hit.transform.gameObject;
                // PickUp the Item
                if (Items.Count < maxItems)
                {   // Add Item to Inventory
                    if (item.TryGetComponent<Rigidbody>(out Rigidbody rb))
                    {
                        rb.useGravity = false;
                        rb.detectCollisions = false;
                    }
                    Items.Add(item);
                    item.transform.SetParent(hand.transform, true);
                    item.transform.position = hand.transform.position;

                }
                else
                {   //swap Item in Hand

                }


                print("Item");
            }

        }
        //Drop
        if (Input.GetKey(KeyCode.Q) && Physics.Raycast(transform.position, forward, out hit, pickUpDistance))
        {
            if (Items.Count > 0)
            {
                GameObject item = Items[0];
                item.transform.SetParent(null);
                Items.RemoveAt(0);
            }
        }
    }
}
