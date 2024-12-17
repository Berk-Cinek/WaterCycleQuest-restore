using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBoxController : MonoBehaviour
{
    [SerializeField] private Animator animator;
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
        int i = 0;

        if (chatBoxPanel.activeSelf && i % 2 == 0)
        {
            Time.timeScale = 0f;
            userInputField.Select();
            animator.enabled = false;
        }
        else
        {
            Time.timeScale = 1.0f;
            userInputField.Select();
            animator.enabled = true;
        }
    }

    public void CloseChatBox()
    {
        chatBoxPanel.SetActive(false);
        Time.timeScale = 1.0f;
        animator.enabled = true;

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
