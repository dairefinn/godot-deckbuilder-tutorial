namespace DeckBuilder;

using Godot;

[GlobalClass]
public partial class Status : Resource
{

    public enum Type
    {
        START_OF_TURN,
        END_OF_TURN,
        EVENT_BASED
    }

    public enum StackType
    {
        NONE,
        INTENSITY,
        DURATION
    }

    [Signal] public delegate void StatusAppliedEventHandler(Status status);
    [Signal] public delegate void StatusChangedEventHandler();


    [ExportGroup("Status data")]
    [Export] public string id;
    [Export] public Type type;
    [Export] public StackType stackType;
    [Export] public bool canExpire;
    [Export] public int duration {
        get => _duration;
        set => SetDuration(value);
    }
    private int _duration;
    [Export] public int stacks {
        get => _stacks;
        set => SetStacks(value);
    }
    private int _stacks;

    [ExportGroup("Status visuals")]
    [Export] public Texture2D icon;
    [Export(PropertyHint.MultilineText)] public string tooltip;


    public virtual void InitializeStatus(Node target)
    {
    }

    public virtual void ApplyStatus(Node target)
    {
        EmitSignal(SignalName.StatusApplied, this);
    }

    public string GetTooltip()
    {
        return tooltip;
    }

    public void SetDuration(int value)
    {
        _duration = value;
        EmitSignal(SignalName.StatusChanged);
    }

    public void SetStacks(int value)
    {
        _stacks = value;
        EmitSignal(SignalName.StatusChanged);
    }

}