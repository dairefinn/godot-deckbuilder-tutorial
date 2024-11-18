namespace DeckBuilder;

using Godot;

public partial class StatusUI : Control
{

    [Export] public Status status {
        get => _status;
        set => SetStatus(value);
    }
    private Status _status;

    public TextureRect icon;
    public Label duration;
    public Label stacks;


    public override void _Ready()
    {
        icon = GetNode<TextureRect>("Icon");
        duration = GetNode<Label>("Duration");
        stacks = GetNode<Label>("Stacks");
    }

    public async void SetStatus(Status status)
    {
        if (!IsNodeReady())
        {
            await ToSignal(this, "ready");
        }

        _status = status;
        icon.Texture = status.icon;
        duration.Visible = status.stackType == Status.StackType.DURATION;
        stacks.Visible = status.stackType == Status.StackType.INTENSITY;
        CustomMinimumSize = icon.Size;

        if (duration.Visible)
        {
            CustomMinimumSize = duration.Size + duration.Position;
        }
        else if (stacks.Visible)
        {
            CustomMinimumSize = stacks.Size + stacks.Position;
        }

        if (!status.IsConnected(Status.SignalName.StatusChanged, new Callable(this, MethodName.OnStatusChanged)))
		{
            status.StatusChanged += OnStatusChanged;
		}

        OnStatusChanged();
    }

    public void OnStatusChanged()
    {
        if (status == null) return;

        if (status.canExpire && status.duration <= 0)
        {
            QueueFree();
            return;
        }

        if (status.stackType == Status.StackType.INTENSITY && status.stacks == 0)
        {
            QueueFree();
            return;
        }

        duration.Text = status.duration.ToString();
        stacks.Text = status.stacks.ToString();
    }

}
