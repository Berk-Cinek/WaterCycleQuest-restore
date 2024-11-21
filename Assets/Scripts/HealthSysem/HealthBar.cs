using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private int maxHealth;
    private int currentHealth;
    private Transform bar;
    [SerializeField] private Transform barTransform; // Reference to the health bar object (the fill part)
    

    private void Awake()
    {
        bar = transform.Find("Bar");

    }
    public void Setup(int maxHealth, int currentHealth)
    {
        this.maxHealth = maxHealth;
        this.currentHealth = currentHealth;

        // Update the health bar UI
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (barTransform != null)
        {
            // Update the scale of the health bar fill based on current health percentage
            float healthPercent = (float)currentHealth / maxHealth;
            barTransform.localScale = new Vector3(healthPercent, 1, 1);
        }
    }

    // Call this method to update the health
    public void UpdateHealth(int newHealth)
    {
        currentHealth = newHealth;
        UpdateHealthBar();
    }
    public void SetSize(float sizeNormalized)
    {
        bar.localScale = new Vector3(sizeNormalized, 1f);

    }


}
