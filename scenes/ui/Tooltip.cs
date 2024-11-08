namespace  DeckBuilder;

using Godot;

public partial class Tooltip : PanelContainer
{

	[Export] public float fadeSeconds = 0.2f;

	public TextureRect tooltipIcon;
	public RichTextLabel tooltipTextLabel;

	private Tween tween;
	private bool isVisible = false;

	public override void _Ready()
	{
		tooltipIcon = GetNode<TextureRect>("%TooltipIcon");
		tooltipTextLabel = GetNode<RichTextLabel>("%TooltipText");

		Modulate = Colors.Transparent;
		Hide();

		Events.Instance.CardTooltipRequested += ShowTooltip;
		Events.Instance.TooltipHideRequested += HideTooltip;
	}

	public void ShowTooltip(Texture2D icon, string text)
	{
		if (!IsInstanceValid(this)) return;
		if (!IsInstanceValid(tooltipIcon)) return;
		if (!IsInstanceValid(tooltipTextLabel)) return;

		isVisible = true;
		tween?.Kill();

		tooltipIcon.Texture = icon;
		tooltipTextLabel.Text = text;

		tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
		tween.TweenCallback(Callable.From(Show));
		tween.TweenProperty(this, "modulate", Colors.White, fadeSeconds);
	}

	public void HideTooltip()
	{
		if (!IsInstanceValid(this)) return;
		isVisible = false;
		tween?.Kill();

		// HideAnimation();
		GetTree().CreateTimer(fadeSeconds, false).Timeout += HideAnimation;
	}

	public void HideAnimation()
	{
		if (isVisible) return;

		tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
		tween.TweenProperty(this, "modulate", Colors.Transparent, fadeSeconds);
		tween.TweenCallback(Callable.From(Hide));
	}

	private void show()
	{
		Show();
	}

	private void hide()
	{
		Hide();
	}

}
