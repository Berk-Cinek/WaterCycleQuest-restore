using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Diagnostics; // For managing processes
using System.IO;          // For path management

public class ChatbotAPI : MonoBehaviour
{
    private string flaskUrl = "http://127.0.0.1:5000/chat"; // Flask server endpoint
    private Process flaskProcess;                          // Process to manage Flask server
    public ChatBoxController chatBoxController;            // Reference to ChatBoxController

    void Start()
    {
        StartFlaskServer(); // Start the Flask server when Unity starts
    }

    // Function to start the Flask server
    private void StartFlaskServer()
    {
        string pythonPath = @"C:\Users\bcine\AppData\Local\Programs\Python\Python310\python.exe";
        string flaskAppPath = @"C:\Users\bcine\Desktop\LLMgpt2\app.py";

        try
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = pythonPath,
                Arguments = $"\"{flaskAppPath}\"", // Pass the path to app.py
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            flaskProcess = Process.Start(startInfo); // Start the Flask server
            UnityEngine.Debug.Log("Flask server started successfully.");
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError("Failed to start Flask server: " + ex.Message);
        }
    }

    // Function to send a question to the chatbot
    public void SendQuestionToChatbot(string question)
    {
        StartCoroutine(PostRequest(question));
    }

    // Coroutine to send POST request to Flask server
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
            UnityEngine.Debug.Log("Raw Chatbot Response: " + responseText);

            // Parse the response JSON
            ChatbotResponse chatbotResponse = JsonUtility.FromJson<ChatbotResponse>(responseText);
            string answer = chatbotResponse.answer;

            // Display the chatbot's response
            chatBoxController.DisplayResponse(answer);
        }
        else
        {
            UnityEngine.Debug.LogError("Error: " + request.error);
            chatBoxController.DisplayResponse("Error: Could not get response.");
        }
    }

    // Ensure the Flask server process is terminated when the game exits
    private void OnApplicationQuit()
    {
        if (flaskProcess != null && !flaskProcess.HasExited)
        {
            flaskProcess.Kill();
            UnityEngine.Debug.Log("Flask server process terminated.");
        }
    }

    // Classes to handle JSON serialization
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


//using System.Collections;
//using UnityEngine;
//using UnityEngine.Networking;

//public class ChatbotAPI : MonoBehaviour
//{

//    private string flaskUrl = "http://127.0.0.1:5000/chat";


//    public ChatBoxController chatBoxController;


//    public void SendQuestionToChatbot(string question)
//    {
//        StartCoroutine(PostRequest(question));
//    }


//    private IEnumerator PostRequest(string question)
//    {

//        string jsonData = JsonUtility.ToJson(new QuestionData { question = question });


//        UnityWebRequest request = new UnityWebRequest(flaskUrl, "POST");
//        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
//        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
//        request.downloadHandler = new DownloadHandlerBuffer();
//        request.SetRequestHeader("Content-Type", "application/json");


//        yield return request.SendWebRequest();

//        if (request.result == UnityWebRequest.Result.Success)
//        {

//            string responseText = request.downloadHandler.text;
//            Debug.Log("Raw Chatbot Response: " + responseText);


//            ChatbotResponse chatbotResponse = JsonUtility.FromJson<ChatbotResponse>(responseText);
//            string answer = chatbotResponse.answer;


//            chatBoxController.DisplayResponse(answer);
//        }
//        else
//        {

//            Debug.LogError("Error: " + request.error);
//            chatBoxController.DisplayResponse("Error: Could not get response.");
//        }
//    }


//    [System.Serializable]
//    private class QuestionData
//    {
//        public string question;
//    }

//    [System.Serializable]
//    private class ChatbotResponse
//    {
//        public string answer;
//    }

//}
