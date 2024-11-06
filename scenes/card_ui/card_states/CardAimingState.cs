using Godot;

namespace DeckBuilder;

public partial class CardAimingState : CardState
{

	const int MOUSE_Y_SNAPBACK_THRESHOLD = 138;

    public override void Enter()
	{
		cardUI.targets.Clear();

		float cardOffsetX = (cardUI.parent.Size.X - cardUI.Size.X) / 2;
		float cardOffsetY = cardUI.Size.Y / 2;
		Vector2 offset = new(cardOffsetX, - cardOffsetY);
		cardUI.AnimateToPosition(cardUI.parent.GlobalPosition + offset, 0.2f);

		cardUI.dropPointDetector.Monitoring = false;
		Events.Instance.EmitSignal(Events.SignalName.CardAimStarted, cardUI);
	}

	public override void Exit()
	{
		Events.Instance.EmitSignal(Events.SignalName.CardAimEnded, cardUI);
	}

    public override void OnInput(InputEvent @event)
    {
		bool mouseMotion = @event is InputEventMouseMotion;
		bool mouseAtBottom = cardUI.GetGlobalMousePosition().Y > MOUSE_Y_SNAPBACK_THRESHOLD;

		// If the mouse is at the bottom of the screen, or the right mouse button is pressed, revert to the base state
		if ((mouseMotion && mouseAtBottom) || @event.IsActionPressed("right_mouse"))
		{
			EmitSignal(CardState.SignalName.TransitionRequested, this, (int)CardState.State.BASE);
			return;
		}

		// If the left mouse button is pressed or released, transition to the released state
		if (@event.IsActionPressed("left_mouse") || @event.IsActionReleased("left_mouse"))
		{
			GetViewport().SetInputAsHandled();
			EmitSignal(CardState.SignalName.TransitionRequested, this, (int)CardState.State.RELEASED);
			return;
		}
    }

}
