namespace DeckBuilder;

using Godot;

public partial class StatusTemplate : Status
{

    public int memberVariable;

    public override void InitializeStatus(Node target)
    {
        GD.Print("Status initialized for target " + target);
    }

    public override void ApplyStatus(Node target)
    {
        GD.Print("Status targets " + target);
        EmitSignal(SignalName.StatusApplied, this);
    }

}
