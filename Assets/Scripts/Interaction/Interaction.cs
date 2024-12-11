using UnityEngine;
using TMPro;

public class InteractionTrigger2D : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactionText;

    private void Start()
    {
        interactionText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player")) 
        {
            Debug.Log("Player Collider i�ine girdi");
            interactionText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("Player Collider'dan ��kt�");
            interactionText.gameObject.SetActive(false);
        }
    }
}
