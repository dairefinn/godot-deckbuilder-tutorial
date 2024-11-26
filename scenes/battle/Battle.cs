namespace DeckBuilder;

using Godot;

public partial class Battle : Node2D
{

	[Export] public BattleStats battleStats;
	[Export] public CharacterStats charStats;
	[Export] public AudioStream music;
	[Export] public RelicHandler relics;

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

		enemyHandler.ChildOrderChanged += OnEnemiesChildOrderChanged;
		Events.Instance.PlayerDied += OnPlayerDied;
		Events.Instance.EnemyTurnEnded += OnEnemyTurnEnded;
		Events.Instance.PlayerTurnEnded += playerHandler.EndTurn;
		Events.Instance.PlayerHandDiscarded += enemyHandler.StartTurn;
	}

    public void StartBattle()
	{
		GetTree().Paused = false;
		SoundPlayer.TryPlayOnInstance("MusicPlayer", music, true);

		battleUI.charStats = charStats;
		player.stats = charStats;
		playerHandler.relics = relics;
		enemyHandler.SetupEnemies(battleStats);
		enemyHandler.ResetEnemyActions();

		relics.RelicsActivated += OnRelicsActivated;
		relics.ActivateRelicsByType(Relic.Type.START_OF_COMBAT);
	}

	public void OnRelicsActivated(Relic.Type type)
	{
		switch (type)
		{
			case Relic.Type.START_OF_COMBAT:
				playerHandler.StartBattle(charStats);
				battleUI.InitializeCardPileUI();
				break;
			case Relic.Type.END_OF_COMBAT:
				Events.Instance.EmitSignal(Events.SignalName.BattleOverScreenRequested, "Victory!", (int)BattleOverPanel.Type.WIN);
				break;
		}
	}

	public void OnEnemyTurnEnded()
	{
		playerHandler.StartTurn();
		enemyHandler.ResetEnemyActions();
	}

	public void OnEnemiesChildOrderChanged()
	{
		if (enemyHandler.GetChildCount() != 0) return;
		if (!IsInstanceValid(relics)) return;
		relics.ActivateRelicsByType(Relic.Type.END_OF_COMBAT);
	}

	public void OnPlayerDied()
	{
		Events.Instance.EmitSignal(Events.SignalName.BattleOverScreenRequested, "You lose!", (int)BattleOverPanel.Type.LOSE);
		SaveGame.DeleteData();
	}

}
