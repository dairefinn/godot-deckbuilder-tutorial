namespace DeckBuilder;

using Godot;

public partial class EnemyActionPicker : Node
{
	
	[Export] public Enemy enemy {
		get => _enemy;
		set => SetEnemy(value);
	}
	private Enemy _enemy;

	[Export] public Node2D target {
		get => _target;
		set => SetTarget(value);
	}
	private Node2D _target;

	public float totalWeight;

	public override void _Ready()
	{
		target = GetTree().GetFirstNodeInGroup("player") as Node2D;
		SetupChances();
	}

	public void SetEnemy(Enemy enemy)
	{
		_enemy = enemy;

		foreach (Node child in GetChildren())
		{
			if (child is not EnemyAction action) continue;
			action.enemy = enemy;
		}
	}

	public void SetTarget(Node2D target)
	{
		_target = target;

		foreach (Node child in GetChildren())
		{
			if (child is not EnemyAction action) continue;
			action.target = target;
		}
	}

	public EnemyAction GetAction()
	{
		EnemyAction action = GetFirstConditionalAction();
		if (action != null) return action;

		return GetChanceBasedAction();
	}

	public EnemyAction GetFirstConditionalAction()
	{
		foreach (Node child in GetChildren())
		{
            if (child is not EnemyAction action) continue;
            if (action.type != EnemyAction.Type.CONDITIONAL) continue;
			if (!action.IsPerformable()) continue;
			return action;
		}

		return null;
	}

	public EnemyAction GetChanceBasedAction()
	{
		float roll = (float)GD.RandRange(0.0f, totalWeight);

		foreach (Node child in GetChildren())
		{
            if (child is not EnemyAction action) continue;
            if (action.type != EnemyAction.Type.CHANCE_BASED) continue;
			if (roll > action.accumulatedWeight) continue;
			return action;
		}

		return null;
	}

	public void SetupChances()
	{
		foreach (Node child in GetChildren())
		{
			if (child is not EnemyAction action) continue;
			if (action.type != EnemyAction.Type.CHANCE_BASED) continue;

			totalWeight += action.chanceWeight;
			action.accumulatedWeight = totalWeight;
		}
	}

}
