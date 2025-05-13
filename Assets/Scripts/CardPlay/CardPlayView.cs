using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPlayView : MonoBehaviour
{
    [SerializeField] private GameObject objCardPlayParent;
    
    [SerializeField] private GameObject cardParent;
    public Transform CardParent => cardParent.transform;

    [SerializeField] private GameObject playerInput;
    
    [SerializeField] private Button talkButton;
    public Button TalkButton => talkButton;
    
    //会話の選択肢
    [SerializeField] private GameObject talkOptionParent;
    
    [SerializeField] private List<TextMeshProUGUI> talkOptionTexts;
    public List<TextMeshProUGUI> TalkOptionTexts => talkOptionTexts;
    
    [SerializeField] private List<Button> talkOptionButton;
    public List<Button> TalkOptionButtons => talkOptionButton;

    //カードの間隔
    [SerializeField] private float spacing = 150f;
    
    public void ShowCard() => cardParent.SetActive(true);
    public void HideCard() => cardParent.SetActive(false);
    
    [SerializeField] private  TMP_InputField talkInputField;
    public TMP_InputField TalkInputField => talkInputField;
    
    [SerializeField] private Transform setCardArea;
    public Transform SetCardArea => setCardArea;

    
    public void AddCard(CardBase card)
    {
        card.ShowCard();
        card.GetComponent<Image>().sprite = card.CardData.cardSprite;
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
        if (cards == null || cards.Count == 0) return;

        float startY = -(cards.Count - 1) * spacing * 0.5f;

        for (int i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            card.transform.localPosition = new Vector3(0, startY + i * spacing, 0);
            card.transform.localRotation = Quaternion.identity;
        }
    }


    public void ConfigSetCard(List<CardBase> cards)
    {
        float startX = -(cards.Count - 1) * spacing * 0.5f;
        
        for (int i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            card.transform.localPosition = new Vector3(startX + i * spacing, 0, 0);
            card.transform.localRotation = Quaternion.identity;
        }
    }
    
    public void Show()
    {
        objCardPlayParent.SetActive(true);
    }
    
    public void Hide()
    {
        objCardPlayParent.SetActive(false);
    }

    public void ShowTalkOption()
    {
        talkOptionParent.SetActive(true);
        playerInput.SetActive(false);
    }

    public void HideTalkOption()
    {
        talkOptionParent.SetActive(false);
        playerInput.SetActive(true);
    }

    //ターンが始まった時に初期化
    public void Initialize()
    {
        ShowTalkOption();
    }
}