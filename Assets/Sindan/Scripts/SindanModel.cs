using UnityEngine;
using Cysharp.Threading.Tasks;
using Utage;

// Model 層：ChatGPT 通信を担当
public class SindanModel : MonoBehaviour
{
    private ChatGptNetworkManager gptMgr;

    void Awake()
    {
        gptMgr = FindObjectOfType<ChatGptNetworkManager>();
        if (gptMgr == null)
            Debug.LogError("ChatGptNetworkManager が見つかりません");
    }

    // ChatGPT へ非同期リクエストし、結果の文字列を返す
    public async UniTask<string> SendMessageAsync(string prompt)
    {
        try
        {
            var response = await gptMgr.SendChatRequestAsync(prompt);
            return response.reply;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("ChatGPT 通信エラー: " + ex.Message);
            return "エラー：通信失敗";
        }
    }

    public async UniTask<(float likability, string personality)> SendMessageWithParamsAsync(string prompt)
    {
        try
        {
            var response = await gptMgr.SendChatRequestAsync(prompt);
            return (response.parameters.likability, response.parameters.personality);
        }
        catch
        {
            return (0f, "");
        }
    }
}
