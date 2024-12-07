using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBoxController : MonoBehaviour
{
    public GameObject chatBoxPanel;          
    public TMP_InputField userInputField;    
    public TMP_Text responseText;            
    public Button sendButton;                
    public ChatbotAPI chatbotAPI;            

    private void Start()
    {
        
        chatBoxPanel.SetActive(false);

        
        sendButton.onClick.AddListener(SendQuestion);
    }

    
    public void ToggleChatBox()
    {
        chatBoxPanel.SetActive(!chatBoxPanel.activeSelf);
        if (chatBoxPanel.activeSelf)
        {
            userInputField.Select();  
        }
    }

    public void CloseChatBox()
    {
        chatBoxPanel.SetActive(false);  
    }

    
    private void SendQuestion()
    {
        string question = userInputField.text;
        if (!string.IsNullOrEmpty(question))
        {
            responseText.text = "Waiting for response...";  
            chatbotAPI.SendQuestionToChatbot(question);
        }
    }

    
    public void DisplayResponse(string response)
    {
        responseText.text = response;
    }


}
