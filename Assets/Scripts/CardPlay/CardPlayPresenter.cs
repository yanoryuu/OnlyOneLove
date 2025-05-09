using System;
using System.Collections.Generic;
using CardGame;
using NUnit.Framework;
using UnityEngine;
using R3;
using Random = UnityEngine.Random;

public class CardPlayPresenter : MonoBehaviour
{
    private CardPlayModel model;
    public CardPlayModel Model => model;
    
    [SerializeField] private CardPlayView view;
    
    [SerializeField] private ChooseTopicView chooseTopicView;
    
    [SerializeField] private AngelTalkView angelTalkView;
    
    [SerializeField] private CardFactory cardFactory;
    
    [SerializeField] private AngelPresenter angelPresenter;
    
    [SerializeField] private LocalAIClient localAIClient;
    public AngelPresenter AngelPresenter => angelPresenter;

    //現在選択中のカード
    private CardBase currentSelectedCard;
    
    //使用カード（最大３枚）
    private List<CardBase> setCards = new List<CardBase>();
    public List<CardBase> SetCards => setCards;

    private ReactiveProperty<bool> isProcessing;
    
    //デート中（デート中は告白成功率アップ）
    private ReactiveProperty<bool> isDate;
    public ReactiveProperty<bool> IsDate => isDate;

    //カップル状態
    private ReactiveProperty<bool> isCouple;
    public ReactiveProperty<bool> IsCouple => isCouple;
    
    //ゲーム開始時のドロープール
    [SerializeField] private List<CardScriptableObject.cardTypes> startCardPool = new List<CardScriptableObject.cardTypes>();

    //ゲーム開始時にカードを引く枚数
    [SerializeField] private int startGetCardNum = 5;
    
    private List<CardBase> oneTurnUsedCards = new List<CardBase>();

    private void Start()
    {
        
        model = new CardPlayModel();
        isProcessing = new ReactiveProperty<bool>();
        isDate = new ReactiveProperty<bool>();
        Bind();
        
        //ゲーム開始時のドロー
        StartDrawCards(startCardPool,startGetCardNum);
    }

