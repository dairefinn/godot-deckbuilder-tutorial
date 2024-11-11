namespace DeckBuilder;

using Godot;
using Godot.Collections;


public partial class CardPileView : Control
{

    private readonly PackedScene CARD_MENU_UI_SCENE = GD.Load<PackedScene>("res://scenes/ui/card_menu_ui.tscn");

    [Export] public CardPile cardPile;

    public Label title;
    public GridContainer cards;
    public CardTooltipPopup cardTooltipPopup;
    public Button backButton;

    public override void _Ready()
    {
        cardTooltipPopup = GetNode<CardTooltipPopup>("%CardTooltipPopup");
        title = GetNode<Label>("%Title");
        cards = GetNode<GridContainer>("%Cards");
        backButton = GetNode<Button>("%BackButton");

        backButton.Pressed += () => Hide();

        foreach (Node card in cards.GetChildren())
        {
            card.QueueFree();
        }

        cardTooltipPopup.HideTooltip();
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event.IsActionPressed("ui_cancel"))
        {
            if (cardTooltipPopup.Visible)
            {
                cardTooltipPopup.HideTooltip();
            }
            else
            {
                Hide();
            }
        }
    }

    public void ShowCurrentView(string newTitle, bool randomized = false)
    {
        foreach (Node card in cards.GetChildren())
        {
            card.QueueFree();
        }

        cardTooltipPopup.HideTooltip();
        title.Text = newTitle;
        CallDeferred(MethodName.UpdateView, randomized);
        Show();
    }

    public void UpdateView(bool randomized)
    {
        if (cardPile == null) return;

        Array<Card> allCards = cardPile.cards.Duplicate();

        if (randomized)
        {
            allCards.Shuffle();
        }

        foreach (Card card in allCards)
        {
            CardMenuUI newCard = CARD_MENU_UI_SCENE.Instantiate<CardMenuUI>();
            cards.AddChild(newCard);
            newCard.card = card;
            newCard.TooltipRequested += (param) => cardTooltipPopup.ShowTooltip(param);
        }
    }

}
