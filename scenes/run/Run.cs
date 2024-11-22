namespace DeckBuilder;

using Godot;

public partial class Run : Node
{

	private PackedScene BATTLE_SCENE = GD.Load<PackedScene>("res://scenes/battle/battle.tscn");
	private PackedScene BATTLE_REWARD_SCENE = GD.Load<PackedScene>("res://scenes/battle_reward/battle_reward.tscn");
	private PackedScene CAMPFIRE_SCENE = GD.Load<PackedScene>("res://scenes/campfire/campfire.tscn");
	private PackedScene SHOP_SCENE = GD.Load<PackedScene>("res://scenes/shop/shop.tscn");
	private PackedScene TREASURE_SCENE = GD.Load<PackedScene>("res://scenes/treasure/treasure.tscn");
	private PackedScene WIN_SCREEN_SCENE = GD.Load<PackedScene>("res://scenes/win_screen/win_screen.tscn");

	[Export] RunStartup runStartup;

	public Map map;
	public Node CurrentView;
	public RelicHandler relicHandler;
	public RelicTooltip relicTooltip;
	public CardPileOpener deckButton;
	public CardPileView deckView;
	public HealthUI healthUI;
	public GoldUI goldUI;

	public Button BattleButton;
	public Button CampfireButton;
	public Button MapButton;
	public Button RewardsButton;
	public Button ShopButton;
	public Button TreasureButton;

	public RunStats stats;
	public CharacterStats character;

    public override void _Ready()
    {
		map = GetNode<Map>("Map");
		CurrentView = GetNode<Node>("CurrentView");
		deckButton = GetNode<CardPileOpener>("%DeckButton");
		relicHandler = GetNode<RelicHandler>("%RelicHandler");
		relicTooltip = GetNode<RelicTooltip>("%RelicTooltip");
		deckView = GetNode<CardPileView>("%DeckView");
		healthUI = GetNode<HealthUI>("%HealthUI");
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
				character = runStartup.pickedCharacter.CreateInstance();
				StartRun();
				break;
			case RunStartup.Type.CONTINUED_RUN:
				GD.Print("TODO: Load previous run");
				break;
		}
    }

    public override void _Input(InputEvent @event)
    {
		if (@event.IsActionPressed("cheat"))
		{
			GetTree().CallGroup("enemies", "queue_free");
		}
    }

    public void StartRun()
	{
		stats = new RunStats();
		SetupEventConnections();
		SetupTopBar();
		map.GenerateNewMap();
		map.UnlockFloor(0);
	}

	public void SetupTopBar()
	{
		character.StatsChanged += () => healthUI.UpdateStats(character);
		healthUI.UpdateStats(character);
		goldUI.runStats = stats;

		relicHandler.AddRelic(character.startingRelic);
		Events.Instance.RelicTooltipRequested += relicTooltip.ShowTooltip;

		deckButton.cardPile = character.deck;
		deckView.cardPile = character.deck;
		deckButton.Pressed += () => deckView.ShowCurrentView("Deck");
	}

	public void ShowRegularBattleRewards()
	{
		BattleReward rewardScene = ChangeView(BATTLE_REWARD_SCENE) as BattleReward;
		rewardScene.runStats = stats;
		rewardScene.characterStats = character;

		rewardScene.AddGoldReward(map.lastRoom.battleStats.RollGoldReward());
		rewardScene.AddCardReward();
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

		map.HideMap();

		return newView;
	}

	public void ShowMap()
	{
		if (CurrentView.GetChildCount() > 0)
		{
			CurrentView.GetChild(0).QueueFree();
		}

		map.ShowMap();
		map.UnlockNextRooms();
	}

	public void SetupEventConnections()
	{
		Events.Instance.BattleRewardExited += () => ShowMap();
		Events.Instance.CampfireExited += () => ShowMap();
		Events.Instance.ShopExited += () => ShowMap();
		Events.Instance.TreasureRoomExited += OnTreasureRoomExited;

		Events.Instance.BattleWon += OnBattleWon;
		Events.Instance.MapExited += OnMapExited;

		BattleButton.Pressed += () => ChangeView(BATTLE_SCENE);
		CampfireButton.Pressed += () => ChangeView(CAMPFIRE_SCENE);
		MapButton.Pressed += () => ShowMap();
		RewardsButton.Pressed += () => ChangeView(BATTLE_REWARD_SCENE);
		ShopButton.Pressed += () => ChangeView(SHOP_SCENE);
		TreasureButton.Pressed += () => ChangeView(TREASURE_SCENE);
	}

	public void OnMapExited(Room room)
	{
		switch (room.type)
		{
			case Room.Type.MONSTER:
			case Room.Type.BOSS:
				OnBattleRoomEntered(room);
				break;
			case Room.Type.TREASURE:
				OnTreasureRoomEntered();
				break;
			case Room.Type.CAMPFIRE:
				OnCampfireRoomEntered();
				break;
			case Room.Type.SHOP:
				OnShopEntered();
				break;
		}
	}


	public void OnBattleRoomEntered(Room room)
	{
		Battle battleScene = ChangeView(BATTLE_SCENE) as Battle;
		battleScene.charStats = character;
		battleScene.battleStats = room.battleStats;
		battleScene.relics = relicHandler;
		battleScene.StartBattle();
	}

	public void OnTreasureRoomEntered()
	{
		Treasure treasureScene = ChangeView(TREASURE_SCENE) as Treasure;
		treasureScene.relicHandler = relicHandler;
		treasureScene.charStats = character;
		treasureScene.GenerateRelic();
	}

	public void OnCampfireRoomEntered()
	{
		Campfire campfireScene = ChangeView(CAMPFIRE_SCENE) as Campfire;
		campfireScene.charStats = character;
	}

	public void OnShopEntered()
	{
		Shop shop = ChangeView(SHOP_SCENE) as Shop;
		shop.charStats = character;
		shop.runStats = stats;
		shop.relicHandler = relicHandler;
		Events.Instance.EmitSignal(Events.SignalName.ShopEntered, shop);
		shop.PopulateShop();
	}

	public void OnTreasureRoomExited(Relic relic)
	{
		BattleReward rewardScene = ChangeView(BATTLE_REWARD_SCENE) as BattleReward;
		rewardScene.runStats = stats;
		rewardScene.characterStats = character;
		rewardScene.relicHandler = relicHandler;

		rewardScene.AddRelicReward(relic);
	}

	public void OnBattleWon()
	{
		if (map.floorsClimbed == MapGenerator.FLOORS)
		{
			WinScreen winScreen = ChangeView(WIN_SCREEN_SCENE) as WinScreen;
			winScreen.character = character;
		}
		else
		{
			ShowRegularBattleRewards();
		}
	}

}

