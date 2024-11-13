namespace DeckBuilder;

using System.Linq;
using Godot;
using Godot.Collections;


[GlobalClass]
public partial class BattleStatsPool : Resource
{

    [Export] public Array<BattleStats> pool;

    public float[] totalWeightsByTier = new float[] { 0.0f, 0.0f, 0.0f };

    public Array<BattleStats> GetAllBattlesForTier(int tier)
    {
        return new Array<BattleStats>(pool.Where(battle => battle.battleTier == tier).ToArray());
    }

    public void SetupWeightForTier(int tier)
    {
        Array<BattleStats> battles = GetAllBattlesForTier(tier);
        totalWeightsByTier[tier] = 0.0f;

        foreach (BattleStats battle in battles)
        {
            totalWeightsByTier[tier] += battle.weight;
            battle.accumulatedWeight = totalWeightsByTier[tier];
        }
    }

    public BattleStats GetRandomBattleForTier(int tier)
    {
        float roll = (float) GD.RandRange(0.0, totalWeightsByTier[tier]);
        Array<BattleStats> battles = GetAllBattlesForTier(tier);
        
        foreach (BattleStats battle in battles)
        {
            if (battle.accumulatedWeight > roll)
            {
                return battle;
            }
        }

        return null;
    }

    public void Setup()
    {
        for (int i = 0; i < 3; i++)
        {
            SetupWeightForTier(i);
        }
    }

}
