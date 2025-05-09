using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class LocalAISendData
{
    public string prompt;
    public int max_tokens = 200;
    public float temperature = 0.7f;
}

[Serializable]
public class LocalAIResponseData
{
    public string response; // API仕様に応じて書き換え（例：text, resultなど）
}

public class LocalAIClient : MonoBehaviour
{
    [SerializeField] private string apiUrl = "http://localhost:5000/api/generate";

    public void SendToLocalAI(string prompt, Action<string> onResponse)
    {
        var data = new LocalAISendData
        {
            prompt = prompt,
            max_tokens = 200,
            temperature = 0.7f
        };

        string jsonData = JsonUtility.ToJson(data);
        StartCoroutine(PostRequest(apiUrl, jsonData, onResponse));
    }

    private IEnumerator PostRequest(string url, string jsonData, Action<string> onResponse)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

#if UNITY_2022_1_OR_NEWER
        if (request.result != UnityWebRequest.Result.Success)
#else
        if (request.isNetworkError || request.isHttpError)
#endif
        {
            Debug.LogError("Local AI Error: " + request.error);
        }
        else
        {
            var json = request.downloadHandler.text;
            LocalAIResponseData result = JsonUtility.FromJson<LocalAIResponseData>(json);
            onResponse?.Invoke(result.response); // or result.text etc.
        }
    }
}

