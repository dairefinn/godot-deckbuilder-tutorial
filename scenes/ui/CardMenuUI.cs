namespace DeckBuilder;

using Godot;

public partial class CardMenuUI : CenterContainer
{

    [Signal] public delegate void TooltipRequestedEventHandler(Card card);

    private readonly StyleBox BASE_STYLEBOX = GD.Load<StyleBox>("res://scenes/card_ui/card_base_stylebox.tres");
    private readonly StyleBox HOVER_STYLEBOX = GD.Load<StyleBox>("res://scenes/card_ui/card_hover_stylebox.tres");

    public Control Visuals;

    [Export] public Card card {
        get => _card;
        set => SetCard(value);
    }
    private Card _card;

    public Panel panel;
    public Label cost;
    public TextureRect icon;

    public override void _Ready()
    {
        Visuals = GetNode<Control>("Visuals");
        panel = GetNode<Panel>("Visuals/Panel");
        cost = GetNode<Label>("Visuals/Cost");
        icon = GetNode<TextureRect>("Visuals/Icon");

        Visuals.GuiInput += OnVisualsGuiInput;
        Visuals.MouseEntered += OnVisualsMouseEntered;
        Visuals.MouseExited += OnVisualsMouseExited;
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
        panel.Set("theme_override_styles/panel", HOVER_STYLEBOX);
    }

    public void OnVisualsMouseExited()
    {
        panel.Set("theme_override_styles/panel", BASE_STYLEBOX);
    }

    public async void SetCard(Card value)
    {
        if (!IsNodeReady())
        {
            await ToSignal(this, "ready");
        }

        _card = value;
        cost.Text = card.cost.ToString();
        icon.Texture = card.icon;
    }


}
