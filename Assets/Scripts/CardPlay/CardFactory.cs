using UnityEngine;

public class CardFactory : MonoBehaviour
{
    [Header("Card Prefabs")]
    [SerializeField] private ActionCard actionPrefab;
    [SerializeField] private CommentCard commentCardPrefab;
    [SerializeField] private SpecialCard specialCardPrefab;
    [SerializeField] private TopicCard topicCardPrefab;
    [SerializeField] private ConffesionCard conffesionCardPrefab;
    [SerializeField] private PsychicCard psychicCardPrefab;
    public CardBase CreateCard(CardScriptableObject date, Transform parent)
    {
        CardBase prefab = date.cardType switch
        {
            CardScriptableObject.cardTypes.Topic => topicCardPrefab,
            CardScriptableObject.cardTypes.Confession => conffesionCardPrefab,
            CardScriptableObject.cardTypes.Special => specialCardPrefab,
            CardScriptableObject.cardTypes.Psychic => psychicCardPrefab,
            CardScriptableObject.cardTypes.Action => actionPrefab,
            CardScriptableObject.cardTypes.Comment => commentCardPrefab,
            _ => null
        };

        if (prefab == null)
        {
            Debug.LogError("Card prefab not found for type: " + date.cardType);
            return null;
        }

        var card = Instantiate(prefab, parent.position, parent.rotation, parent);
        card.SetCard(date);
        return card;
    }
}
