using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int coinValue = 0;
    private bool isCollected = false;  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCollected)
        {
            Debug.Log("Coin collected!");
            NewPlayerMovement playerCoins = collision.GetComponent<NewPlayerMovement>();
            if (playerCoins != null)
            {
                playerCoins.AddCoins(coinValue);
            }

            isCollected = true;  
            Destroy(gameObject); 
        }
    }

}
