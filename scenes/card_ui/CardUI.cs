namespace DeckBuilder;

using System.Collections.Generic;
using Godot;


public partial class CardUI : Control
{

	public static readonly StyleBox BASE_STYLEBOX = ResourceLoader.Load<StyleBox>("res://scenes/card_ui/card_base_stylebox.tres");
	public static readonly StyleBox DRAG_STYLEBOX = ResourceLoader.Load<StyleBox>("res://scenes/card_ui/card_dragging_stylebox.tres");
	public static readonly StyleBox HOVER_STYLEBOX = ResourceLoader.Load<StyleBox>("res://scenes/card_ui/card_hover_stylebox.tres");

	private Events globalEvents;


    [Signal] public delegate void ReparentRequestedEventHandler(CardUI whichCardUI);

	[Export] public Card card {
		get => _card;
		set => SetCard(value);
	}
	private Card _card;
	[Export] public CharacterStats charStats;

	public Panel panel;
	public Label cost;
	public TextureRect icon;

    public Area2D dropPointDetector;
	public CardStateMachine cardStateMachine;
	public List<Node> targets = new();
	public int originalIndex;

	public Control parent;
	public Tween tween;

	public bool playable {
		get => _playable;
		set => SetPlayable(value);
	}
	private bool _playable = true;

	public bool disabled = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		globalEvents = GetNode<Events>("/root/Events");
		panel = GetNode<Panel>("Panel");
		cost = GetNode<Label>("Cost");
		icon = GetNode<TextureRect>("Icon");

		dropPointDetector = GetNode<Area2D>("DropPointDetector");
		cardStateMachine = GetNode<CardStateMachine>("CardStateMachine");

		originalIndex = GetIndex();

		dropPointDetector.AreaEntered += OnDropPointDetectorAreaEntered;
		dropPointDetector.AreaExited += OnDropPointDetectorAreaExited;

		globalEvents.CardDragStarted += OnCardDragOrAimingStarted;
		globalEvents.CardDragEnded += OnCardDragOrAimingEnded;

		cardStateMachine.Init(this);
	}

	public async void SetCard(Card value)
	{
		if (!IsNodeReady()) await ToSignal(this, "ready");

		_card = value;

        cost.Text = value.cost.ToString();
        icon.Texture = value.icon;
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

	public void Play()
	{
		if (card == null) return;

		card.Play(targets, charStats, globalEvents);

		QueueFree();
	}

	public void SetPlayable(bool value)
	{
		_playable = value;

		if (!IsInstanceValid(cost)) return;
		if (!IsInstanceValid(icon)) return;

		if (!playable)
		{
			cost.AddThemeColorOverride("font_color", Colors.Red);
			icon.Modulate = new Color(1, 1, 1, 0.5f);
		}
		else
		{
			cost.RemoveThemeColorOverride("font_color");
			icon.Modulate = new Color(1, 1, 1, 1);
		}
	}

	public void SetCharStats(CharacterStats value)
	{
		charStats = value;
		charStats.StatsChanged += OnCharStatsChanged;
	}

	public void OnCharStatsChanged()
	{
		playable = charStats.CanPlayCard(card);
	}

	public void OnCardDragOrAimingStarted(CardUI usedCard)
	{
		if (usedCard == this) return;

		disabled = true;
	}

	public void OnCardDragOrAimingEnded(CardUI _usedCard)
	{
		disabled = false;
		playable = charStats.CanPlayCard(card);
	}

}
