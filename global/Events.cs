using Godot;

namespace DeckBuilder;

public partial class Events : Node
{

	public static Events Instance;

	public Events()
	{
		Instance = this;
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

	// Enemy related events

	[Signal] public delegate void EnemyActionCompletedEventHandler(Enemy enemy);
	[Signal] public delegate void EnemyTurnEndedEventHandler();

}
