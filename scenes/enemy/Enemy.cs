using Godot;

namespace DeckBuilder;

public partial class Enemy : Area2D
{

	const int ARROW_OFFSET = 5;
	private Material WHITE_SPRITE_MATERIAL = GD.Load<Material>("res://art/white_sprite_material.tres");

	[Export] public EnemyStats stats {
		get => _stats;
		set => SetEnemyStats(value);
	}
	private EnemyStats _stats;

	public Sprite2D sprite2D;
	public Sprite2D arrow;
	public StatsUI statsUI;
	public IntentUI intentUI;
	public StatusHandler statusHandler;
	public ModifierHandler modifierHandler;

	public EnemyActionPicker enemyActionPicker;
	public EnemyAction currentAction {
		get => _currentAction;
		set => SetCurrentAction(value);
	}
	private EnemyAction _currentAction = null;

	public override void _Ready()
	{
		sprite2D = GetNode<Sprite2D>("Sprite2D");
		arrow = GetNode<Sprite2D>("Arrow");
		statsUI = GetNode<StatsUI>("StatsUI");
		intentUI = GetNode<IntentUI>("IntentUI");
		statusHandler = GetNode<StatusHandler>("StatusHandler");
		modifierHandler = GetNode<ModifierHandler>("ModifierHandler");

		AreaEntered += OnAreaEntered;
		AreaExited += OnAreaExited;

		statusHandler.statusOwner = this;
	}

	public void SetCurrentAction(EnemyAction value)
	{
		_currentAction = value;

		if (currentAction != null)
		{
			intentUI?.UpdateIntent(currentAction.intent);
		}
	}

	public void SetEnemyStats(EnemyStats value)
	{
		_stats = value.CreateInstance();

		if (!value.IsConnected(Stats.SignalName.StatsChanged, new Callable(this, MethodName.UpdateStats)))
		{
			_stats.StatsChanged += UpdateStats;
			_stats.StatsChanged += UpdateAction;
		}

		UpdateEnemy();
	}

	public void UpdateStats()
	{
		statsUI.UpdateStats(stats);
	}

	public void SetupAI()
	{
		enemyActionPicker?.QueueFree();

		EnemyActionPicker newActionPicker = stats.ai.Instantiate() as EnemyActionPicker;
		AddChild(newActionPicker);
		enemyActionPicker = newActionPicker;
		enemyActionPicker.enemy = this;
	}

	public void UpdateAction()
	{
		if (enemyActionPicker == null) {
			return;
		}

		if (currentAction == null)
		{
			currentAction = enemyActionPicker.GetAction();
			return;
		}

		EnemyAction newConditionalAction = enemyActionPicker.GetFirstConditionalAction();
		if (newConditionalAction != null && currentAction != newConditionalAction)
		{
			currentAction = newConditionalAction;
		}
	}

	public async void UpdateEnemy()
	{
		if (stats == null) return;
		if (!IsInsideTree()) await ToSignal(this, "ready");

		sprite2D.Texture = stats.art;
		arrow.Position = Vector2.Right * (sprite2D.GetRect().Size.X / 2 + ARROW_OFFSET); // Arrow is X pixels to the right of the sprite
		SetupAI();
		UpdateStats();
	}

	public void DoTurn()
	{
		stats.block = 0;

		if (currentAction == null) return;

		currentAction.PerformAction();
	}

	public void TakeDamage(int damage, Modifier.Type whichModifier)
	{
		if (stats.health <= 0) return;

		sprite2D.Material = WHITE_SPRITE_MATERIAL;
		int modifiedDamage = modifierHandler.GetModifiedValue(damage, whichModifier);

		Tween tween = CreateTween();
		tween.TweenCallback(Callable.From(() => Shaker.Instance.Shake(this, 16f, 0.15f)));
		tween.TweenCallback(Callable.From(() => stats.TakeDamage(modifiedDamage)));
		tween.TweenInterval(0.17f);
		
		tween.Finished += () => {
			sprite2D.Material = null;
			if (stats.health <= 0)
			{
				Events.Instance.EmitSignal(Events.SignalName.EnemyDied, this);
				QueueFree(); // Remove the enemy from the scene
			}
		};
	}

	public void OnAreaEntered(Area2D area)
	{
		arrow.Show();
	}

	public void OnAreaExited(Area2D area)
	{
		arrow.Hide();
	}

}
