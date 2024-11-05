using Godot;

namespace DeckBuilder;

public partial class CardReleasedState : CardState
{

	public bool played;
	
	private Events globalEvents;


    public override void _Ready()
    {
        globalEvents = GetNode<Events>("/root/Events");
    }

	public override void Enter()
	{
		played = false;

		if (cardUI.targets.Count > 0)
		{
			played = true;
			cardUI.Play();
			globalEvents.EmitSignal(Events.SignalName.TooltipHideRequested);
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
