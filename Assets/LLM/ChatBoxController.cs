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

    public static bool IsChatBoxActive { get; private set; }

    private void Start()
    {
        chatBoxPanel.SetActive(false);
        IsChatBoxActive = false;

        // add listener for send
        sendButton.onClick.AddListener(SendQuestion);

        // Add listener for the Enter key (onSubmit event)
        userInputField.onSubmit.AddListener(delegate { SendQuestion(); });
    }

    private void Update()
    {
        // Check if ESC key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Close the chat box if it's active
            if (chatBoxPanel.activeSelf)
            {
                CloseChatBox();
            }
        }
    }

    public void ToggleChatBox()
    {
        chatBoxPanel.SetActive(!chatBoxPanel.activeSelf);
        IsChatBoxActive = chatBoxPanel.activeSelf; // Update the flag

        if (IsChatBoxActive)
        {
            Time.timeScale = 0f;
            userInputField.Select();
            animator.enabled = false;
        }
        else
        {
            Time.timeScale = 1.0f;
            animator.enabled = true;
        }
    }

    public void CloseChatBox()
    {
        chatBoxPanel.SetActive(false);
        IsChatBoxActive = false; // Update the flag
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

            // Clear the input field after sending the question
            userInputField.text = string.Empty;
            userInputField.ActivateInputField();
        }
    }

    public void DisplayResponse(string response)
    {
        responseText.text = response;
    }
}