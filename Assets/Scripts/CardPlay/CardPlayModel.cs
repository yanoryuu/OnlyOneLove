using System.Collections.Generic;
using R3;
using UnityEngine;

/// <summary>
/// ゲーム中のカードプレイに関するモデル
/// </summary>
public class CardPlayModel
{
    // 現在の保持カード
    private ReactiveProperty<List<CardBase>> currentHoldCard;
    public ReactiveProperty<List<CardBase>> CurrentHoldCard => currentHoldCard;
    
    // 現在の保持カードの数
    private ReactiveProperty<int> currentHoldCardIndex;
    public ReactiveProperty<int> CurrentHoldCardIndex => currentHoldCardIndex;
    
    // 最大カード保持数
    private int maxHoldCards; 
    public int MaxHoldCards => maxHoldCards;
    
    //現在の会話カード
    private ReactiveProperty<List<CardBase>> currentHoldTopicCard;
    public ReactiveProperty<List<CardBase>> CurrentHoldTopicCard => currentHoldTopicCard;
    
    //会話カード保持数
    private ReactiveProperty<int> currentHoldTopicCardIndex;
    public ReactiveProperty<int> CurrentHoldTopicCardIndex => currentHoldTopicCardIndex;
    
    //会話カードの最大保持数
    private int maxTopicCards;
    public int MaxTopicCards => maxTopicCards;
    
    // 墓地カード
    private List<CardBase> playedCards;
    public List<CardBase> PlayedCards => playedCards;

    // 新しく追加されたカードを通知する
    private Subject<CardBase> onAddCard;
    public Observable<CardBase> OnAddCard => onAddCard;
    
    private Subject<CardBase> onAddTopicCard;
    public Observable<CardBase> OnAddTopicCard => onAddTopicCard;
    
    // プレイヤーのステータス（ReactiveProperty使用）
    private PlayerParameterRuntime playerParameter;
    public PlayerParameterRuntime PlayerParameter => playerParameter;

    private ReactiveProperty<CardBase> talkTopic;
    public ReactiveProperty<CardBase> TalkTopic => talkTopic;
    
    // コンストラクタ
    public CardPlayModel()
    {
        // プレイヤーパラメータの初期化（ReactiveProperty）
        playerParameter = new PlayerParameterRuntime();

        // 保持カード、インデックスなどの初期化
        currentHoldTopicCard = new ReactiveProperty<List<CardBase>>(new List<CardBase>());
        currentHoldTopicCardIndex = new ReactiveProperty<int>();
        currentHoldCard = new ReactiveProperty<List<CardBase>>(new List<CardBase>());
        currentHoldCardIndex = new ReactiveProperty<int>(0);
        playedCards = new List<CardBase>();
        onAddCard = new Subject<CardBase>();
        onAddTopicCard = new Subject<CardBase>();
        talkTopic = new ReactiveProperty<CardBase>();
        
        
        playerParameter.ActionPoint.Value = 3; // 初期AP設定
        maxHoldCards = CardPlayConst.maxHoldCardNum;
        maxTopicCards = CardPlayConst.maxHoldTopicCard;
    }

    // カードを追加
    public void AddCard(CardBase card)
    {
        if (card.CardData.cardType == CardScriptableObject.cardTypes.Topic)
        {
            Debug.Log("会話カードはAddTopicを使ってください");
            return;
        }
        
        if (currentHoldCard.Value.Count >= maxHoldCards)
        {
            // 最大手札数に達していたら追加しない
            Debug.LogWarning($"カードを追加できません。最大保持数({maxHoldCards})に達しています。");
            return;
        }

        currentHoldCardIndex.Value++;
        currentHoldCard.Value.Add(card);
        onAddCard.OnNext(card);
    }

    public void AddTopic(CardBase card)
    {
        if (card.CardData.cardType != CardScriptableObject.cardTypes.Topic)
        {
            Debug.Log("会話カードではないです");
            return;
        }
        if (currentHoldTopicCard.Value.Count >= maxHoldCards)
        {
            // 最大手札数に達していたら追加しない
            Debug.LogWarning($"カードを追加できません。最大保持数({maxHoldCards})に達しています。");
            return;
        }

        currentHoldTopicCardIndex.Value++;
        currentHoldTopicCard.Value.Add(card);
        onAddTopicCard.OnNext(card);
    }

    // カードを削除
    public void RemoveCard(CardBase card)
    {
        currentHoldCard.Value.Remove(card);
        currentHoldCardIndex.Value--;
    }

    // カードプレイ時の処理
    public void PlayCard(CardBase card)
    {
        playedCards.Add(card);
    }

    // 行動ポイントを追加
    public void AddActionPoint(int point)
    {
        playerParameter.ActionPoint.Value += point;
    }

    // 初期化（ターン開始や再スタート用）
    public void Initialize()
    {
        Debug.Log("Initialize");
        playerParameter.ActionPoint.Value = 3;
    }

    public void SetTalkTopic(CardBase TopicCard)
    {
        talkTopic.Value = TopicCard;
    }

    public CardBase ResetTalkTopic()
    {
        var lastTopic = talkTopic.Value;
        
        talkTopic.Value = null;
        return lastTopic;
    }
}