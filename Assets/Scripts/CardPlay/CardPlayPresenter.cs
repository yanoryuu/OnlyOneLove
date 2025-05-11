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
    
    [SerializeField] private CardPlayView cardPlayView;
    
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
    
    [SerializeField] private List<CardScriptableObject.cardTypes> startTopicCardPool = new List<CardScriptableObject.cardTypes>();
    //ゲーム開始時にカードを引く枚数
    [SerializeField] private int startGetCardNum = 5;
    
    [SerializeField] private int startGetTopicCardNum = 2;
    
    private List<CardBase> oneTurnUsedCards = new List<CardBase>();

    private void Start()
    {
        
        model = new CardPlayModel();
        isProcessing = new ReactiveProperty<bool>();
        isDate = new ReactiveProperty<bool>();
        Bind();
        
        //ゲーム開始時のドロー
        StartDrawCards(startCardPool,startGetCardNum);
        
        DrawTopicCards(startTopicCardPool, startGetTopicCardNum);
    }

    private void Bind()
    {
        model.OnAddCard
            .Subscribe(cardData =>
            {
                cardData.cardButton.OnClickAsObservable()
                    .Where(_ => InGameManager.Instance.CurrentState.Value == InGameEnum.GameState.PlayerTurn || InGameManager.Instance.CurrentState.Value == InGameEnum.GameState.ChooseTopic)
                    .Subscribe(x =>
                    {
                        Debug.Log($"Select {x.ToString()}");
                        OnCardClicked(cardData);
                    })
                    .AddTo(cardData);

                // ビューに追加
                cardPlayView.AddCard(cardData);
                Debug.Log($"{cardData}のカードを追加しました。");
            })
            .AddTo(this);
        
        model.OnAddTopicCard
            .Subscribe(cardData =>
            {
                cardData.cardButton.OnClickAsObservable()
                    .Where(_ => InGameManager.Instance.CurrentState.Value == InGameEnum.GameState.PlayerTurn || InGameManager.Instance.CurrentState.Value == InGameEnum.GameState.ChooseTopic)
                    .Subscribe(x =>
                    {
                        Debug.Log($"Select {x.ToString()}");
                        OnTopicCardClicked(cardData);
                    })
                    .AddTo(cardData);

                // ビューに追加
                chooseTopicView.AddTopicCard(cardData);
                Debug.Log($"{cardData}のカードを追加しました。");
            })
            .AddTo(this);
        
        // 会話送信ボタン
        cardPlayView.TalkButton.OnClickAsObservable()
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
                    RemoveCard(card);
                }

                // c. JSONを組み立てて送信
                string json = AIPromptBuilder.BuildPromptJson(
                    cardPlayView.TalkInputField.text,
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
        
        chooseTopicView.SetTopicButton.OnClickAsObservable()
            .Where(_=>model.TalkTopic!=null)
            .Subscribe(_ => model.SetTalkTopic(currentSelectedCard))
            .AddTo(this);
    }
    
    public void OnCardClicked(CardBase card)
    {
        if (SetCards.Contains(card))
        {
            // 再クリックで外す
            SetCards.Remove(card);
            card.transform.SetParent(cardPlayView.CardParent); // 元の親に戻す
            card.transform.localPosition = Vector3.zero;
            Debug.Log("カードをセットから外しました");
        }
        else
        {
            if (SetCards.Count >= 3)
            {
                Debug.Log("3枚までしかセットできません");
                return;
            }

            SetCards.Add(card);
            card.transform.SetParent(cardPlayView.SetCardArea); // セット枠のUIに移動
            card.transform.localPosition = Vector3.zero;
            Debug.Log("カードをセットしました");
        }

        // 表示更新
        cardPlayView.ConfigCard(model.CurrentHoldCard.Value);
        cardPlayView.ConfigSetCard(setCards);
    }

    public void OnTopicCardClicked(CardBase card)
    {
        if (model.TalkTopic == null)
        {
            model.SetTalkTopic(card);
            card.transform.SetParent(chooseTopicView.SetTopicCardParent.transform);
            card.transform.localPosition = Vector3.zero;
            chooseTopicView.SetTopicCard(card);
            
            Debug.Log("会話カードをセット");
            
        }else if (model.TalkTopic.Value == card)
        {
            var lastTopic = model.ResetTalkTopic();
            lastTopic.transform.SetParent(chooseTopicView.TopicCardParent.transform);
            Debug.Log("会話カードを外した");
        }
        else
        {
            var lastTopic = model.ResetTalkTopic();
            lastTopic.transform.SetParent(chooseTopicView.TopicCardParent.transform);
            model.SetTalkTopic(card);
            card.transform.SetParent(chooseTopicView.SetTopicCardParent.transform);
            card.transform.localPosition = Vector3.zero;
            chooseTopicView.SetTopicCard(card);
            Debug.Log("会話カード入れ替え");
        }
        // 表示更新
        var topicCards = model.CurrentHoldTopicCard?.Value;
        if (topicCards != null)
        {
            chooseTopicView.ConfigCard(topicCards);
        }

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
    /*private AngelParameter PlayCard(CardBase card,AngelParameter aiOfAngelParameter)
    {
        Debug.Log($"使用：{card}");
        
        //コストの増減など
        model.PlayCard(card);
        
        //見た目
        cardPlayView.PlayCard(card);
        
        //実際の効果
        var parameter = card.PlayCard(this, aiOfAngelParameter);
        
        //追加効果
        if (card.CardData.additionalEffect != null) card.PlayAdditionalEffect(this);
        
        //カードを削除演出、削除
        RemoveCard(card);

        return parameter;
    }*/
    
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
        // localAIClient.SendToLocalAI(Json, OnAIResponse);
        
        Debug.Log("AI送信は未実装。代わりにダミーの返答を表示します。");

        // 仮の感情変化（小さな変化）
        AngelParameter dummyDelta = new AngelParameter
        {
            affection = 2f,
            trust = 1f,
            jealousy = 0f,
            closeness = 1f
        };

        // 仮のAIテキスト返答
        AIResponseData dummyResponse = new AIResponseData
        {
            reply = "うん、なんだか嬉しいかも。ありがとう。",
            deltaParameter = dummyDelta
        };

        // JSON形式にして再利用（本来のAI応答と同じ処理で動くように）
        string fakeJson = JsonUtility.ToJson(dummyResponse);
        OnAIResponse(fakeJson);
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
        var card = cardFactory.CreateCard(cardDate, cardPlayView.CardParent);
        
        //カード生成用
        model.AddCard(card);
        
        cardPlayView.ConfigCard(model.CurrentHoldCard.Value);
    }

    public void AddTopicCard(CardScriptableObject cardDate)
    {
        var card = cardFactory.CreateCard(cardDate, chooseTopicView.TopicCardParent.transform);
        model.AddTopic(card);

        var topicCards = model.CurrentHoldTopicCard?.Value;
        if (topicCards != null)
        {
            chooseTopicView.ConfigCard(topicCards);
        }
    }

    
    //カード削除
    public void RemoveCard(CardBase cardDate)
    {
        Debug.Log($"削除{cardDate}");
        model.RemoveCard(cardDate);
        
        cardPlayView.RemoveCard(cardDate);
        
        cardPlayView.ConfigCard(model.CurrentHoldCard.Value);
    }
    
    //ランダムでチョイス
    public CardScriptableObject SelectRandomCard(List<CardScriptableObject> cards)
    {
        return cards[Random.Range(0, cards.Count)];
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
    
    //ゲーム開始時のドロー
    public void StartDrawCards(List<CardScriptableObject.cardTypes> cardData,int drawCount)
    {
        for (int i = 0; i < drawCount; i++)
        {
            AddCard(SelectRandomCard(CollectTargetCardType(cardData)));
        }
    }

    public void DrawTopicCards(List<CardScriptableObject.cardTypes> cardData, int drawCount)
    {
        for (int i = 0; i < drawCount; i++)
        {
            AddTopicCard(SelectRandomCard(CollectTargetCardType(cardData)));
        }
    }
}