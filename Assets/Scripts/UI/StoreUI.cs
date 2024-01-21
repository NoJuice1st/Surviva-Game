using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoreUI : MonoBehaviour
{
    public GameObject storeGui;
    public SaleManager sm;
    public int price;


    private void Update() {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(storeGui.activeSelf)
            {
                Cursor.lockState = CursorLockMode.Locked;
                storeGui.SetActive(false);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                storeGui.SetActive(true);
            }
        }
    }

    public void BuyPickaxe(GameObject pickaxe)
    {

        if (sm.currency >= price && !GameObject.Find(pickaxe.name+"(Clone)"))
        {
            print(pickaxe.name+"(clone)");
            Instantiate(pickaxe, new Vector3(125, 4, 125), new Quaternion());
            sm.currency -= price;
        }
    }

    public void BuyAxe(GameObject axe)
    {
        if (sm.currency >= price && !GameObject.Find(axe.name+"(Clone)"))
        {
            Instantiate(axe, new Vector3(125, 4, 125), new Quaternion());
            sm.currency -= price;
        }
    }

    public void SetPrice(int cash)
    {
        price = cash;
    }
}
