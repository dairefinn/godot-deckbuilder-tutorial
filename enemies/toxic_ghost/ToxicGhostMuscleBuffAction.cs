namespace DeckBuilder;

using Godot;
using Godot.Collections;


public partial class ToxicGhostMuscleBuffAction : EnemyAction
{

	public static readonly MuscleStatus MUSCLE_STATUS = GD.Load<MuscleStatus>("res://statuses/muscle.tres");

	[Export] public int stacksPerAction = 2;

	public int healthThreshold = 25;
	public int usages = 0;

    public override bool IsPerformable()
    {
		bool hpUnderThreshold = enemy.stats.health < healthThreshold;

		if (usages == 0 || (usages == 1 && hpUnderThreshold))
		{
			usages += 1;
			return true;
		}

		return false;
    }

    public override void PerformAction()
	{
		if (enemy == null) return;
		if (target == null) return;

		StatusEffect statusEffect = new();
		MuscleStatus muscle = MUSCLE_STATUS.Duplicate() as MuscleStatus;
		muscle.stacks = stacksPerAction;
		statusEffect.status = muscle;
		Array<Node> enemyArray = new() { enemy };
		statusEffect.Execute(enemyArray);

		SoundPlayer.TryPlayOnInstance("SFXPlayer", sound);

		Events.Instance.EmitSignal(Events.SignalName.EnemyActionCompleted, enemy);
	}

}

