using System.Collections.Generic;
using Godot;

public partial class CardUI : Control
{

    [Signal]
    public delegate void ReparentRequestedEventHandler(CardUI whichCardUI);

	[Export] public Card card;

	public ColorRect color;
	public Label state;
    public Area2D dropPointDetector;
	public CardStateMachine cardStateMachine;
	public List<Node> targets = new List<Node>();

	public Control parent;
	public Tween tween;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		color = GetNode<ColorRect>("Color");
		state = GetNode<Label>("State");
		dropPointDetector = GetNode<Area2D>("DropPointDetector");
		cardStateMachine = GetNode<CardStateMachine>("CardStateMachine");

		dropPointDetector.AreaEntered += OnDropPointDetectorAreaEntered;
		dropPointDetector.AreaExited += OnDropPointDetectorAreaExited;

		cardStateMachine.Init(this);
	}

    public override void _Input(InputEvent @event)
    {
		cardStateMachine.OnInput(@event);

    }

    public void OnGuiInput(InputEvent @event)
    {
		cardStateMachine.OnGuiInput(@event);
    }

	public void OnMouseEntered()
	{
		cardStateMachine.OnMouseEntered();
	}

	public void OnMouseExited()
	{
		cardStateMachine.OnMouseExited();
	}

	public void OnDropPointDetectorAreaEntered(Area2D area)
	{
		if (!targets.Contains(area))
		{
			targets.Add(area);
		}
	}

	public void OnDropPointDetectorAreaExited(Area2D area)
	{
		targets.Remove(area);
	}

	public void AnimateToPosition(Vector2 newPosition, float duration)
	{
		tween = CreateTween().SetTrans(Tween.TransitionType.Circ).SetEase(Tween.EaseType.Out);
		tween.TweenProperty(this, "global_position", newPosition, duration);
	}

}
