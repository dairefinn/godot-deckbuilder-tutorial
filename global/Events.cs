namespace DeckBuilder;

using Godot;

public partial class Events : Node
{

	public static Events Instance;

	public Events()
	{
		Instance = this;

		CardDragStarted += (cardUI) => { GD.Print("CardDragStarted: ", cardUI); };
		CardDragEnded += (cardUI) => { GD.Print("CardDragEnded: ", cardUI); };
		CardAimStarted += (cardUI) => { GD.Print("CardAimStarted: ", cardUI); };
		CardAimEnded += (cardUI) => { GD.Print("CardAimEnded: ", cardUI); };
		CardPlayed += (card) => { GD.Print("CardPlayed: ", card); };
		
		CardTooltipRequested += (icon, text) => { GD.Print("CardTooltipRequested: ", icon, text); };
		TooltipHideRequested += () => { GD.Print("TooltipHideRequested"); };

		PlayerHandDrawn += () => { GD.Print("PlayerHandDrawn"); };
		PlayerHandDiscarded += () => { GD.Print("PlayerHandDiscarded"); };
		PlayerTurnEnded += () => { GD.Print("PlayerTurnEnded"); };
		PlayerDied += () => { GD.Print("PlayerDied"); };
		PlayerHit += () => { GD.Print("PlayerHit"); };

		EnemyActionCompleted += (enemy) => { GD.Print("EnemyActionCompleted: ", enemy); };
		EnemyTurnEnded += () => { GD.Print("EnemyTurnEnded"); };
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

}
