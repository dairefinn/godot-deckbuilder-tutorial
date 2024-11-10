namespace DeckBuilder;

using Godot;

public partial class Battle : Node2D
{

	[Export] public CharacterStats charStats;
	[Export] public AudioStream music;

	public BattleUI battleUI;
	public PlayerHandler playerHandler;
	public EnemyHandler enemyHandler;
	public Player player;

	public override void _Ready()
	{
		battleUI = GetNode<BattleUI>("BattleUI");
		playerHandler = GetNode<PlayerHandler>("PlayerHandler");
		enemyHandler = GetNode<EnemyHandler>("EnemyHandler");
		player = GetNode<Player>("Player");
		
		// Normally, we would do this on a 'Run' level so we keep our health, gold and deck between battles
		CharacterStats newStats = charStats.CreateInstance();
		battleUI.charStats = newStats;
		player.stats = newStats;

		enemyHandler.ChildOrderChanged += OnEnemiesChildOrderChanged;
		Events.Instance.PlayerDied += OnPlayerDied;
		Events.Instance.EnemyTurnEnded += OnEnemyTurnEnded;
		Events.Instance.PlayerTurnEnded += playerHandler.EndTurn;
		Events.Instance.PlayerHandDiscarded += enemyHandler.StartTurn;

		StartBattle(newStats);
	}

	public void StartBattle(CharacterStats stats)
	{
		GetTree().Paused = false;
		SoundPlayer.TryPlayOnInstance("MusicPlayer", music, true);
		enemyHandler.ResetEnemyActions();
		playerHandler.StartBattle(stats);
		battleUI.InitializeCardPileUI();
	}

	public void OnEnemyTurnEnded()
	{
		playerHandler.StartTurn();
		enemyHandler.ResetEnemyActions();
	}

	public void OnEnemiesChildOrderChanged()
	{
		if (enemyHandler.GetChildCount() == 0)
		{
			// battleUI.OnEndTurnButtonPressed();
			Events.Instance.EmitSignal(Events.SignalName.BattleOverScreenRequested, "Victory!", (int)BattleOverPanel.Type.WIN);
		}
	}

	public void OnPlayerDied()
	{
		Events.Instance.EmitSignal(Events.SignalName.BattleOverScreenRequested, "You lose!", (int)BattleOverPanel.Type.LOSE);
	}

}
