namespace DeckBuilder;

using Godot;
using Godot.Collections;


public partial class TrueStrengthFormStatus : Status
{

    public static readonly Status MUSCLE_STATUS = GD.Load<Status>("res://statuses/muscle.tres");

    public int stacksPerTurn = 2;

    public override void ApplyStatus(Node target)
	{
		GD.Print("Applied true strength form");

        MuscleStatus muscle = MUSCLE_STATUS.Duplicate() as MuscleStatus;
        muscle.stacks = stacksPerTurn;
		StatusEffect statusEffect = new()
		{
			status = muscle
		};
		statusEffect.Execute(new Array<Node> { target });

		EmitSignal(SignalName.StatusApplied, this);
	}

}

