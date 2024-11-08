namespace DeckBuilder;

using System.Collections.Generic;
using Godot;

public partial class BatBlockAction : EnemyAction
{

	[Export] public int block;

	public override void PerformAction()
	{
		if (enemy == null) return;
		if (target == null) return;

		BlockEffect blockEffect = new()
		{
			amount = block,
			sound = sound
		};
		List<Node> enemyArray = new() { enemy };
		blockEffect.Execute(enemyArray);

		SoundPlayer.TryPlayOnInstance("SFXPlayer", sound);

		Events.Instance.EmitSignal(Events.SignalName.EnemyActionCompleted, enemy);
	}
}

