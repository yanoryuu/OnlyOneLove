using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseTopicView : MonoBehaviour
{
    [SerializeField] private Button setTopicButton;
    public Button SetTopicButton => setTopicButton;

    [SerializeField] private GameObject chooseTopicUI;
    public GameObject ChooseTopicUI => chooseTopicUI;
    
    [SerializeField] private GameObject topicCardParent;
    public GameObject TopicCardParent => topicCardParent;
    
    [SerializeField] private GameObject setTopicCardParent;
    public GameObject SetTopicCardParent => setTopicCardParent;
    
    //カードの間隔
    [SerializeField] private float spacing = 150f;

    public void Show()
    {
        chooseTopicUI.SetActive(true);
    }

    public void Hide()
    {
        chooseTopicUI.SetActive(false);
    }
    public void AddTopicCard(CardBase card)
    {
        card.ShowCard();
    }
    
    public void SetTopicCard(CardBase cards)
    {
       
    }

    public void RemoveTopicCard(CardBase cards)
    {
        
    }

    public void ConfigCard(List<CardBase> cards)
    {
        float startX = -(cards.Count - 1) * spacing * 0.5f;
        
        for (int i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            card.transform.localPosition = new Vector3(startX + i * spacing, 0, 0);
            card.transform.localRotation = Quaternion.identity;
        }
    }
    
}
