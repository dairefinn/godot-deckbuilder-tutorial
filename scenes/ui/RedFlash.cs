namespace DeckBuilder;

using Godot;

public partial class RedFlash : CanvasLayer
{
	
	public ColorRect colorRect;
	public Timer timer;

	public override void _Ready()
	{
		colorRect = GetNode<ColorRect>("ColorRect");
		timer = GetNode<Timer>("Timer");

		Events.Instance.PlayerHit += OnPlayerHit;
		timer.Timeout += OnTimerTimeout;
	}

	public void OnPlayerHit()
	{
		SetRectAlpha(0.2f);
		if (!IsInstanceValid(timer)) return;
		timer.Start();
	}

	public void OnTimerTimeout()
	{
		SetRectAlpha(0);
	}

	private void SetRectAlpha(float alpha)
	{
		if (!IsInstanceValid(colorRect)) return;
		Color tempColor = colorRect.Color;
		tempColor.A = alpha;
		colorRect.Color = tempColor;
	}

}
