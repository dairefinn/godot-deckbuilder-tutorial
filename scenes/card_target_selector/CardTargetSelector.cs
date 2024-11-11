namespace DeckBuilder;

using System.Linq;
using Godot;
using Godot.Collections;


public partial class CardTargetSelector : Node2D
{

	const int ARC_POINTS = 8;

	public Area2D area2D;
	public Line2D cardArc;

	public CardUI currentCard;
	public bool targeting = false;

    public override void _Ready()
    {
		area2D = GetNode<Area2D>("Area2D");
		cardArc = GetNode<Line2D>("CanvasLayer/CardArc");
		
		Events.Instance.CardAimStarted += OnCardAimStarted;
		Events.Instance.CardAimEnded += OnCardAimEnded;

		area2D.AreaEntered += OnArea2DEntered;
		area2D.AreaExited += OnArea2DExited;
    }

    public override void _Process(double delta)
    {
        if (!targeting) return;
		
		if (!IsInstanceValid(area2D)) return;
		if (!IsInstanceValid(cardArc)) return;

		area2D.Position = GetLocalMousePosition();
		cardArc.Points = GetPoints();
    }

    private void OnCardAimStarted(CardUI cardUI)
	{
		if (!cardUI.card.GetIsSingleTargeted()) return;
		
		if (!IsInstanceValid(area2D)) return;

		targeting = true;
		area2D.Monitoring = true;
		area2D.Monitorable = true;
		currentCard = cardUI;
	}

	private void OnCardAimEnded(CardUI cardUI)
	{
		if (!IsInstanceValid(area2D)) return;
		if (!IsInstanceValid(cardArc)) return;

		targeting = false;
		cardArc.ClearPoints();
		area2D.Position = Vector2.Zero;
		area2D.Monitoring = false;
		area2D.Monitorable = false;
		currentCard = null;
	}

	private void OnArea2DEntered(Area2D area)
	{
		if (!targeting) return;
		if (currentCard == null) return;

		if (!currentCard.targets.Contains(area))
		{
			currentCard.targets.Add(area);
		}
	}

	private void OnArea2DExited(Area2D area)
	{
		if (!targeting) return;
		if (currentCard == null) return;

		if (currentCard.targets.Contains(area))
		{
			currentCard.targets.Remove(area);
		}
	}

	private Vector2[] GetPoints()
	{
		Array<Vector2> points = new();
		Vector2 start = currentCard.GlobalPosition;
		start.X += currentCard.Size.X / 2;
		Vector2 target = GetLocalMousePosition();
		Vector2 distance = target - start;

		for (int i = 0; i < ARC_POINTS; i++)
		{
			float t = (1.0f / ARC_POINTS) * i;
			float x = start.X + (distance.X / ARC_POINTS) * i;
			float y = start.Y + EaseOutCubic(t) * distance.Y;
			points.Add(new Vector2(x, y));
		}

		points.Add(target);

		return points.ToArray();
	}

	private float EaseOutCubic(float x)
	{
		return 1.0f - Mathf.Pow(1.0f - x, 3.0f);
	}

}
