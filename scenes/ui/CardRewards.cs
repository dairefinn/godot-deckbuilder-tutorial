namespace DeckBuilder;

using Godot;
using Godot.Collections;


public partial class CardRewards : ColorRect
{

    public static readonly PackedScene CARD_MENU_UI = GD.Load<PackedScene>("res://scenes/ui/card_menu_ui.tscn");

    [Signal] public delegate void CardRewardSelectedEventHandler(Card card);

    [Export] public Array<Card> rewards {
        get => _rewards;
        set => SetRewards(value);
    }
    private Array<Card> _rewards;

    public HBoxContainer cards;
    public Button skipCardReward;
    public CardTooltipPopup cardTooltipPopup;
    public Button takeButton;

    public Card selectedCard;

    public override void _Ready()
    {
        cards = GetNode<HBoxContainer>("%Cards");
        skipCardReward = GetNode<Button>("%SkipCardReward");
        cardTooltipPopup = GetNode<CardTooltipPopup>("CardTooltipPopup");
        takeButton = GetNode<Button>("%TakeButton");

        ClearRewards();
        takeButton.Pressed += () => {
            EmitSignal(CardRewards.SignalName.CardRewardSelected, selectedCard);
            QueueFree();
        };

        skipCardReward.Pressed += () => {
            EmitSignal(CardRewards.SignalName.CardRewardSelected, null);
            QueueFree();
        };
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            cardTooltipPopup.HideTooltip();
        }
    }

    public void ClearRewards()
    {
        foreach(Node card in cards.GetChildren())
        {
            card.QueueFree();
        }

        cardTooltipPopup.HideTooltip();

        selectedCard = null;
    }

    public void ShowTooltip(Card card)
    {
        selectedCard = card;
        cardTooltipPopup.ShowTooltip(card);
    }

    public async void SetRewards(Array<Card> value)
    {
        _rewards = value;

        if (!IsNodeReady()) {
            await ToSignal(this, "ready");
        }

        ClearRewards();

        foreach(Card card in _rewards)
        {
            CardMenuUI newCard = CARD_MENU_UI.Instantiate<CardMenuUI>();
            cards.AddChild(newCard);
            newCard.card = card;
            newCard.TooltipRequested += (card) => ShowTooltip(card);
        }
    }

}
