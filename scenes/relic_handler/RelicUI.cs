namespace DeckBuilder;

using Godot;

public partial class RelicUI : Control
{

    [Export] public Relic relic {
        get => _relic;
        set => SetRelic(value);
    }
    private Relic _relic;

    public TextureRect icon;
    public AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        icon = GetNode<TextureRect>("Icon");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        GuiInput += OnGuiInput;
    }

    public void OnGuiInput(InputEvent @event)
    {
        if (@event.IsActionPressed("left_mouse"))
        {
            Events.Instance.EmitSignal(Events.SignalName.RelicTooltipRequested, relic);
        }
    }

    public async void SetRelic(Relic relic)
    {
        if (!IsNodeReady())
        {
            await ToSignal(this, "ready");
        }

        _relic = relic;
        icon.Texture = relic.icon;
    }

    public void Flash()
    {
        animationPlayer.Play("flash");
    }

}
