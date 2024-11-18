namespace DeckBuilder;

using Godot;
using Godot.Collections;


public partial class WarriorSlash : Card
{

	public int baseDamage = 4;

	public override void ApplyEffects(Array<Node> targets, ModifierHandler modifiers)
	{
		DamageEffect damageEffect = new()
		{
			amount = modifiers.GetModifiedValue(baseDamage, Modifier.Type.DMG_DEALT),
			sound = sound
		};
		damageEffect.Execute(targets);
	}

}

