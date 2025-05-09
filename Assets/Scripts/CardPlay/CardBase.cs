using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public abstract class CardBase : MonoBehaviour
{
    [Header("カードのUI")]
    [SerializeField] private Button _cardButton;
    public Button cardButton => _cardButton;

    [SerializeField] private Image _cardImage;

    protected CardScriptableObject _cardData;
    public CardScriptableObject CardData => _cardData;

    public virtual void SetCard(CardScriptableObject cardData)
    {
        _cardData = cardData;

        // 見た目を設定
        if (_cardImage != null && _cardData.cardSprite != null)
        {
            _cardImage.sprite = _cardData.cardSprite;
        }
    }

    //追加効果
    public virtual void PlayAdditionalEffect(CardPlayPresenter presenter)
    {
        foreach (var additionalEffect in _cardData.additionalEffect)
        {
            additionalEffect.PlayCard(presenter);
        }
    }
    
    /// <summary>
    /// AI送信用の効果説明とパラメーター変化を返します。
    /// </summary>
    /// <returns>CardEffectEntry</returns>
    public virtual CardEffectEntry GetEffectForAI()
    {
        return new CardEffectEntry
        {
            description = CardData.aiDescription,
            effect = CardData.addParameterNum
        };
    }

    public virtual void ShowCard() => gameObject.SetActive(true);
    public virtual void HideCard() => gameObject.SetActive(false);

    public abstract AngelParameter PlayCard(CardPlayPresenter presenter ,AngelParameter aiOfAngelParameter);
    
}