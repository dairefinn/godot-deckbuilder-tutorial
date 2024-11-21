namespace DeckBuilder;

using Godot;

public partial class ShopCard : VBoxContainer
{

	public static readonly PackedScene CARD_MENU_UI = GD.Load<PackedScene>("res://scenes/ui/card_menu_ui.tscn");

	[Export] public Card card {
		get => _card;
		set => SetCard(value);
	}
	private Card _card;

	public CenterContainer cardContainer;
	public HBoxContainer price;
	public Label priceLabel;
	public Button buyButton;
	public int goldCost = GD.RandRange(100, 300);

	public CardMenuUI currentCardUI;

    public override void _Ready()
    {
		cardContainer = GetNode<CenterContainer>("%CardsContainer");
		price = GetNode<HBoxContainer>("%Price");
		priceLabel = GetNode<Label>("%PriceLabel");
		buyButton = GetNode<Button>("%BuyButton");

		buyButton.Pressed += OnBuyButtonPressed;
    }

	public void Update(RunStats runStats)
	{
		if (cardContainer == null) return;
		if (price == null) return;
		if (buyButton == null) return;

		priceLabel.Text = goldCost.ToString();

		if (runStats.gold > goldCost)
		{
			priceLabel.RemoveThemeColorOverride("font_color");
			buyButton.Disabled = false;
		}
		else
		{
			priceLabel.AddThemeColorOverride("font_color", Colors.Red);
			buyButton.Disabled = true;
		}
	}

	public async void SetCard(Card value)
	{
		if (!IsNodeReady())
		{
			await ToSignal(this, "ready");
		}

		_card = value;

		foreach (Node cardMenuUINode in cardContainer.GetChildren())
		{
			if (cardMenuUINode is not CardMenuUI cardMenuUI) continue;
			cardMenuUI.QueueFree();
		}

		CardMenuUI newCardMenuUI = CARD_MENU_UI.Instantiate<CardMenuUI>();
		cardContainer.AddChild(newCardMenuUI);
		newCardMenuUI.card = card;
		currentCardUI = newCardMenuUI;
	}

	public void OnBuyButtonPressed()
	{
		Events.Instance.EmitSignal(Events.SignalName.ShopCardBought, card, goldCost);
		cardContainer.QueueFree();
		price.QueueFree();
		buyButton.QueueFree();
	}

}

