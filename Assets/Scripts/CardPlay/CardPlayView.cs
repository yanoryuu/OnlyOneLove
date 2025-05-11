using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPlayView : MonoBehaviour
{
    [SerializeField] private GameObject objCardPlayView;
    public GameObject ObjCardPlayView => objCardPlayView;
    
    [SerializeField] private GameObject cardParent;
    public Transform CardParent => cardParent.transform;

    [SerializeField] private GameObject playerInput;
    public GameObject PlayerInput => playerInput;
    
    [SerializeField] private Button talkButton;
    public Button TalkButton => talkButton;

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
        objCardPlayView.SetActive(true);
    }
    
    public void Hide()
    {
        objCardPlayView.SetActive(false);
    }
}