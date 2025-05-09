using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIPromptPayload
{
    public string                      userInput;          // プレイヤーの発言
    public AngelParameter             currentParameter;   // 会話前の現在パラメーター
    public List<CardEffectEntry>      cardDeltas;         // カードごとのパラメーター変更値
}

public static class AIPromptBuilder
{
    public static string BuildPromptJson(
        string userInput,
        AngelParameter currentParam,
        List<CardEffectEntry> cardDeltas)
    {
        var payload = new AIPromptPayload
        {
            userInput        = userInput,
            currentParameter = currentParam,
            cardDeltas       = cardDeltas
        };
        return JsonUtility.ToJson(payload);
    }
}


public class CardEffectEntry
{
    public string description;
    public AngelParameter effect;
}

[System.Serializable]
public class AIResponseData
{
    public string         reply;               // AIのテキスト返答
    public AngelParameter deltaParameter;      // AIが提案するパラメーター変更値
}
