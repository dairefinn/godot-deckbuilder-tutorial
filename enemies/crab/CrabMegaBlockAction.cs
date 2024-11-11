namespace DeckBuilder;

using Godot;

public partial class CrabMegaBlockAction : EnemyAction
{

    [Export] public int hp_threshold = 6;
    [Export] public int block = 15;

    public bool alreadyUsed = false;

    public override void PerformAction()
    {
        base.PerformAction();

        BlockEffect blockEffect = new()
        {
            amount = block,
            sound = sound
        };
        blockEffect.Execute(new() { enemy });

        GetTree().CreateTimer(0.6f, false).Timeout += () => {
            Events.Instance.EmitSignal(Events.SignalName.EnemyActionCompleted, enemy);
        };
    }

    public override bool IsPerformable()
    {
        if (enemy == null) return false;
        if (alreadyUsed) return false;

        bool isLow = enemy.stats.health <= hp_threshold;
        alreadyUsed = isLow;

        if (target == null) return false;

        return isLow;
    }

}
