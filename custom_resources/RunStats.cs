namespace DeckBuilder;

using Godot;

[GlobalClass]
public partial class RunStats : Resource
{

    public static readonly int STARTING_GOLD = 70;
    public static readonly float BASE_CARD_REWARDS = 3;
    public static readonly float BASE_COMMON_WEIGHT = 6.0f;
    public static readonly float BASE_UNCOMMON_WEIGHT = 3.7f;
    public static readonly float BASE_RARE_WEIGHT = 0.3f;

    [Signal] public delegate void GoldChangedEventHandler();

    [Export] public int gold {
        get => _gold;
        set => SetGold(value);
    }
    private int _gold = STARTING_GOLD;

    [Export] public float cardRewards = BASE_CARD_REWARDS;
    [Export(PropertyHint.Range, "0.0, 10.0")] public float commonWeight = BASE_COMMON_WEIGHT;
    [Export(PropertyHint.Range, "0.0, 10.0")] public float uncommonWeight = BASE_UNCOMMON_WEIGHT;
    [Export(PropertyHint.Range, "0.0, 10.0")] public float rareWeight = BASE_RARE_WEIGHT;

    public void SetGold(int value)
    {
        _gold = value;
        EmitSignal(RunStats.SignalName.GoldChanged);
    }

    public void ResetWeights()
    {
        commonWeight = BASE_COMMON_WEIGHT;
        uncommonWeight = BASE_UNCOMMON_WEIGHT;
        rareWeight = BASE_RARE_WEIGHT;
    }

}
