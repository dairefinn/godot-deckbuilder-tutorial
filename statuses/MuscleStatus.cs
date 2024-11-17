namespace DeckBuilder;

using Godot;


public partial class MuscleStatus : Status
{

	public static readonly float MODIFIER = 0.5f;

    public override void InitializeStatus(Node target)
    {
        StatusChanged += OnStatusChanged;
        OnStatusChanged();
    }

    public void OnStatusChanged()
    {
        GD.Print("Muscle status: +" + stacks + "% damage");

        EmitSignal(SignalName.StatusApplied, this);
    }

}

