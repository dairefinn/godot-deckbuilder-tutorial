namespace DeckBuilder;

using System.Collections.Generic;
using Godot;

public partial class CrabBlockAction : EnemyAction
{

    [Export] public int block = 6;

    public override void PerformAction()
    {
        base.PerformAction();

        BlockEffect blockEffect = new()
        {
            amount = block,
            sound = sound
        };
        List<Node> enemyArray = new() { enemy };
        blockEffect.Execute(enemyArray);

        GetTree().CreateTimer(0.6f, false).Timeout += () => {
            Events.Instance.EmitSignal(Events.SignalName.EnemyActionCompleted, enemy);
        };
    }

    public override bool IsPerformable()
    {
        if (enemy == null) return false;
        if (target == null) return false;

        return true;
    }

}
