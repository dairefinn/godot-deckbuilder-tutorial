namespace DeckBuilder;

using Godot;

public partial class CardMenuUI : CenterContainer
{

    [Signal] public delegate void TooltipRequestedEventHandler(Card card);

    private readonly StyleBox BASE_STYLEBOX = GD.Load<StyleBox>("res://scenes/card_ui/card_base_stylebox.tres");
    private readonly StyleBox HOVER_STYLEBOX = GD.Load<StyleBox>("res://scenes/card_ui/card_hover_stylebox.tres");

    public CardVisuals CardVisuals;

    [Export] public Card card {
        get => _card;
        set => SetCard(value);
    }
    private Card _card;

    public override void _Ready()
    {
        CardVisuals = GetNode<CardVisuals>("CardVisuals");

        CardVisuals.GuiInput += OnVisualsGuiInput;
        CardVisuals.MouseEntered += OnVisualsMouseEntered;
        CardVisuals.MouseExited += OnVisualsMouseExited;
    }

    public void OnVisualsGuiInput(InputEvent @event)
    {
        if (@event.IsActionPressed("left_mouse"))
        {
            EmitSignal(SignalName.TooltipRequested, card);
        }
    }

    public void OnVisualsMouseEntered()
    {
        CardVisuals.panel.Set("theme_override_styles/panel", HOVER_STYLEBOX);
    }

    public void OnVisualsMouseExited()
    {
        CardVisuals.panel.Set("theme_override_styles/panel", BASE_STYLEBOX);
    }

    public async void SetCard(Card value)
    {
        if (!IsNodeReady())
        {
            await ToSignal(this, "ready");
        }

        _card = value;
        CardVisuals.card = value;
    }


}
