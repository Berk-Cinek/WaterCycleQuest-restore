using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] public HealthSystem healthSystem;

    private void OnCollisionEnter2D(Collision2D collision)
    {
            if (collision.gameObject.tag == "Player")
        {
            healthSystem.Damage(damage);
        }
    }
}
