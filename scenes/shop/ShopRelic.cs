namespace DeckBuilder;

using Godot;
using Godot.Collections;

public partial class ShopRelic : VBoxContainer
{

	public static readonly PackedScene RELIC_UI = GD.Load<PackedScene>("res://scenes/relic_handler/relic_ui.tscn");

	[Export] public Relic relic {
		get => _relic;
		set => SetRelic(value);
	}
	private Relic _relic;

	public CenterContainer relicContainer;
	public HBoxContainer price;
	public Label priceLabel;
	public Button buyButton;
	public int goldCost = RNG.Instance.RandiRange(100, 300);

    public override void _Ready()
    {
		relicContainer = GetNode<CenterContainer>("%RelicsContainer");
		price = GetNode<HBoxContainer>("%Price");
		priceLabel = GetNode<Label>("%PriceLabel");
		buyButton = GetNode<Button>("%BuyButton");

		buyButton.Pressed += OnBuyButtonPressed;
    }

	public void Update(RunStats runStats)
	{
		if (relicContainer == null) return;
		if (price == null) return;
		if (buyButton == null) return;

		priceLabel.Text = goldCost.ToString();

		if (runStats.gold >= goldCost)
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

	public async void SetRelic(Relic value)
	{
		if (!IsNodeReady())
		{
			await ToSignal(this, "ready");
		}

		_relic = value;

		foreach (Node relicUINode in relicContainer.GetChildren())
		{
			if (relicUINode is not RelicUI RelicUI) continue;
			RelicUI.QueueFree();
		}

		RelicUI newRelicUI = RELIC_UI.Instantiate<RelicUI>();
		relicContainer.AddChild(newRelicUI);
		newRelicUI.relic = relic;
	}

	public void OnBuyButtonPressed()
	{
		Events.Instance.EmitSignal(Events.SignalName.ShopRelicBought, relic, goldCost);
		relicContainer.QueueFree();
		price.QueueFree();
		buyButton.QueueFree();
	}


}

