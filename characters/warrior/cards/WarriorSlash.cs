namespace DeckBuilder;

using Godot;
using Godot.Collections;


public partial class WarriorSlash : Card
{

	public override void ApplyEffects(Array<Node> targets)
	{
		DamageEffect damageEffect = new()
		{
			amount = 4,
			sound = sound
		};
		damageEffect.Execute(targets);
	}

}

