namespace DeckBuilder;

using Godot;

public partial class GoldUI : HBoxContainer
{

    [Export] public RunStats runStats {
        get => _runStats;
        set => SetRunStats(value);
    }
    private RunStats _runStats;

    public Label label;

    public void SetRunStats(RunStats value)
    {
        _runStats = value;

        if (!value.IsConnected(RunStats.SignalName.GoldChanged, Callable.From(UpdateGold)))
        {
            value.Connect(RunStats.SignalName.GoldChanged, Callable.From(UpdateGold));
            UpdateGold();
        }
    }

    public override void _Ready()
    {
        label = GetNode<Label>("Label");
        label.Text = "0";
    }

    public void UpdateGold()
    {
        if (!IsInstanceValid(label)) return;
        label.Text = runStats.gold.ToString();
    }

}
