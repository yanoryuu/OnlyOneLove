using UnityEngine;

/// <summary>
/// 恋愛対象キャラクター「Angel」のパラメーター管理クラス
/// </summary>
public class AngelModel
{
    // 愛情（プレイヤーに対する好意度）
    private float affection;
    public float Affection => affection;

    // 信頼度（Trust） - 嘘・裏切りなどで下がる。真剣な関係になるには一定以上必要。
    private float trust;
    public float Trust => trust;

    // 嫉妬度（Jealousy） - 他キャラとの接触で上昇。イベントや会話に影響を与えることも。
    private float jealousy;
    public float Jealousy => jealousy;

    // 親密度 - 高すぎると友達になりノーマルエンドになることも
    private float closeness;
    public float Closeness => closeness;

    // 秘密（Secret） - 特定条件で発覚するキャラごとの隠し設定。発見でルート分岐あり。
    private float secret;
    public float Secret => secret;

    // 魅力（Charm） - 見た目や性格などの総合的な魅力度
    private float charm;
    public float Charm => charm;

    // パラメーターの更新
    public void UpdateParameter(Parameters parameters)
    {
        affection += parameters.affection;
        trust += parameters.trust;
        jealousy += parameters.jealousy;
        closeness += parameters.closeness;
        secret += parameters.secret;
        charm += parameters.charm;
    }
    
    // コンストラクタ
    public void Initialize()
    {
        affection = 0f;
        trust = 0f;
        jealousy = 0f;
        closeness = 0f;
        secret = 0f;
        charm = 0f;
    }
    // すべての値を初期化
    public AngelModel()
    {
        Initialize();
    }
}