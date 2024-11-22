namespace DeckBuilder;

using Godot;
using Godot.Collections;

public partial class ToxicGhostBlockAction : EnemyAction
{

	[Export] public int block = 10;

	public override void PerformAction()
	{
		if (enemy == null) return;
		if (target == null) return;

        BlockEffect blockEffect = new()
        {
            amount = block,
            sound = sound
        };
        Array<Node> enemyArray = new() { enemy };
		blockEffect.Execute(enemyArray);

		GetTree().CreateTimer(0.6f, false).Timeout += () => {
			Events.Instance.EmitSignal(Events.SignalName.EnemyActionCompleted, enemy);
		};
	}

}

