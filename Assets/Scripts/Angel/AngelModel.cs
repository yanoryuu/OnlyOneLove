using UnityEngine;

/// <summary>
/// 恋愛対象キャラクター「Angel」のパラメーター管理クラス
/// </summary>
public class AngelModel
{
    
    private AngelParameter angelParameter = new AngelParameter();
    public AngelParameter AngelParameter => angelParameter;

    // パラメーターの更新
    public void UpdateParameter(AngelParameter parameters)
    {
        angelParameter.affection = parameters.affection;
        angelParameter.trust = parameters.trust;
        angelParameter.jealousy = parameters.jealousy;
        angelParameter.closeness = parameters.closeness;
    }
    
    // コンストラクタ
    public void Initialize()
    {
        angelParameter.affection = 0f;
        angelParameter.trust = 0f;
        angelParameter.jealousy = 0f;
        angelParameter.closeness = 0f;
    }
    // すべての値を初期化
    public AngelModel()
    {
        Initialize();
    }
}