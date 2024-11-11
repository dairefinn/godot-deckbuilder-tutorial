using Godot;

namespace DeckBuilder;

public partial class CardBaseState : CardState
{

    public override async void Enter()
	{
		if (!cardUI.IsNodeReady())
		{
			await ToSignal(cardUI, "ready");
		}

		if (cardUI.tween != null && cardUI.tween.IsRunning())
		{
			cardUI.tween.Kill();
		}

		cardUI.CardVisuals.panel.Set("theme_override_styles/panel", CardUI.BASE_STYLEBOX);
		cardUI.EmitSignal(CardUI.SignalName.ReparentRequested, cardUI);
		cardUI.PivotOffset = Vector2.Zero;

		Events.Instance.EmitSignal(Events.SignalName.TooltipHideRequested);
	}

	public override void OnGuiInput(InputEvent @event)
	{
		if (!cardUI.playable) return;
		if (cardUI.disabled) return;

		if (@event.IsActionPressed("left_mouse"))
		{
			cardUI.PivotOffset = cardUI.GetGlobalMousePosition() - cardUI.GlobalPosition;
			EmitSignal(SignalName.TransitionRequested, this, (int)CardState.State.CLICKED);
		}
	}

    public override void OnMouseEntered()
    {
		if (!cardUI.playable) return;
		if (cardUI.disabled) return;

		cardUI.CardVisuals.panel.Set("theme_override_styles/panel", CardUI.HOVER_STYLEBOX);
		Events.Instance.EmitSignal(Events.SignalName.CardTooltipRequested, cardUI.card.icon, cardUI.card.tooltipText);
    }

	public override void OnMouseExited()
	{
		if (!cardUI.playable) return;
		if (cardUI.disabled) return;

		cardUI.CardVisuals.Set("theme_override_styles/panel", CardUI.BASE_STYLEBOX);
		Events.Instance.EmitSignal(Events.SignalName.TooltipHideRequested);
	}

}
