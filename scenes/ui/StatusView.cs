namespace DeckBuilder;

using Godot;
using Godot.Collections;


public partial class StatusView : Control
{

    public static readonly PackedScene STATUS_TOOLTIP = GD.Load<PackedScene>("res://scenes/ui/status_tooltip.tscn");

    public Button backButton;
    public VBoxContainer statusTooltips;

    public override void _Ready()
    {
        backButton = GetNode<Button>("BackButton");
        statusTooltips = GetNode<VBoxContainer>("%StatusTooltips");

        GuiInput += OnGuiInput;
        backButton.Pressed += OnBackButtonPressed;

        foreach (Node tooltipNode in statusTooltips.GetChildren())
        {
            if (tooltipNode is not StatusTooltip tooltip) continue;
            tooltip.QueueFree();
        }

        Events.Instance.StatusTooltipRequested += ShowView;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel") && Visible)
        {
            HideView();
        }
    }

    public void ShowView(Array<Status> statuses)
    {
        foreach (Status status in statuses)
        {
            StatusTooltip newStatusTooltip = STATUS_TOOLTIP.Instantiate<StatusTooltip>();
            statusTooltips.AddChild(newStatusTooltip);
            newStatusTooltip.status = status;
        }
        Show();
    }

    public void HideView()
    {
        foreach (Node tooltipNode in statusTooltips.GetChildren())
        {
            if (tooltipNode is not StatusTooltip tooltip) continue;
            tooltip.QueueFree();
        }
        Hide();
    }

    private void OnGuiInput(InputEvent @event)
    {
        if (@event.IsActionPressed("left_mouse") && Visible)
        {
            HideView();
        }
    }

    private void OnBackButtonPressed()
    {
        HideView();
    }

}
