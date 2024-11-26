namespace DeckBuilder;

using Godot;

public partial class CardClickedState : CardState
{
	
	public override void Enter()
	{
		cardUI.dropPointDetector.Monitoring = true;
		cardUI.originalIndex = cardUI.GetIndex();
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
