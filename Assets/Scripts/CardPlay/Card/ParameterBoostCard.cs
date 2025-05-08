using UnityEngine;

public class ParameterAddCard : CardBase
{
    public override void PlayCard(CardPlayPresenter presenter)
    {
        // 処理はここに記述
        presenter.AngelPresenter.UpdateAngel(_cardData.addParameterNum);
    }
}