using UnityEngine;

public class HealthItem : MonoBehaviour
{
    [SerializeField] private int healthRestoreAmount = 20; 
    [SerializeField] private float pickupRadius = 2f; 
    [SerializeField] private float destroyDelay = 0.5f; 

    private bool hasBeenUsed = false; 

    private void Update()
    {
        
        if (Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= pickupRadius && !hasBeenUsed)
        {
            
            AttemptPickup();
        }
    }

    private void AttemptPickup()
    {
        
        NewPlayerMovement player = GameObject.FindGameObjectWithTag("Player").GetComponent<NewPlayerMovement>();

        
        if (player.health < player.maxHealth)
        {
            
            int healthToRestore = Mathf.Min(healthRestoreAmount, player.maxHealth - player.health);
            player.health += healthToRestore;

            
            Debug.Log($"Restored {healthToRestore} health. Current health: {player.health}");

            
            hasBeenUsed = true;

            
            Destroy(gameObject, destroyDelay);
        }
    }
}

