namespace DeckBuilder;

using Godot;
using Godot.Collections;


public partial class WarriorBigSlam : Card
{

	public override void ApplyEffects(Array<Node> targets)
	{
		DamageEffect damageEffect = new()
		{
			amount = 10,
			sound = sound
		};
		damageEffect.Execute(targets);
		GD.Print("WarriorBigSlam card has been played");
	}

}

