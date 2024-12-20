namespace DeckBuilder;

using Godot;
using Godot.Collections;


public partial class Events : Node
{

	public static Events Instance;

	public Events()
	{
		Instance = this;

		// CardDragStarted += (cardUI) => { GD.Print("CardDragStarted: ", cardUI); };
		// CardDragEnded += (cardUI) => { GD.Print("CardDragEnded: ", cardUI); };
		// CardAimStarted += (cardUI) => { GD.Print("CardAimStarted: ", cardUI); };
		// CardAimEnded += (cardUI) => { GD.Print("CardAimEnded: ", cardUI); };
		// CardPlayed += (card) => { GD.Print("CardPlayed: ", card); };
		
		// CardTooltipRequested += (icon, text) => { GD.Print("CardTooltipRequested: ", icon, text); };
		// TooltipHideRequested += () => { GD.Print("TooltipHideRequested"); };

		// PlayerHandDrawn += () => { GD.Print("PlayerHandDrawn"); };
		// PlayerHandDiscarded += () => { GD.Print("PlayerHandDiscarded"); };
		// PlayerTurnEnded += () => { GD.Print("PlayerTurnEnded"); };
		// PlayerDied += () => { GD.Print("PlayerDied"); };
		// PlayerHit += () => { GD.Print("PlayerHit"); };

		// EnemyActionCompleted += (enemy) => { GD.Print("EnemyActionCompleted: ", enemy); };
		// EnemyTurnEnded += () => { GD.Print("EnemyTurnEnded"); };

		// BattleOverScreenRequested += (text, type) => { GD.Print("BattleOverScreenRequested: ", text, type); };
	}

	// Card related events

	[Signal] public delegate void CardDragStartedEventHandler(CardUI cardUI);
	[Signal] public delegate void CardDragEndedEventHandler(CardUI cardUI);
	[Signal] public delegate void CardAimStartedEventHandler(CardUI cardUI);
	[Signal] public delegate void CardAimEndedEventHandler(CardUI cardUI);
	[Signal] public delegate void CardPlayedEventHandler(Card card);


	// Tooltip related events

	[Signal] public delegate void CardTooltipRequestedEventHandler(Texture2D icon, string text);
	[Signal] public delegate void TooltipHideRequestedEventHandler();


	// Player related events

	[Signal] public delegate void PlayerHandDrawnEventHandler();
	[Signal] public delegate void PlayerHandDiscardedEventHandler();
	[Signal] public delegate void PlayerTurnEndedEventHandler();
	[Signal] public delegate void PlayerDiedEventHandler();
	[Signal] public delegate void PlayerHitEventHandler();

	// Enemy related events

	[Signal] public delegate void EnemyActionCompletedEventHandler(Enemy enemy);
	[Signal] public delegate void EnemyTurnEndedEventHandler();
	[Signal] public delegate void EnemyDiedEventHandler(Enemy enemy);

	// Battle related events

	[Signal] public delegate void BattleOverScreenRequestedEventHandler(string text, BattleOverPanel.Type type);
	[Signal] public delegate void BattleWonEventHandler();
	[Signal] public delegate void StatusTooltipRequestedEventHandler(Array<Status> statuses);

	// Map related events

	[Signal] public delegate void MapExitedEventHandler(Room room);

	// Shop related events

	[Signal] public delegate void ShopEnteredEventHandler(Shop shop);
	[Signal] public delegate void ShopExitedEventHandler();
	[Signal] public delegate void ShopCardBoughtEventHandler(Card card, int goldCost);
	[Signal] public delegate void ShopRelicBoughtEventHandler(Relic relic, int goldCost);

	// Campfire related events

	[Signal] public delegate void CampfireExitedEventHandler();

	// Battle reward related events

	[Signal] public delegate void BattleRewardExitedEventHandler();

	// Treasure room related events

	[Signal] public delegate void TreasureRoomExitedEventHandler(Relic foundRelic);

	// Relic related events

	[Signal] public delegate void RelicTooltipRequestedEventHandler(Relic relic);

}
