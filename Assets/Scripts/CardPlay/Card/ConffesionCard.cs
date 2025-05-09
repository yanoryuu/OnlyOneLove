using UnityEngine;

public class ConffesionCard : CardBase
{
    public override AngelParameter PlayCard(CardPlayPresenter presenter,AngelParameter angelParameter)
    {
        return new AngelParameter();
    }
}
