using Godot;

namespace DeckBuilder;

public partial class CardReleasedState : CardState
{

	public bool played;

	public override void Enter()
	{
		played = false;

		if (cardUI.targets.Count > 0)
		{
			played = true;
			cardUI.Play();
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
