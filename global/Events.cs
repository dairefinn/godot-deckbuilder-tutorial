using Godot;

namespace DeckBuilder;

public partial class Events : Node
{

	[Signal] public delegate void CardAimStartedEventHandler(CardUI cardUI);
	[Signal] public delegate void CardAimEndedEventHandler(CardUI cardUI);
	
}
