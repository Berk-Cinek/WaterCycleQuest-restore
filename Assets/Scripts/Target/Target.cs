using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private int health = 100; // Default health of the target

    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        Debug.Log($"Target {gameObject.name} hit! Remaining health: {health}");

        if (health <= 0)
        {
            Destroy(gameObject);
            Debug.Log($"Target {gameObject.name} destroyed!");
        }
    }
}
