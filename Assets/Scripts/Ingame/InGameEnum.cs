using UnityEngine;

public class InGameEnum
{
    public enum GameState
    {
        Default,
        PlayerTurn,      // プレイヤーのターン（カード選択/会話）
        CardEffect,      // カード効果の発動処理
        Talk,        // 会話シーン（カード効果 or イベント）
        FinishTurn,      // ターンの終了
        CheckStatus,     // 状態更新（好感度・SP等）
        Confession,      // 告白フェーズ
    }
}
