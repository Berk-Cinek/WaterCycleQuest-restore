using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public Transform pfHealthBar;  // Reference to the HealthBar prefab
    public Transform player;       // Reference to the player object

    private void Start()
    {
        // Initialize max health and current health
        int maxHealth = 100;
        int currentHealth = maxHealth;

        // Instantiate the health bar above the player
        Transform healthBarTransform = Instantiate(pfHealthBar, new Vector3(0, 10, 0), Quaternion.identity);
        healthBarTransform.SetParent(player);
        healthBarTransform.localPosition = new Vector3(0, 1, 0);

        // Get the HealthBar component and set it up with max and current health
        HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
        if (healthBar != null)
        {
            healthBar.Setup(maxHealth, currentHealth);
        }

        // Debug log the initial health
        Debug.Log("Health: " + currentHealth);
    }
}

