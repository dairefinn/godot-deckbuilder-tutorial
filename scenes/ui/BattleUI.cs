namespace  DeckBuilder;

using Godot;

public partial class BattleUI : CanvasLayer
{

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
		hand = GetNode<Hand>("Hand");
		manaUI = GetNode<ManaUI>("ManaUI");
		endTurnButton = GetNode<Button>("%EndTurnButton");

		Events.Instance.PlayerHandDrawn += OnPlayerHandDrawn;
		endTurnButton.Pressed += OnEndTurnButtonPressed;
    }

	public void OnPlayerHandDrawn()
	{
		endTurnButton.Disabled = false;
	}

	public void OnEndTurnButtonPressed()
	{
		endTurnButton.Disabled = true;
		Events.Instance.EmitSignal(Events.SignalName.PlayerTurnEnded);
	}

	public void SetCharStats(CharacterStats stats)
	{
		_charStats = stats;
		manaUI.charStats = stats;
		hand.charStats = stats;
	}

}
