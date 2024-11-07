using Godot;

namespace DeckBuilder;

public partial class Player : Node2D
{

	private Material WHITE_SPRITE_MATERIAL = GD.Load<Material>("res://art/white_sprite_material.tres");

	public Sprite2D sprite2D;
	public StatsUI statsUI;
	
	[Export] public CharacterStats stats {
		get => _stats;
		set => SetStats(value);
	}
	private CharacterStats _stats;

	public override void _Ready()
	{
		sprite2D = GetNode<Sprite2D>("Sprite2D");
		statsUI = GetNode<StatsUI>("StatsUI");
	}

	public void SetStats(CharacterStats value)
	{
		_stats = value;

		if (!value.IsConnected(CharacterStats.SignalName.StatsChanged, Callable.From(UpdateStats)))
		{
			_stats.StatsChanged += UpdateStats;
		}

		UpdatePlayer();
	}

	public async void UpdatePlayer()
	{
		if (stats is null) return;
		if (!IsInsideTree()) await ToSignal(this, "ready");

		sprite2D.Texture = stats.art;
		UpdateStats();
	}

	public void UpdateStats()
	{
		statsUI.UpdateStats(stats);
	}

	public void TakeDamage(int damage)
	{
		if (stats.health <= 0) return;

		sprite2D.Material = WHITE_SPRITE_MATERIAL;

		Tween tween = CreateTween();
		tween.TweenCallback(Callable.From(() => Shaker.Instance.Shake(this, 16f, 0.15f)));
		tween.TweenCallback(Callable.From(() => stats.TakeDamage(damage)));
		tween.TweenInterval(0.17f);
		
		tween.Finished += () => {
			sprite2D.Material = null;

			if (stats.health <= 0)
			{
				Events.Instance.EmitSignal(Events.SignalName.PlayerDied);
				QueueFree(); // Remove the enemy from the scene
			}
		};
	}
}
