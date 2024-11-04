using Godot;

namespace DeckBuilder;

public partial class Enemy : Area2D
{

	const int ARROW_OFFSET = 5;

	public Sprite2D sprite2D;
	public StatsUI statsUI;
	public Sprite2D arrow;

	[Export] public Stats stats {
		get => _stats;
		set => SetStats(value);
	}
	private Stats _stats;

	public override void _Ready()
	{
		sprite2D = GetNode<Sprite2D>("Sprite2D");
		statsUI = GetNode<StatsUI>("StatsUI");
		arrow = GetNode<Sprite2D>("Arrow");

		AreaEntered += OnAreaEntered;
		AreaExited += OnAreaExited;
	}

	public void SetStats(Stats value)
	{
		_stats = value.CreateInstance() as Stats;

		if (!value.IsConnected(Stats.SignalName.StatsChanged, Callable.From(UpdateStats)))
		{
			_stats.StatsChanged += UpdateStats;
		}

		UpdateEnemy();
	}

	public async void UpdateEnemy()
	{
		if (stats == null) return;
		if (!IsInsideTree()) await ToSignal(this, "ready");

		sprite2D.Texture = stats.art;
		arrow.Position = Vector2.Right * (sprite2D.GetRect().Size.X / 2 + ARROW_OFFSET); // Arrow is X pixels to the right of the sprite

		UpdateStats();
	}

	public void UpdateStats()
	{
		statsUI.UpdateStats(stats);
	}

	public void TakeDamage(int damage)
	{
		if (stats.health <= 0) return;

		stats.TakeDamage(damage);

		if (stats.health <= 0)
		{
			QueueFree(); // Remove the enemy from the scene
		}
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
