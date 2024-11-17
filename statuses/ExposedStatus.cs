namespace DeckBuilder;

using Godot;
using Godot.Collections;


public partial class ExposedStatus : Status
{

	public static readonly float MODIFIER = 0.5f;

	public override void ApplyStatus(Node target)
	{
		GD.Print(target + " should take " + MODIFIER + "% more damage");

		DamageEffect damageEffect = new()
		{
			amount = 12
		};
		damageEffect.Execute(new Array<Node> { target });

		EmitSignal(SignalName.StatusApplied, this);
	}

}

