namespace DeckBuilder;

using Godot;

[GlobalClass]
public partial class EnemyAction : Node
{

    public enum Type {
        CONDITIONAL,
        CHANCE_BASED
    }

    [Export] public Intent intent;
    [Export] public Type type;
    [Export(PropertyHint.Range, "0,100,")] public float chanceWeight = 0.0f;
    [Export] public AudioStream sound;

    public float accumulatedWeight = 0.0f;

    public Enemy enemy;
    public Node2D target;

    public override void _Ready()
    {

    }

    public virtual bool IsPerformable()
    {
        return true;
    }

    public virtual void PerformAction()
    {
        if (!IsPerformable()) return;
    }

}