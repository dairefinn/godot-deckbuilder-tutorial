namespace DeckBuilder;

using Godot;

public partial class Battle : Node2D
{

	private Events globalEvents;

	[Export] public CharacterStats charStats;

	public BattleUI battleUI;
	public PlayerHandler playerHandler;
	public Player player;

	public override void _Ready()
	{
		globalEvents = GetNode<Events>("/root/Events");

		battleUI = GetNode<BattleUI>("BattleUI");
		playerHandler = GetNode<PlayerHandler>("PlayerHandler");
		player = GetNode<Player>("Player");
		
		// Normally, we would do this on a 'Run' level so we keep our health, gold and deck between battles
		CharacterStats newStats = charStats.CreateInstance();
		battleUI.charStats = newStats;
		player.stats = newStats;

		globalEvents.PlayerTurnEnded += playerHandler.EndTurn;
		globalEvents.PlayerHandDiscarded += playerHandler.StartTurn;

		StartBattle(newStats);
	}

	public void StartBattle(CharacterStats stats)
	{
		GD.Print("Battle started!");
		playerHandler.StartBattle(stats);
	}

}
