
public class ActionCard : CardBase
{
    public override AngelParameter PlayCard(CardPlayPresenter presenter,AngelParameter angelParameter)
    {
        // 処理はここに記述
        presenter.AngelPresenter.UpdateAngel(_cardData.addParameterNum);
        return new AngelParameter();
    }
}