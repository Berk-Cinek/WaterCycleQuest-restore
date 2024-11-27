using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ChatbotAPI : MonoBehaviour
{
    
    private string flaskUrl = "http://127.0.0.1:5000/chat";  


    public ChatBoxController chatBoxController;

    
    public void SendQuestionToChatbot(string question)
    {
        StartCoroutine(PostRequest(question));
    }

    
    private IEnumerator PostRequest(string question)
{
    
    string jsonData = JsonUtility.ToJson(new QuestionData { question = question });

    
    UnityWebRequest request = new UnityWebRequest(flaskUrl, "POST");
    byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    request.downloadHandler = new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");

    
    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
        
        string responseText = request.downloadHandler.text;
        Debug.Log("Raw Chatbot Response: " + responseText);

        
        ChatbotResponse chatbotResponse = JsonUtility.FromJson<ChatbotResponse>(responseText);
        string answer = chatbotResponse.answer;  
        
        
        chatBoxController.DisplayResponse(answer);
    }
    else
    {
        
        Debug.LogError("Error: " + request.error);
        chatBoxController.DisplayResponse("Error: Could not get response.");
    }
}

    
    [System.Serializable]
    private class QuestionData
    {
        public string question;
    }

    [System.Serializable]
    private class ChatbotResponse
{
    public string answer;
}

}