    private void Bind()
    {
        model.OnAddCard
            .Subscribe(cardData =>
            {
                cardData.cardButton.OnClickAsObservable()
                    .Where(_ => InGameManager.Instance.CurrentState.Value == InGameEnum.GameState.PlayerTurn)
                    .Subscribe(x =>
                    {
                        Debug.Log($"Select {x.ToString()}");
                        currentSelectedCard = cardData;
                    })
                    .AddTo(cardData);

                // ビューに追加
                view.AddCard(cardData);
                Debug.Log($"{cardData}のカードを追加しました。");
            })
            .AddTo(this);

        //会話パート終了
        view.SetButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                if (currentSelectedCard != null)
                {
                    SetCard(currentSelectedCard);
                }
            })
            .AddTo(this);
        
        //会話送信ボタン
        view.TalkButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                // a. 現在パラを取得
                AngelParameter before = angelPresenter.GetAngelParameter();

                // b. カード効果を適用しつつデルタも集める
                AngelParameter tmp = new AngelParameter();
                List<CardEffectEntry> cardDeltas = new List<CardEffectEntry>();
                foreach (var card in SetCards)
                {
                    tmp = card.PlayCard(this, tmp);
                    cardDeltas.Add(card.GetEffectForAI());
                    oneTurnUsedCards.Add(card);
                }

                // c. JSONを組み立てて送信
                string json = AIPromptBuilder.BuildPromptJson(
                    view.TalkInputField.text,
                    before,
                    cardDeltas
                );
                SendToAITalk(json);

                // 後片付け
                setCards.Clear();
                oneTurnUsedCards.Clear();
            })
            .AddTo(this);
        
        InGameManager.Instance.CurrentState.Where(x => x == InGameEnum.GameState.PlayerTurn)
            .Subscribe(_=>
            {
                model.Initialize();
                //選択中のカードを初期化
                currentSelectedCard = null;
            })
            .AddTo(this);
        
        model.CurrentHoldCardIndex.Subscribe(x=>view.SetRestCards(x))
            .AddTo(this);
        
        chooseTopicView.SetTopicButton.OnClickAsObservable()
            .Subscribe(_ => model.SetTalkTopic(currentSelectedCard))
            .AddTo(this);
    }
    
    public List<CardEffectEntry> CollectUsedCardEffects()
    {
        var list = new List<CardEffectEntry>();
        foreach (var card in oneTurnUsedCards)
        {
            list.Add(card.GetEffectForAI());
        }
        return list;
    }
    

    //カード使用時の演出
    private AngelParameter PlayCard(CardBase card,AngelParameter aiOfAngelParameter)
    {
        Debug.Log($"使用：{card}");
        
        //コストの増減など
        model.PlayCard(card);
        
        //見た目
        view.PlayCard(card);
        
        //実際の効果
        var parameter = card.PlayCard(this, aiOfAngelParameter);
        
        //追加効果
        if (card.CardData.additionalEffect != null) card.PlayAdditionalEffect(this);
        
        //カードを削除演出、削除
        RemoveCard(card);

        return parameter;
    }
    
    /// <summary>
    /// AI からの返答（JSON）を受け取り、パラメーターを更新して UI に表示します
    /// </summary>
    /// <param name="responseJson">AI から返ってきた JSON 文字列</param>
    private void OnAIResponse(string responseJson)
    {
        try
        {
            // 1. レスポンスをパース
            AIResponseData data = JsonUtility.FromJson<AIResponseData>(responseJson);
            // data.reply         : AI のテキスト返答
            // data.deltaParameter: AI が提案するパラメーター変更値（デルタ）

            // 2. 会話前の現在パラメーターを取得
            AngelParameter before = angelPresenter.GetAngelParameter();

            // 3. カードごとの倍率を掛け合わせ
            float affMul = 1f, trustMul = 1f, jealMul = 1f, closeMul = 1f;
            foreach (var card in oneTurnUsedCards)
            {
                affMul   *= card.CardData.affectionMultiplier;
                trustMul *= card.CardData.trustMultiplier;
                jealMul  *= card.CardData.jealousyMultiplier;
                closeMul *= card.CardData.closenessMultiplier;
            }

            // 4. AI のデルタに倍率を適用
            AngelParameter adjustedDelta = data.deltaParameter.Multiply(
                affMul, trustMul, jealMul, closeMul
            );

            // 5. 新しいパラメーターを計算（現在値 + 調整済デルタ）
            AngelParameter after = new AngelParameter
            {
                affection = before.affection + adjustedDelta.affection,
                trust     = before.trust     + adjustedDelta.trust,
                jealousy  = before.jealousy  + adjustedDelta.jealousy,
                closeness = before.closeness + adjustedDelta.closeness
            };

            // 6. モデルを更新
            angelPresenter.UpdateAngel(after);

            // 7. UI に AI の返答を表示
            angelTalkView.ShowAITalk(data.reply);

            // 8. ターン終了後のクリア
            setCards.Clear();
            oneTurnUsedCards.Clear();
        }
        catch (Exception ex)
        {
            Debug.LogError($"OnAIResponse 処理中にエラー: {ex}");
        }
    }

    
    //会話内容とカード内容の送信
    private void SendToAITalk(string Json)
    {
        localAIClient.SendToLocalAI(Json, OnAIResponse);
    }

    //使用予定カードに入れる
    private void SetCard(CardBase card)
    {
        foreach (var setcard in setCards)
        {
            if (setcard == card)
            {
                OutCard(card);
                return;
            }
        }
        
        if (setCards.Count >= 3)
        {
            Debug.Log("使用カードは３枚までです。");
            return;
        }
        
        Debug.Log($"セットしました。{card}");
        setCards.Add(card);
    }
    
    //使用予定カードから抜く
    private void OutCard(CardBase card)
    {
        Debug.Log($"抜きました{card}");
        setCards.Remove(card);
    }

    //カード追加時の演出
    public void AddCard(CardScriptableObject cardDate)
    {
        var card = cardFactory.CreateCard(cardDate, view.CardParent);
        
        //カード生成用
        model.AddCard(card);
        
        view.ConfigCard(model.CurrentHoldCard.Value);
    }
    
    //カード削除
    public void RemoveCard(CardBase cardDate)
    {
        model.RemoveCard(cardDate);
        
        view.RemoveCard(cardDate);
        
        view.ConfigCard(model.CurrentHoldCard.Value);
    }
    
    //ランダムでチョイス
    public CardScriptableObject SelectRandomCard(List<CardScriptableObject> cards)
    {
        return cards[Random.Range(0, cards.Count)];
    }

    //手札総入れ替え
    public void HandSwap()
    {
        for (int i = 0; i > model.CurrentHoldCard.Value.Count; i++)
        {
            AddCard(SelectRandomCard(CardPool.Instance.cardpool));
        }
    }

    //選択したカードタイプを選択
    public List<CardScriptableObject> CollectTargetCardType(List<CardScriptableObject.cardTypes> cardType)
    {
        List<CardScriptableObject> targetCardList = new List<CardScriptableObject>();
        foreach (var card in CardPool.Instance.cardpool)
        {
            foreach (var Type in cardType)
            {
                if (card.cardType == Type)
                {
                    targetCardList.Add(card);
                    break;
                }
            }
        }
        return targetCardList;
    }
    
    //手札にある選択したカードタイプを選択
    public List<CardBase> CollectTargetCardTypeHoldCards(CardScriptableObject.cardTypes cardType)
    {
        List<CardBase> targetCardList = new List<CardBase>();
        
        foreach (var card in model.CurrentHoldCard.Value)
        {
            if (card.CardData.cardType == cardType)
            {
                targetCardList.Add(card);
            }
        }
        
        return targetCardList;
    }
    

    //アクションポイントを増加
    public void AddActionPoint(int actionPoint)
    {
        model.AddActionPoint(actionPoint);
    }
    
    //デートモードに以降
    public void GoToDate()
    {
        IsDate.Value = true;
    }

    public void FinishDate()
    {
        IsDate.Value = false;
    }
    
    //告白成功
    public void BeCouple()
    {
        isCouple.Value = true;
    }
    
    //ゲーム開始時のドロー
    public void StartDrawCards(List<CardScriptableObject.cardTypes> cardData,int drawCount)
    {
        for (int i = 0; i < drawCount; i++)
        {
            AddCard(SelectRandomCard(CollectTargetCardType(cardData)));
        }
    }
    
}