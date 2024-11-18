using Godot;

namespace DeckBuilder;

public partial class Player : Node2D
{

	private Material WHITE_SPRITE_MATERIAL = GD.Load<Material>("res://art/white_sprite_material.tres");

	public Sprite2D sprite2D;
	public StatsUI statsUI;
	public StatusHandler statusHandler;
	public ModifierHandler modifierHandler;
	
	[Export] public CharacterStats stats {
		get => _stats;
		set => SetStats(value);
	}
	private CharacterStats _stats;

	public override void _Ready()
	{
		sprite2D = GetNode<Sprite2D>("Sprite2D");
		statsUI = GetNode<StatsUI>("StatsUI");
		statusHandler = GetNode<StatusHandler>("StatusHandler");
		modifierHandler = GetNode<ModifierHandler>("ModifierHandler");

		statusHandler.statusOwner = this;
	}

	public void SetStats(CharacterStats value)
	{
		_stats = value;

		if (!value.IsConnected(CharacterStats.SignalName.StatsChanged, new Callable(this, MethodName.UpdateStats)))
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
				Events.Instance.EmitSignal(Events.SignalName.PlayerDied);
				QueueFree(); // Remove the enemy from the scene
			}
		};
	}
}
