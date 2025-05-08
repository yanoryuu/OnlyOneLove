using System;

[Serializable]
public abstract class AdditionalEffect
{
    public string effectName = "New Effect"; // 表示用の名前
    public abstract void PlayCard(CardPlayPresenter presenter);
}
