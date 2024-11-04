using Godot;

namespace DeckBuilder;

public partial class Events : Node
{

	// Card related events

	[Signal] public delegate void CardDragStartedEventHandler(CardUI cardUI);
	[Signal] public delegate void CardDragEndedEventHandler(CardUI cardUI);
	[Signal] public delegate void CardAimStartedEventHandler(CardUI cardUI);
	[Signal] public delegate void CardAimEndedEventHandler(CardUI cardUI);

	[Signal] public delegate void CardPlayedEventHandler(Card card);

	
}
