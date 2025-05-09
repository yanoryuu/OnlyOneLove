using UnityEngine;

public class SpecialCard : CardBase
{
    public override AngelParameter PlayCard(CardPlayPresenter presenter,AngelParameter angelParameter)
    {
        return new AngelParameter();
    }
}
