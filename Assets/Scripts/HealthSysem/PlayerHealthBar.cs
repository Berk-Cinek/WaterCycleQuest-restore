using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;  
    [SerializeField] private NewPlayerMovement player; 

    private void Start()
    {
        if (player != null)
        {
            healthSlider.maxValue = player.maxHealth;  
            healthSlider.value = player.health; 
        }
    }

    public void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = player.maxHealth;  
            healthSlider.value = player.health;  
        }
    }


    private void Update()
    {
        if (player != null)
        {
            
            healthSlider.value = player.health;

            
            float healthPercentage = (float)player.health / player.maxHealth;

            
            if (healthPercentage > 0.6f)
            {
                healthSlider.fillRect.GetComponent<Image>().color = Color.green;
            }
            else if (healthPercentage > 0.3f)
            {
                healthSlider.fillRect.GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                healthSlider.fillRect.GetComponent<Image>().color = Color.red;
            }
        }
    }

}
