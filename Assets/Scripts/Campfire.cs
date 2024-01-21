using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Campfire : MonoBehaviour
{
    public HealthBarUI healthBar;
    public Inventory inv;

    public float maxHealth;
    public float currentHealth;
    public float damageMultiplier;

    public float lightMax;

    public Light lit;
    public GameObject safeArea;

    void Start()
    {
        currentHealth = maxHealth;

        healthBar.SetMaxHealth(maxHealth);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.TryGetComponent<Item>(out Item item))
        {
            if (item.itemName.Contains("Log"))
            {
                currentHealth += item.itemValue;

                if(currentHealth > maxHealth)
                {
                    currentHealth = maxHealth;
                }
                
                Destroy(other.gameObject);
                inv.RemoveItem(other.gameObject);
            }
        }   
    }

    void Update()
    {   
        float currentRange = lightMax * (currentHealth / maxHealth);
        lit.range = currentRange;

        safeArea.transform.localScale = new Vector3(currentRange, currentRange, currentRange);

        if (currentHealth > 0)
        {
            currentHealth -= Time.deltaTime * damageMultiplier;

            healthBar.SetHealth(currentHealth);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
