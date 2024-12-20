namespace DeckBuilder;

using Godot;

public partial class CardTooltipPopup : Control
{

    private readonly PackedScene CARD_MENU_UI_SCENE = GD.Load<PackedScene>("res://scenes/ui/card_menu_ui.tscn");

    [Export] public Color backgroundColor = new("000000b0");

    public ColorRect background;
    public CenterContainer tooltipCard;
    public RichTextLabel cardDescription;

    public override void _Ready()
    {
        background = GetNode<ColorRect>("Background");
        tooltipCard = GetNode<CenterContainer>("%TooltipCard");
        cardDescription = GetNode<RichTextLabel>("%CardDescription");

        GuiInput += OnGuiInput;

        foreach (Node child in tooltipCard.GetChildren())
        {
            if (child is CardMenuUI cardMenuUI)
            {
                cardMenuUI.QueueFree();
            }
        }

        background.Color = backgroundColor;
    }

    public void ShowTooltip(Card card)
    {
        CardMenuUI newCard = CARD_MENU_UI_SCENE.Instantiate<CardMenuUI>();
        tooltipCard.AddChild(newCard);
        newCard.card = card;
        newCard.TooltipRequested += (card) => HideTooltip();
        cardDescription.Text = card.GetDefaultTooltip();
        Show();
    }

    public void HideTooltip()
    {
        if (!Visible) return;

        foreach (Node child in tooltipCard.GetChildren())
        {
            if (child is CardMenuUI cardMenuUI)
            {
                cardMenuUI.QueueFree();
            }
        }

        Hide();
    }

    public void OnGuiInput(InputEvent @event)
    {
        if (@event.IsActionPressed("left_mouse"))
        {
            HideTooltip();
        }
    }

}
