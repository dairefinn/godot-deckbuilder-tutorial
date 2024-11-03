using Godot;

public partial class CardDraggingState : CardState
{

	private const float DRAGGING_MINIMUM_THRESHOLD = 0.05f;
	private bool minimumDragTimeElapsed = false;

    public override void Enter()
    {
		base.Enter();

		Node uiLayer = GetTree().GetFirstNodeInGroup("ui_layer");
		if (uiLayer != null)
		{
			cardUI.Reparent(uiLayer);
		}

		cardUI.color.Color = Colors.NavyBlue;

		minimumDragTimeElapsed = false;
		SceneTreeTimer thresholdTimer = GetTree().CreateTimer(DRAGGING_MINIMUM_THRESHOLD, false);
		thresholdTimer.Timeout += () => minimumDragTimeElapsed = true;
    }

    public override void OnInput(InputEvent @event)
    {
		bool singleTargeted = cardUI.card.GetIsSingleTargeted();
		bool mouseMotion = @event is InputEventMouseMotion;
		bool cancel = @event.IsActionPressed("right_mouse");
		bool confirm = @event.IsActionPressed("left_mouse") || @event.IsActionReleased("left_mouse");

		if (singleTargeted && mouseMotion && cardUI.targets.Count > 0)
		{
			EmitSignal(SignalName.TransitionRequested, this, (int)CardState.State.AIMING);
			return;
		}

		if (mouseMotion)
		{
			cardUI.GlobalPosition = cardUI.GetGlobalMousePosition() - cardUI.PivotOffset;
		}

		if (cancel)
		{
			EmitSignal(SignalName.TransitionRequested, this, (int)CardState.State.BASE);
			return;
		}
		
		if (minimumDragTimeElapsed && confirm)
		{
			GetViewport().SetInputAsHandled();
			EmitSignal(SignalName.TransitionRequested, this, (int)CardState.State.RELEASED);
			return;
		}
    }

}
