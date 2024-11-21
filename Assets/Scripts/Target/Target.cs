using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100; // Max health of the target
    private int currentHealth;

    [SerializeField] private HealthBar healthBar; // Reference to the health bar
    [SerializeField] private GameObject hitEffect; // Optional hit effect prefab
    [SerializeField] private AudioSource audioSource; // Optional audio source for sound effects
    [SerializeField] private AudioClip hitSound; // Sound played when target is hit

    private void Start()
    {
        // Initialize the target's health
        currentHealth = maxHealth;

        // Setup health bar with the target's max and current health
        if (healthBar != null)
        {
            healthBar.Setup(maxHealth, currentHealth);
        }
    }

    // This method is called when the target takes damage
    public void Damage(int damageAmount)
    {
        // Reduce health
        currentHealth -= damageAmount;
        Debug.Log($"Target {gameObject.name} hit! Remaining health: {currentHealth}");

        // Update health bar
        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth);
        }

        // Play hit effects
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        // Check if health is zero or below
        if (currentHealth <= 0)
        {
            HandleDestruction();
        }
    }

    // This method handles target destruction when health reaches zero
    private void HandleDestruction()
    {
        // Handle destruction logic here (e.g., animation, sound, etc.)
        Destroy(gameObject);
        Debug.Log($"Target {gameObject.name} destroyed!");
    }
}
