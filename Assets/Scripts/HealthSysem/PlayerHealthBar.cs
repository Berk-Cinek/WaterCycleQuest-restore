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

    private void Update()
    {
        if (player != null)
        {
            healthSlider.value = player.health; 

            
            if (player.health > 50)
            {
                healthSlider.fillRect.GetComponent<Image>().color = Color.green;
            }
            else if (player.health > 20)
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
