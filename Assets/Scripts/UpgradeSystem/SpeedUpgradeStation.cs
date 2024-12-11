using UnityEngine;
using TMPro;

public class SpeedUpgradeStation : MonoBehaviour, IInteractable
{
    [SerializeField] private int[] upgradeCosts = { 15, 30, 60, 100, 150 };
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
                player.UpgradeSpeed(0.5f);
                Debug.Log($"Speed upgraded to level {currentUpgradeLevel}! Current speed: {player.moveSpeed}.");
            }
            else
            {
                Debug.Log("Not enough coins to upgrade speed.");
            }
        }
        else
        {
            Debug.Log("Maximum speed upgrade level reached.");
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
                upgradeMessageText.text = "Speed at maximum level";
            }
            else
            {
                int nextCost = upgradeCosts[currentUpgradeLevel];
                if (player.GetCoins() >= nextCost)
                {
                    upgradeMessageText.text = $"Level ({currentUpgradeLevel}) {nextCost} gold required for next upgrade\nPress E to upgrade speed";
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
