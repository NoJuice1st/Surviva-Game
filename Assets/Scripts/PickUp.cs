using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public float pickUpDistance = 10f;
    public Transform hand;
    public Inventory inv;
    public int selecItem = 0;

    private void Update() {
        Drop();
        PickUpp();

        if (Input.mouseScrollDelta.y != 0)
        {
            selecItem += (int)Math.Round(Input.mouseScrollDelta.y);
            UpdateHeldItem();
            print(selecItem);
        }
    }

    /*void FixedUpdate()
    {
        //PickUpp();
    }*/

    private void PickUpp()
    {
        Vector3 forward = transform.forward;

        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.Mouse0) && Physics.Raycast(transform.position, forward, out hit, pickUpDistance))
        {
            if (hit.transform.CompareTag("Pickable"))
            {
                GameObject item = hit.transform.gameObject;
                // PickUp the Item
                if (!inv.MaxItems())
                {   // Add Item to Inventory
                    inv.AddItem(item, selecItem);
                    UpdateHeldItem();
                }
                else
                {   //swap Item in Hand

                }
            }
        }
    }

    private void Drop()
    {
        Vector3 forward = transform.forward;
        bool isCooldown = false;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!inv.isEmpty() && !isCooldown)
            {
                isCooldown = true;
                GameObject item = inv.GetItem(selecItem);
                inv.RemoveItem(item);
                UpdateHeldItem();

                item.SetActive(true);
                if(item.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    rb.useGravity = true;
                    rb.detectCollisions = true;
                    rb.isKinematic = false;
                    rb.AddForce(forward.normalized * 10, ForceMode.Impulse);
                    item.transform.SetParent(null);
                }

                isCooldown = false;
            }
        }
    }

    void UpdateHeldItem()
    {
        int maxItems = inv.GetAllItems().Count;
        
        if (selecItem < 0)
        {
            selecItem = maxItems - 1;
        }
        else if (selecItem >= maxItems)
        {
            selecItem = 0;
        }

        print(selecItem);
        if (maxItems > 0)
        {
            foreach (var other in inv.GetAllItems())
            {
                other.SetActive(false);
            }

            GameObject item = inv.GetItem(selecItem);

            item.SetActive(true);
            if (item.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.useGravity = false;
                rb.detectCollisions = false;
                rb.isKinematic = true;
            }
            item.transform.SetParent(hand.transform, true);
            item.transform.position = hand.transform.position;
        }

    }


}
