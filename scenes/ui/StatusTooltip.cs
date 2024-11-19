namespace DeckBuilder;

using Godot;

public partial class StatusTooltip : HBoxContainer
{

    [Export] public Status status {
        get => _status;
        set => SetStatus(value);
    }
    private Status _status;

    public TextureRect icon;
    public Label label;

    public override void _Ready()
    {
        icon = GetNode<TextureRect>("Icon");
        label = GetNode<Label>("Label");
    }

    public async void SetStatus(Status value)
    {
        if (!IsNodeReady())
        {
            await ToSignal(this, "ready");
        }

        _status = value;
        icon.Texture = _status.icon;
        label.Text = _status.GetTooltip();
    }

}
