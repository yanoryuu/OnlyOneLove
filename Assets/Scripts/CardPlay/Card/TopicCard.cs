using UnityEngine;

public class TopicCard : CardBase
{
    public override AngelParameter PlayCard(CardPlayPresenter presenter,AngelParameter angelParameter)
    {
        return new AngelParameter();
    }
}
