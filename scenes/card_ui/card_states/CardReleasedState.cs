using Godot;

namespace DeckBuilder;

public partial class CardReleasedState : CardState
{

	public bool played;

	public override void Enter()
	{
		base.Enter();

		cardUI.color.Color = Colors.DarkViolet;
		played = false;

		if (cardUI.targets.Count > 0)
		{
			played = true;
			GD.Print("Card played for target: " + cardUI.targets[0].Name);
		}
	}

	public override void OnInput(InputEvent @event)
	{
		if (played)
		{
			return;
		}

		EmitSignal(SignalName.TransitionRequested, this, (int)CardState.State.BASE);
	}

}
