namespace DeckBuilder;

using Godot;

[GlobalClass]
public partial class EnemyStats : Stats
{

    [Export] public PackedScene ai;

    public new EnemyStats CreateInstance()
    {
        return base.CreateInstance() as EnemyStats;
    }

}
