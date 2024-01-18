using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactions : MonoBehaviour
{
    public float pickUpDistance = 10f;
    public float interactDistance = 1f;
    public int selecItem = 0;

    public Animator animator;
    public Transform hand;
    public Inventory inv;
    public GameObject particleSys;

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
        }

    }
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

                        // switch spots
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
        if (Input.GetKeyDown(KeyCode.Q) && !inv.isEmpty())
        {
            GameObject item = inv.GetItem(selecItem);

            // if Tool, Remove from Inventory, make sure the tool belongs to no one
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

            // If has a rb, launch
            if (item.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.useGravity = true;
                rb.detectCollisions = true;
                rb.isKinematic = false;
                rb.AddForce(forward.normalized * 10, ForceMode.Impulse);
            }
            
        }
    }

    private void UpdateHeldItem()
    {
        int maxItems = inv.GetAllItems().Count;

        // Make sure selected Item doesn't go out of bounds
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

        // If there are still Items, switch to new Item
        if (maxItems > 0)
        {
            // Make sure Items aren't active
            foreach (var other in inv.GetAllItems())
            {
                other.SetActive(false);
            }

            GameObject item = inv.GetItem(selecItem);

            // Switch Item to hand
            item.SetActive(true);
            
            if (item.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.useGravity = false;
                rb.detectCollisions = false;
                rb.isKinematic = true;
            }

            item.transform.SetParent(hand.transform, true);

            item.transform.rotation = new Quaternion();
            
            item.transform.position = hand.transform.position;

            if (item.TryGetComponent<Tool>(out Tool tool))
            {
                //print("picked up");
                item.transform.localRotation = Quaternion.Euler(0, 100, 0);
                if (item.TryGetComponent<Pickaxe>(out Pickaxe pickaxe))
                {
                    item.transform.position += hand.transform.up * 0.5f;
                }
            }
        }
    }

    private void Interact()
    {
        if (Input.GetKey(KeyCode.Mouse0) && !inv.isEmpty())
        {
            GameObject item = inv.GetItem(selecItem);
            //get Tool Script
            //Detect if something is being hit
            RaycastHit hit;
            if(animator.GetCurrentAnimatorStateInfo(0).length < animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                if (item.CompareTag("Tool") && item.TryGetComponent<Tool>(out Tool tool) && Physics.Raycast(transform.position, forward, out hit, interactDistance))
                {
                    
                    //Get which tool hit
                    if (item.TryGetComponent<Axe>(out Axe axe))
                    {
                        axe.useTool(hit);
                        animator.Play("SwingSideways");
                    }
                    else if (item.TryGetComponent<Pickaxe>(out Pickaxe pickaxe))
                    {
                        pickaxe.useTool(hit);
                        animator.Play("SwingDown");
                    }

                    HitParticles(hit);

                    tool.DamageTool();
                }
                else if(item.CompareTag("Tool") && item.TryGetComponent<Tool>(out Tool tools))
                {
                    //Get which tool to animate if not hit anything
                    if (item.TryGetComponent<Axe>(out Axe axe))
                    {
                        animator.Play("SwingSideways");
                    }
                    else if (item.TryGetComponent<Pickaxe>(out Pickaxe pickaxe))
                    {
                        print(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                        animator.Play("SwingDown");
                    }
                }
            }
        }
    }

    private void HitParticles(RaycastHit hit)
    {
        
        GameObject particles = Instantiate(particleSys, hit.point, hit.transform.rotation, null);
        particles.GetComponent<Renderer>().material.color = hit.transform.GetComponent<Renderer>().material.color;

        ParticleSystem ps = particles.GetComponent<ParticleSystem>();
        ps.Play();
    }
}
