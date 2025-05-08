using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPlayView : MonoBehaviour
{
    [SerializeField] private GameObject cardParent;
    public Transform CardParent => cardParent.transform;
    
    [SerializeField] private Button setButton;
    public Button SetButton => setButton;
    
    [SerializeField] private Button talkButton;
    public Button TalkButton => talkButton;

    //カードの感覚
    [SerializeField] private float spacing = 150f;
    
    public void ShowCard() => cardParent.SetActive(true);
    public void HideCard() => cardParent.SetActive(false);

    [SerializeField] private TextMeshProUGUI restCardsText;
    
    public void AddCard(CardBase card)
    {
        card.ShowCard();
    }

    //カードをプレイするときの演出はここに
    public void PlayCard(CardBase card)
    {
        
    }

    public void RemoveCard(CardBase card)
    {
        Destroy(card.gameObject);
    }

    public void ReturnCard(CardBase card)
    {
        card.HideCard();
    }

    public void ConfigCard(List<CardBase> cards)
    {
        // 親オブジェクトの中心からカードを並べるイメージ
        float startX = -(cards.Count - 1) * spacing * 0.5f; // 最初のカードのX位置

        for (int i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            card.transform.localPosition = new Vector3(startX + i * spacing, 0, 0); // 横に並べる
            card.transform.localRotation = Quaternion.identity; // 回転リセット（必要なら）
        }
    }
    
    public void SetRestCards(int restCards)
    {
        restCardsText.text = $"RestCards: {restCards}";
    }
}