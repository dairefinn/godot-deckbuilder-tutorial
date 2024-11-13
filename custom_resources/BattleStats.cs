namespace DeckBuilder;

using Godot;

[GlobalClass]
public partial class BattleStats : Resource
{

    [Export(PropertyHint.Range, "0, 2")] public int battleTier;
    [Export(PropertyHint.Range, "0.0, 10.0")] public float weight;
    [Export] public int goldRewardMin;
    [Export] public int goldRewardMax;
    [Export] public PackedScene enemies;

    public float accumulatedWeight = 0.0f;

    public int RollGoldReward()
    {
        return GD.RandRange(goldRewardMin, goldRewardMax);
    }

}
