using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public float pickUpDistance = 10f;
    public float interactDistance = 5f;
    public Transform hand;
    public Inventory inv;
    public int selecItem = 0;
    Vector3 forward;

    private void Update() {
        forward = transform.forward;
        Drop();
        Interact();
        PickUpp();

        //SwitchItem
        if (Input.mouseScrollDelta.y != 0)
        {
            selecItem += (int)Math.Round(Input.mouseScrollDelta.y);
            UpdateHeldItem();
            //print(selecItem);
        }
    }

    /*void FixedUpdate()
    {
        //PickUpp();
    }*/
    private void PickUpp()
    {
        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.E) && Physics.Raycast(transform.position, forward, out hit, pickUpDistance))
        {
            if (hit.transform.CompareTag("Pickable") || hit.transform.CompareTag("Tool"))
            {
                GameObject item = hit.transform.gameObject;
                // PickUp the Item
                if (!inv.MaxItems())
                {   // Add Item to Inventory
                    inv.AddItem(item, selecItem);
                    UpdateHeldItem();
                }
                else
                {   //swap Item in Hand with other item
                    GameObject selItem = inv.GetItem(selecItem);
                    inv.RemoveItem(selItem);

                    if(selItem.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    rb.useGravity = true;
                    rb.detectCollisions = true;
                    rb.isKinematic = false;

                    selItem.transform.position = item.transform.position;
                    selItem.transform.rotation = item.transform.rotation;

                    selItem.transform.SetParent(null);
                }
                    inv.AddItem(item, selecItem);
                    UpdateHeldItem();
                }
            }
        }
    }

    private void Drop()
    {
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

    private void UpdateHeldItem()
    {
        int maxItems = inv.GetAllItems().Count;
        
        if (selecItem < 0)
        {
            if(maxItems != 0)
                selecItem = maxItems - 1;
            else
                selecItem = 0;
        }
        else if (selecItem >= maxItems)
        {
            selecItem = 0;
        }

        //print(selecItem);
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
            item.transform.rotation = hand.transform.rotation;
            item.transform.position = hand.transform.position;
        }
    }

    private void Interact()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !inv.isEmpty())
        {
            GameObject item = inv.GetItem(selecItem);
            if (item.CompareTag("Tool"))
            {   
                //get ToolScript
                if(item.TryGetComponent<Axe>(out Axe axe))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, forward, out hit, interactDistance))
                    {
                        if (hit.transform.gameObject.CompareTag("Tree"))
                        {
                            Destructable tree = hit.transform.gameObject.GetComponent<Destructable>();
                            tree.TakeDamage();
                            axe.DamageTool();
                        }
                    }
                    axe.Swing();
                }
                

                //Used Tool
                print("used tool");
            }
        }
    }

}
