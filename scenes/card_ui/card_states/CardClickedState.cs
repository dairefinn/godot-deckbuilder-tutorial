using Godot;

namespace DeckBuilder;

public partial class CardClickedState : CardState
{
	
	public override void Enter()
	{
		cardUI.dropPointDetector.Monitoring = true;
	}

    public override void OnInput(InputEvent @event)
    {
        if (@event is InputEventMouse mouseEvent)
		{
			EmitSignal(SignalName.TransitionRequested, this, (int)CardState.State.DRAGGING);
			return;
		}
    }

}
