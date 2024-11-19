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

    public override string GetDefaultTooltip()
    {
        return string.Format(tooltipText, baseDamage);
    }

    public override string GetUpdatedTooltip(ModifierHandler playerModifiers, ModifierHandler enemyModifiers)
    {
        int modifiedDamage = playerModifiers.GetModifiedValue(baseDamage, Modifier.Type.DMG_DEALT);

		if (enemyModifiers != null)
		{
			modifiedDamage = enemyModifiers.GetModifiedValue(modifiedDamage, Modifier.Type.DMG_TAKEN);
		}

		return string.Format(tooltipText, modifiedDamage);
    }

}

