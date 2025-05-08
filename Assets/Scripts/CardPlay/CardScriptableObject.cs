using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Card/CreateCardAsset")]
public class CardScriptableObject : ScriptableObject
{
    /// <summary>
    /// 基本的なカード情報
    /// </summary>
    //追加効果が必要であればアタッチ
    [SerializeReference]
    public List<AdditionalEffect> additionalEffect = new List<AdditionalEffect>();
    
    //プレイに必要な好感度
    // public int playCostAffection = 1;
    
    //カードの名前
    public string cardName;
    
    //プレイに必要なアクションポイント
    public int playActionPoints = 1; 
    
    //プレイに必要な最低限のプレイヤーのパラメーターを利用するかのフラグ
    public bool useRequirmentPlayerParameter;
    
    //プレイに必要な最低限のプレイヤーのパラメーター
    public PlayerParameter requirmentPlayerParameter = new PlayerParameter();
    
    public  cardTypes cardType;
    
    public  Sprite cardSprite;
    
    /// <summary>
    /// カードの種類ごとの効果値
    /// </summary>
    /// 
    //好感度を上げる
    public  int affectionUpNum = 0;
    
    //継続ターン数
    public int keepTurns = 1;
    
    //探すカードの種類
    public List<cardTypes> searchCardType = new List<cardTypes>();
    
    //追加するActionPoint
    public int addAP = 2; 
    
    //効果を与えるターゲットパラメーター
    public Parameters addParameterNum;
    
    [Serializable]
    public enum cardTypes
    {
        Talk,     // パラメータ上昇（弱・強）を包括
        Comment,
        Action,
        Psychic,
        Special,
        Confession
    }
}