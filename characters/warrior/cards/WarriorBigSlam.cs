namespace DeckBuilder;

using Godot;
using Godot.Collections;

public partial class WarriorBigSlam : Card
{

	public static readonly Status EXPOSED_STATUS = GD.Load<Status>("res://statuses/exposed.tres");

	public int baseDamage = 4;
	public int exposedDuration = 2;

	public override void ApplyEffects(Array<Node> targets, ModifierHandler modifiers)
	{
		DamageEffect damageEffect = new()
		{
			amount = modifiers.GetModifiedValue(baseDamage, Modifier.Type.DMG_DEALT),
			sound = sound
		};
		damageEffect.Execute(targets);

		Status exposed = EXPOSED_STATUS.Duplicate() as Status;
		exposed.duration = exposedDuration;

		StatusEffect statusEffect = new()
		{
			status = exposed,
		};
		statusEffect.Execute(targets);
	}

}

