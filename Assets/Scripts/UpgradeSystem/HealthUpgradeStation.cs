using UnityEngine;
using TMPro;

public class HealthUpgradeStation : MonoBehaviour, IInteractable
{
    [SerializeField] private int[] healthUpgradeCosts = { 15, 30, 60, 90, 120 };  
    private int currentUpgradeLevel = 0;  
    private const int maxUpgradeLevel = 5;  

    [SerializeField] private TMP_Text upgradeMessageText;  
    private bool isPlayerInRange = false;  

    public void Interact(NewPlayerMovement player)
    {
        if (currentUpgradeLevel < maxUpgradeLevel)
        {
            int cost = healthUpgradeCosts[currentUpgradeLevel]; 

            if (player.GetCoins() >= cost)  
            {
                player.AddCoins(-cost);  
                currentUpgradeLevel++;  
                player.IncreaseMaxHealth(20);  
                player.RestoreHealth(20);  
                Debug.Log($"Health upgraded to level {currentUpgradeLevel}! Current max health: {player.maxHealth}.");
            }
            else
            {
                Debug.Log("Not enough coins to upgrade health.");
            }
        }
        else
        {
            Debug.Log("Maximum upgrade level reached. Cannot upgrade further.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;  

            NewPlayerMovement player = other.GetComponent<NewPlayerMovement>();
            player.SetInteractable(this);  

            UpdateUpgradeMessage(player);  
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;  

            NewPlayerMovement player = other.GetComponent<NewPlayerMovement>();
            player.SetInteractable(null);  

            ClearUpgradeMessage();  
        }
    }

    private void Update()
    {
        if (isPlayerInRange)
        {
            NewPlayerMovement player = FindObjectOfType<NewPlayerMovement>();
            UpdateUpgradeMessage(player);  
        }
    }

    private void UpdateUpgradeMessage(NewPlayerMovement player)
    {
        if (upgradeMessageText != null)
        {
            if (currentUpgradeLevel >= maxUpgradeLevel)  
            {
                upgradeMessageText.text = "Health at maximum level";
            }
            else
            {
                int nextCost = healthUpgradeCosts[currentUpgradeLevel];  
                if (player.GetCoins() >= nextCost)  
                {
                    upgradeMessageText.text = $"Level ({currentUpgradeLevel}) {nextCost} gold required for next upgrade\nPress E to upgrade health";
                }
                else
                {
                    upgradeMessageText.text = $"Level ({currentUpgradeLevel}) {nextCost} gold required\nNot enough coins to upgrade";
                }
            }
        }
    }

    private void ClearUpgradeMessage()
    {
        if (upgradeMessageText != null)
        {
            upgradeMessageText.text = string.Empty;  
        }
    }
}
