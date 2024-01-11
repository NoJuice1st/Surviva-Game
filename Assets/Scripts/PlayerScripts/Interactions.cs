using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactions : MonoBehaviour
{
    public float pickUpDistance = 10f;
    public float interactDistance = 1f;
    public int selecItem = 0;

    public Transform hand;
    public Inventory inv;
    Vector3 forward;

    private void Update()
    {
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

                // if Tool Set Inventory, make sure the tool belongs to owner
                if (item.TryGetComponent<Tool>(out Tool tool))
                {
                    if (tool.inv == null)
                    {
                        tool.inv = inv;
                    }
                    else
                        return;
                }

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

                    if (selItem.TryGetComponent<Rigidbody>(out Rigidbody rb))
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!inv.isEmpty())
            {
                GameObject item = inv.GetItem(selecItem);

                // if Tool Remove Inventory, make sure the tool belongs to no one
                if (item.TryGetComponent<Tool>(out Tool tool))
                {
                    if (tool.inv != null)
                    {
                        tool.inv = null;
                    }
                }

                inv.RemoveItem(item);
                UpdateHeldItem();

                item.SetActive(true);
                item.transform.SetParent(null);
                if (item.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    rb.useGravity = true;
                    rb.detectCollisions = true;
                    rb.isKinematic = false;
                    rb.AddForce(forward.normalized * 10, ForceMode.Impulse);
                }
            }
        }
    }

    private void UpdateHeldItem()
    {
        int maxItems = inv.GetAllItems().Count;

        if (selecItem < 0)
        {
            if (maxItems != 0)
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
            //get ToolScript
            //Detect if something is being hit
            RaycastHit hit;
            if (item.CompareTag("Tool") && item.TryGetComponent<Tool>(out Tool tool) && Physics.Raycast(transform.position, forward, out hit, interactDistance))
            {
                //Get which tool hit
                if (item.TryGetComponent<Axe>(out Axe axe))
                {
                    axe.useTool(hit);
                }

                tool.DamageTool();

                //Used Tool
                print("used tool");
            }
        }
    }
}
