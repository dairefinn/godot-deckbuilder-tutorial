namespace DeckBuilder;

using Godot;

public partial class Run : Node
{

	private PackedScene BATTLE_SCENE = GD.Load<PackedScene>("res://scenes/battle/battle.tscn");
	private PackedScene BATTLE_REWARD_SCENE = GD.Load<PackedScene>("res://scenes/battle_reward/battle_reward.tscn");
	private PackedScene CAMPFIRE_SCENE = GD.Load<PackedScene>("res://scenes/campfire/campfire.tscn");
	private PackedScene MAP_SCENE = GD.Load<PackedScene>("res://scenes/map/map.tscn");
	private PackedScene SHOP_SCENE = GD.Load<PackedScene>("res://scenes/shop/shop.tscn");
	private PackedScene TREASURE_SCENE = GD.Load<PackedScene>("res://scenes/treasure/treasure.tscn");

	[Export] RunStartup runStartup;

	public Node CurrentView;
	public CardPileOpener deckButton;
	public CardPileView deckView;
	public GoldUI goldUI;

	public Button BattleButton;
	public Button CampfireButton;
	public Button MapButton;
	public Button RewardsButton;
	public Button ShopButton;
	public Button TreasureButton;

	public RunStats stats;
	public CharacterStats Character;

    public override void _Ready()
    {
		CurrentView = GetNode<Node>("CurrentView");
		deckButton = GetNode<CardPileOpener>("%DeckButton");
		deckView = GetNode<CardPileView>("%DeckView");
		goldUI = GetNode<GoldUI>("%GoldUI");

		BattleButton = GetNode<Button>("%BattleButton");
		CampfireButton = GetNode<Button>("%CampfireButton");
		MapButton = GetNode<Button>("%MapButton");
		RewardsButton = GetNode<Button>("%RewardsButton");
		ShopButton = GetNode<Button>("%ShopButton");
		TreasureButton = GetNode<Button>("%TreasureButton");

		if (runStartup == null)
		{
			return;
		}

		switch(runStartup.type)
		{
			case RunStartup.Type.NEW_RUN:
				Character = runStartup.pickedCharacter.CreateInstance();
				StartRun();
				break;
			case RunStartup.Type.CONTINUED_RUN:
				GD.Print("TODO: Load previous run");
				break;
		}
    }

	public void StartRun()
	{
		stats = new RunStats();
		SetupEventConnections();
		SetupTopBar();
		GD.Print("TODO: Procedurally generate map");
	}

	public void SetupTopBar()
	{
		goldUI.runStats = stats;
		deckButton.cardPile = Character.deck;
		deckView.cardPile = Character.deck;
		deckButton.Pressed += () => deckView.ShowCurrentView("Deck");
	}

	public Node ChangeView(PackedScene scene)
	{
		if (CurrentView.GetChildCount() > 0)
		{
			CurrentView.GetChild(0).QueueFree();
		}

		GetTree().Paused = false;
		Node newView = scene.Instantiate();
		CurrentView.AddChild(newView);

		return newView;
	}

	public void SetupEventConnections()
	{
		Events.Instance.BattleRewardExited += () => ChangeView(MAP_SCENE);
		Events.Instance.CampfireExited += () => ChangeView(MAP_SCENE);
		Events.Instance.ShopExited += () => ChangeView(MAP_SCENE);
		Events.Instance.TreasureRoomExited += () => ChangeView(MAP_SCENE);

		Events.Instance.BattleWon += OnBattleWon;
		Events.Instance.MapExited += OnMapExited;

		BattleButton.Pressed += () => ChangeView(BATTLE_SCENE);
		CampfireButton.Pressed += () => ChangeView(CAMPFIRE_SCENE);
		MapButton.Pressed += () => ChangeView(MAP_SCENE);
		RewardsButton.Pressed += () => ChangeView(BATTLE_REWARD_SCENE);
		ShopButton.Pressed += () => ChangeView(SHOP_SCENE);
		TreasureButton.Pressed += () => ChangeView(TREASURE_SCENE);
	}

	public void OnMapExited()
	{
		GD.Print("TODO: From the map, we need to change view based on the current room type");
	}

	public void OnBattleWon()
	{
		BattleReward rewardScene = ChangeView(BATTLE_REWARD_SCENE) as BattleReward;
		rewardScene.runStats = stats;
		rewardScene.characterStats = Character;

		rewardScene.AddGoldReward(77);
		rewardScene.AddCardReward();
	}

}

