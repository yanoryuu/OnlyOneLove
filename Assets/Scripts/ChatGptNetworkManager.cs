using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;

#region リクエスト・レスポンス用クラス

[Serializable]
public class ChatRequestMessage {
    public string role;
    public string content;
}

[Serializable]
public class ChatRequestPayload {
    public string model;
    public List<ChatRequestMessage> messages;
    public float temperature;
}

[Serializable]
public class ChatResponseMessage {
    public string role;
    public string content;
}

[Serializable]
public class ChatChoice {
    public int index;
    public ChatResponseMessage message;
    public string finish_reason;
}

[Serializable]
public class ChatResponse {
    public List<ChatChoice> choices;
}

// ChatGPT が生成する JSON レスポンスを想定したクラス
[Serializable]
public class ResponseData {
    public string reply;
    public Parameters parameters;
}

/// <summary>
/// 好感度や性格などのパラメーター情報用クラス
/// </summary>
[Serializable]
public class Parameters
{
    /// <summary>
    /// 愛情（プレイヤーに対する好意度）
    /// </summary>
    public float affection;

    /// <summary>
    /// 信頼度（Trust） - 嘘・裏切りなどで下がる。真剣な関係になるには一定以上必要。
    /// </summary>
    public float trust;

    /// <summary>
    /// 嫉妬度（Jealousy） - 他キャラとの接触で上昇。イベントや会話に影響を与えることも。
    /// </summary>
    public float jealousy;

    /// <summary>
    /// 親密度（高すぎると友達になる。ノーマルエンドに繋がるが、他のパラメータが高ければプラスにも働く）
    /// </summary>
    public float closeness;

    /// <summary>
    /// 秘密（Secret） - 特定条件で発覚するキャラごとの隠し設定。発見でルート分岐あり。
    /// </summary>
    public float secret;

    /// <summary>
    /// 魅力（キャラクターの見た目・性格・話し方などの総合的な引力）
    /// </summary>
    public float charm;
}

#endregion

public class ChatGptNetworkManager : MonoBehaviour
{
    [SerializeField] private string apiKey = "YOUR_API_KEY_HERE";
    [SerializeField] private string apiUrl = "https://api.openai.com/v1/chat/completions";

    /// <summary>
    /// プレイヤーからの入力テキストをもとに、ChatGPT に非同期でリクエストを送信します。
    /// JSON フォーマットのレスポンスを ResponseData 型にパースして返します。
    /// エラー発生時は例外をスローします。
    /// </summary>
    /// <param name="userInput">プレイヤーからの入力</param>
    /// <returns>ChatGPT の返答と更新されたパラメーター</returns>
    public async UniTask<ResponseData> SendChatRequestAsync(string userInput)
    {
        // リクエスト用ペイロード作成
        ChatRequestPayload payload = new ChatRequestPayload {
            model = "gpt-3.5-turbo",
            temperature = 0.7f,
            messages = new List<ChatRequestMessage>()
        };

        // システムメッセージで出力フォーマットを指示
        ChatRequestMessage systemMsg = new ChatRequestMessage {
            role = "system",
            content =
                "これから好感度や性格パラメーターを更新するためのJSON形式のレスポンスを返してください。"
                + "レスポンスは必ず下記のフォーマットに従ってください: "
                + "{\"reply\": string, \"parameters\": {\"likability\": number, \"personality\": string}}。"
                + "余計なテキストは出力しないようにしてください。"
        };
        payload.messages.Add(systemMsg);

        // ユーザーメッセージを追加
        ChatRequestMessage userMsg = new ChatRequestMessage {
            role = "user",
            content = userInput
        };
        payload.messages.Add(userMsg);

        // JSON にシリアライズ
        string jsonPayload = JsonUtility.ToJson(payload);
        Debug.Log("送信する JSON:\n" + jsonPayload);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, UnityWebRequest.kHttpVerbPOST))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            // UniTask を利用して非同期送信
            await request.SendWebRequest().ToUniTask();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("受信した JSON:\n" + jsonResponse);

                // API レスポンスをパース
                ChatResponse chatResponse = JsonUtility.FromJson<ChatResponse>(jsonResponse);
                if (chatResponse != null &&
                    chatResponse.choices != null &&
                    chatResponse.choices.Count > 0)
                {
                    // チャット API の返信は JSON 文字列として返される前提
                    string content = chatResponse.choices[0].message.content;
                    try
                    {
                        ResponseData responseData = JsonUtility.FromJson<ResponseData>(content);
                        return responseData;
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("JSON パース中にエラーが発生しました: " + ex.Message);
                        throw new Exception("JSONパースエラー", ex);
                    }
                }
                else
                {
                    Debug.LogError("レスポンスフォーマットが不正です。");
                    throw new Exception("レスポンスフォーマットが不正です。");
                }
            }
            else
            {
                Debug.LogError("リクエストエラー: " + request.error);
                throw new Exception("リクエストエラー: " + request.error);
            }
        }
    }
}
