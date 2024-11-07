namespace DeckBuilder;

using Godot;

public partial class Shaker : Node
{

    public static Shaker Instance;

	public Shaker()
	{
		Instance = this;
	}

    public void Shake(Node2D node, float strength, float duration = 0.2f)
    {
        if (node == null) return;

        Vector2 originalPosition = node.Position;
        int shakeCount = 10;
        Tween tween = CreateTween();

        for (int i = 0; i < shakeCount; i++)
        {
            Vector2 shakeOffset = new((float)GD.RandRange(-1.0, 1.0), (float)GD.RandRange(-1.0, 1.0));
            Vector2 target = originalPosition + strength * shakeOffset;
            if (i % 2 == 0)
            {
                target = originalPosition;
            }

            tween.TweenProperty(node, "position", target, duration / shakeCount);
            strength *= 0.75f;
        }

        tween.Finished += () => {
            node.Position = originalPosition;
        };
    }

}
