namespace DeckBuilder;

using Godot;

public partial class RelicTooltip : Control
{

    public TextureRect relicIcon;
    public RichTextLabel relicTooltip;
    public Button backButton;

    public override void _Ready()
    {
        relicIcon = GetNode<TextureRect>("%RelicIcon");
        relicTooltip = GetNode<RichTextLabel>("%RelicTooltip");
        backButton = GetNode<Button>("%BackButton");

        GuiInput += OnGuiInput;
        backButton.Pressed += Hide;
        Hide();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel") && Visible)
        {
            Hide();
        }
    }

    public void ShowTooltip(Relic relic)
    {
        relicIcon.Texture = relic.icon;
        relicTooltip.Text = relic.GetTooltip();
        Show();
    }

    public void OnGuiInput(InputEvent @event)
    {
        if (@event.IsActionPressed("left_mouse"))
        {
            Hide();
        }
    }

}
