using System.Collections.Generic;
using CardGame;
using Ingame;
using UnityEngine;
using R3;
using UnityEngine.UI;

public class CardPlayPresenter : MonoBehaviour
{
    private CardPlayModel model;
    
    [SerializeField] private CardPlayView view;
    
    [SerializeField] private CardFactory cardFactory;
    
    [SerializeField] private AngelPresenter angelPresenter;
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

        view.SetButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                if (currentSelectedCard != null)
                {
                    SetCard(currentSelectedCard);
                }
            })
            .AddTo(this);
        
        view.TalkButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                foreach (var card in SetCards)
                { 
                    PlayCard(card);
                }
            })
            .AddTo(this);
        
        InGameManager.Instance.CurrentState.Where(x => x==InGameEnum.GameState.PlayerTurn)
            .Subscribe(_=>model.Initialize())
            .AddTo(this);
        
        model.CurrentHoldCardIndex.Subscribe(x=>view.SetRestCards(x))
            .AddTo(this);
    }

    //カード使用時の演出
    private void PlayCard(CardBase card, int playActionPoint = 1)
    {
        Debug.Log($"使用：{card}");
        
        //コストの増減など
        model.PlayCard(card,playActionPoint);
        
        //見た目
        view.PlayCard(card);
        
        //実際の効果
        card.PlayCard(this);
        
        //追加効果
        if (card.CardData.additionalEffect != null) card.PlayAdditionalEffect(this);
        
        //カードを削除演出、削除
        RemoveCard(card);
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