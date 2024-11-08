namespace DeckBuilder;

using System.Collections.Generic;
using Godot;

public partial class WarriorSlash : Card
{

	public override void ApplyEffects(List<Node> targets)
	{
		DamageEffect damageEffect = new()
		{
			amount = 4,
			sound = sound
		};
		damageEffect.Execute(targets);
	}

}

