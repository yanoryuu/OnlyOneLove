using UnityEngine;

public class PsychicCard : CardBase
{
    public override AngelParameter PlayCard(CardPlayPresenter presenter,AngelParameter angelParameter)
    {
        return new AngelParameter();
    }
}
