using UnityEngine;
using TMPro;

public class DashSpeedUpgradeStation : MonoBehaviour, IInteractable
{
    [SerializeField] private int[] upgradeCosts = { 20, 40, 80, 120, 200 };
    private int currentUpgradeLevel = 0;
    private const int maxUpgradeLevel = 5;

    [SerializeField] private TMP_Text upgradeMessageText;
    private bool isPlayerInRange = false;

    public void Interact(NewPlayerMovement player)
    {
        if (currentUpgradeLevel < maxUpgradeLevel)
        {
            int cost = upgradeCosts[currentUpgradeLevel];

            if (player.GetCoins() >= cost)
            {
                player.AddCoins(-cost);
                currentUpgradeLevel++;
                player.UpgradeDashSpeed(2f); 
                Debug.Log($"Dash speed upgraded to level {currentUpgradeLevel}! Current dash speed: {player.dashSpeed}.");
            }
            else
            {
                Debug.Log("Not enough coins to upgrade dash speed.");
            }
        }
        else
        {
            Debug.Log("Maximum dash speed upgrade level reached.");
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
                upgradeMessageText.text = "Dash speed at maximum level";
            }
            else
            {
                int nextCost = upgradeCosts[currentUpgradeLevel];
                if (player.GetCoins() >= nextCost)
                {
                    upgradeMessageText.text = $"Level ({currentUpgradeLevel}) {nextCost} gold required for next upgrade\nPress E to upgrade dash speed";
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
