namespace DeckBuilder;

using Godot;
using Godot.Collections;


public partial class WarriorTrueStrength : Card
{

	public static readonly Status TRUE_STRENGTH_FORM_STATUS = GD.Load<Status>("res://statuses/true_strength_form.tres");

	public override void ApplyEffects(Array<Node> targets, ModifierHandler modifiers)
	{
		Status trueStrength = TRUE_STRENGTH_FORM_STATUS.Duplicate() as Status;

		StatusEffect statusEffect = new()
		{
			status = trueStrength,
		};
		statusEffect.Execute(targets);
	}

}

