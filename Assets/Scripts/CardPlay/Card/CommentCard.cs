using UnityEngine;

public class CommentCard : CardBase
{
    public override AngelParameter PlayCard(CardPlayPresenter presenter,AngelParameter angelParameter)
    {
        return new AngelParameter();
    }
}
