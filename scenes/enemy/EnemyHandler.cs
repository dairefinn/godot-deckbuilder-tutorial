namespace DeckBuilder;

using Godot;
using Godot.Collections;

public partial class EnemyHandler : Node2D
{

	public Array<Enemy> actingEnemies = new();

    public override void _Ready()
    {
		Events.Instance.EnemyDied += OnEnemyDied;
		Events.Instance.EnemyActionCompleted += OnEnemyActionCompleted;
		Events.Instance.PlayerHandDrawn += OnPlayerHandDrawn;
    }

	public void ResetEnemyActions()
	{
		if (!IsInstanceValid(this)) return;

		foreach (Node enemyNode in GetChildren())
		{
			if (enemyNode is not Enemy enemy) continue;
			enemy.currentAction = null;
			enemy.UpdateAction();
		}
	}

	public void StartTurn()
	{
		if (!IsInstanceValid(this)) return;
		if (GetChildCount() == 0) return;

		actingEnemies.Clear();

		foreach (Node enemyNode in GetChildren())
		{
			if (enemyNode is not Enemy enemy) continue;
			actingEnemies.Add(enemy);
		}

		StartNextEnemyTurn();
	}

	public void OnEnemyActionCompleted(Enemy enemy)
	{
		enemy.statusHandler.ApplyStatusesByType(Status.Type.END_OF_TURN);
	}

	public void SetupEnemies(BattleStats battleStats)
	{
		if (battleStats == null) return;

		foreach(Node enemyNode in GetChildren())
		{
			if (enemyNode is not Enemy enemy) continue;
			enemy.QueueFree();
		}

		Node allNewEnemies = battleStats.enemies.Instantiate<Node>();

		foreach (Node enemyNode in allNewEnemies.GetChildren())
		{
			if (enemyNode is not Enemy enemy) continue;
			Enemy newEnemyChild = enemy.Duplicate() as Enemy;
			AddChild(newEnemyChild);
			newEnemyChild.statusHandler.StatusesApplied += (type) => OnEnemyStatusesApplied(type, newEnemyChild);
		}

		allNewEnemies.QueueFree();
	}

	public void StartNextEnemyTurn()
	{
		if (actingEnemies.Count == 0)
		{
			Events.Instance.EmitSignal(Events.SignalName.EnemyTurnEnded);
			return;
		}

		actingEnemies[0].statusHandler.ApplyStatusesByType(Status.Type.START_OF_TURN);
	}

	public void OnEnemyStatusesApplied(Status.Type type, Enemy enemy)
	{
		switch (type)
		{
			case Status.Type.START_OF_TURN:
				enemy.DoTurn();
				break;
			case Status.Type.END_OF_TURN:
				actingEnemies.Remove(enemy);
				StartNextEnemyTurn();
				break;
		}
	}

	public void OnEnemyDied(Enemy enemy)
	{
		bool isEnemyTurn = actingEnemies.Count > 0;
		actingEnemies.Remove(enemy);

		if (isEnemyTurn)
		{
			StartNextEnemyTurn();
		}
	}

	public void OnPlayerHandDrawn()
	{
		foreach (Node enemyNode in GetChildren())
		{
			if (enemyNode is not Enemy enemy) continue;
			enemy.UpdateIntent();
		}
	}

}
