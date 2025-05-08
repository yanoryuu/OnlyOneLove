using R3;

public class PlayerParameterRuntime
{
    public ReactiveProperty<int> Brave = new();
    public ReactiveProperty<int> Charm = new();
    public ReactiveProperty<int> Strength = new();

    public ReactiveProperty<int> ActionPoint = new(); // ← 追加

    public PlayerParameterRuntime() {}

    public PlayerParameterRuntime(PlayerParameter source)
    {
        LoadFrom(source);
    }

    public void LoadFrom(PlayerParameter source)
    {
        Brave.Value = source.Brave;
        Charm.Value = source.Charm;
        Strength.Value = source.Strength;
    }

    public PlayerParameter ToParameter()
    {
        return new PlayerParameter
        {
            Brave = Brave.Value,
            Charm = Charm.Value,
            Strength = Strength.Value,
        };
    }
}