namespace DeckBuilder;

using Godot;

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
			Events.Instance.EmitSignal(Events.SignalName.TooltipHideRequested);
		}
	}

    public override void PostEnter()
    {
		EmitSignal(SignalName.TransitionRequested, this, (int)CardState.State.BASE);
    }

}
