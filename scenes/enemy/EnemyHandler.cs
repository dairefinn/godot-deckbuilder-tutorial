namespace DeckBuilder;

using Godot;

public partial class EnemyHandler : Node2D
{

    public override void _Ready()
    {
		Events.Instance.EnemyActionCompleted += OnEnemyActionCompleted;
    }

	public void ResetEnemyActions()
	{
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

		Node firstEnemyNode = GetChild(0);
		if (firstEnemyNode == null) return;
		if (!IsInstanceValid(firstEnemyNode)) return;
		if (firstEnemyNode is not Enemy firstEnemy) return;
		firstEnemy.DoTurn();
	}

	public void OnEnemyActionCompleted(Enemy enemy)
	{
		if (enemy.GetIndex() == GetChildCount() - 1)
		{
			Events.Instance.EmitSignal(Events.SignalName.EnemyTurnEnded);
		}

		if (enemy.GetIndex() == GetChildCount() - 1) return;
		Node nextEnemyNode = GetChild(enemy.GetIndex() + 1);
		if (nextEnemyNode == null) return;
		if (nextEnemyNode is not Enemy nextEnemy) return;
		nextEnemy.DoTurn();
	}

}
