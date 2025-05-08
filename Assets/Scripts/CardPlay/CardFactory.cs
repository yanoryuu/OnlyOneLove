using UnityEngine;

public class CardFactory : MonoBehaviour
{
    [Header("Card Prefabs")]
    [SerializeField] private ParameterAddCard parameterChangeCardPrefab;
    [SerializeField] private CostBypassCard costBypassCardPrefab;
    public CardBase CreateCard(CardScriptableObject date, Transform parent)
    {
        CardBase prefab = date.cardType switch
        {
            CardScriptableObject.cardTypes.Talk => parameterChangeCardPrefab,
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
