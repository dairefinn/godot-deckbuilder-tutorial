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
	public CardPileOpener discardPileButton;
	public CardPileOpener drawPileButton;
	public CardPileView discardPileView;
	public CardPileView drawPileView;

    public override void _Ready()
    {
		hand = GetNode<Hand>("Hand");
		manaUI = GetNode<ManaUI>("ManaUI");
		endTurnButton = GetNode<Button>("%EndTurnButton");
		discardPileButton = GetNode<CardPileOpener>("%DiscardPileButton");
		drawPileButton = GetNode<CardPileOpener>("%DrawPileButton");
		discardPileView = GetNode<CardPileView>("%DiscardPileView");
		drawPileView = GetNode<CardPileView>("%DrawPileView");

		Events.Instance.PlayerHandDrawn += OnPlayerHandDrawn;
		endTurnButton.Pressed += OnEndTurnButtonPressed;
		drawPileButton.Pressed += () => {
			drawPileView.ShowCurrentView("Draw pile", true);
		};
		discardPileButton.Pressed += () => discardPileView.ShowCurrentView("Discard pile");
    }

	public void InitializeCardPileUI()
	{
		drawPileButton.cardPile = _charStats.drawPile;
		drawPileView.cardPile = _charStats.drawPile;
		discardPileButton.cardPile = _charStats.discard;
		discardPileView.cardPile = _charStats.discard;
	}

	public void OnPlayerHandDrawn()
	{
		if (!IsInstanceValid(endTurnButton)) return;
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
