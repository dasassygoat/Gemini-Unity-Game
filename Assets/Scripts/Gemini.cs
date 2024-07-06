using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Gemini : MonoBehaviour
{
    private string GeminiEndPoint ="";
    private string APIKey ="";
    [SerializeField] private string message = "Tell me how to make  alfredo sauce.";

    void Start()
    {
        GeminiEndPoint = ConfigManager.configData.GeminiEndPoint;
        APIKey = ConfigManager.configData.APIKey;

        SendRequestToGemini();
    }

    public void SendRequestToGemini()
    {
        string requestData = "{\"contents\":[{\"parts\":[{\"text\":\"" + message + "\"}]}]}";
        UnityWebRequest webRequest = new UnityWebRequest(GeminiEndPoint + "?key=" + APIKey, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(requestData);

        webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("x-api-key", APIKey);
        webRequest.SetRequestHeader("Content-Type", "application/json");
        StartCoroutine(SendRequest(webRequest));
    }

    IEnumerator SendRequest(UnityWebRequest request)
    {
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            ExtractAndLogText(jsonResponse);
        }
    }

    private void ExtractAndLogText(string jsonResponse)
    {
        //Debug.Log(jsonResponse);
        // Get the response from the GeminiResponse class of jsonResponse
        GeminiResponse geminiResponse = JsonUtility.FromJson<GeminiResponse>(jsonResponse);

        if (geminiResponse.candidates[0].content != null)
        {
            string text = geminiResponse.candidates[0].content.parts[0].text;
            Debug.Log(text);
        }
    }
}

[System.Serializable]
public class GeminiResponse
{
    public Candidate[] candidates;
    [System.Serializable]
    public class Candidate
    {
        public Content content;

        [System.Serializable]
        public class Content
        {
            public Part[] parts;

            [System.Serializable]
            public class Part
            {
                public string text;

            }
        }
    }
}
