using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{
    public float pickUpDistance = 10f;

    public Transform hand;
    public List<GameObject> Items;
    public int maxItems = 4;

    private void Update() {
        Drop();
    }

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
                        rb.isKinematic = true;
                    }
                    Items.Insert(0, item);
                    item.transform.SetParent(hand.transform, true);
                    item.transform.position = hand.transform.position;

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

        if (Input.GetKey(KeyCode.Q))
        {
            if (Items.Count > 0)
            {
                print(Items.Count);
                GameObject item = Items[0];
                Items.RemoveAt(0);
                if (item.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    rb.useGravity = true;
                    rb.detectCollisions = true;
                    rb.isKinematic = false;
                    rb.AddForce(forward * 100);
                }
                item.transform.SetParent(null);
            }
        }
    }
}
