using System.Reflection;
using Godot;

public partial class CardBaseState : CardState
{

	public override async void Enter()
	{
		base.Enter();

		if (!cardUI.IsNodeReady())
		{
			await ToSignal(cardUI, "ready");
		}

		if (cardUI.tween != null && cardUI.tween.IsRunning())
		{
			cardUI.tween.Kill();
		}

		cardUI.EmitSignal(CardUI.SignalName.ReparentRequested, cardUI);
		cardUI.color.Color = Colors.WebGreen;
		cardUI.PivotOffset = Vector2.Zero;
	}

	public override void OnGuiInput(InputEvent @event)
	{
		if (@event.IsActionPressed("left_mouse"))
		{
			cardUI.PivotOffset = cardUI.GetGlobalMousePosition() - cardUI.GlobalPosition;
			EmitSignal(SignalName.TransitionRequested, this, (int)CardState.State.CLICKED);
		}
	}

}
