namespace  DeckBuilder;

using Godot;

public partial class BattleUI : CanvasLayer
{

	private Events globalEvents;

	[Export] public CharacterStats charStats {
		get => _charStats;
		set => SetCharStats(value);
	}
	private CharacterStats _charStats;

	public Hand hand;
	public ManaUI manaUI;
	public Button endTurnButton;

    public override void _Ready()
    {
		globalEvents = GetNode<Events>("/root/Events");

		hand = GetNode<Hand>("Hand");
		manaUI = GetNode<ManaUI>("ManaUI");
		endTurnButton = GetNode<Button>("%EndTurnButton");

		globalEvents.PlayerHandDrawn += OnPlayerHandDrawn;
		endTurnButton.Pressed += OnEndTurnButtonPressed;
    }

	private void OnPlayerHandDrawn()
	{
		endTurnButton.Disabled = false;
	}

	private void OnEndTurnButtonPressed()
	{
		endTurnButton.Disabled = true;
		globalEvents.EmitSignal(Events.SignalName.PlayerTurnEnded);
	}

	public void SetCharStats(CharacterStats stats)
	{
		_charStats = stats;
		manaUI.charStats = stats;
		hand.charStats = stats;
	}

}
