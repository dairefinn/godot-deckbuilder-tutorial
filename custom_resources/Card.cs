using Godot;

[GlobalClass]
public partial class Card : Resource
{

    public enum Type {
        ATTACK = 0,
        DEFEND = 1,
        POWER = 2
    }

    public enum Target {
        SELF = 0,
        SINGLE_ENEMY = 1,
        ALL_ENEMIES = 2,
        EVERYONE = 3
    }

    [ExportGroup("Card attributes")]
    [Export] public string id;
    [Export] public Type type;
    [Export] public Target target;

    public bool GetIsSingleTargeted() {
        return target == Target.SINGLE_ENEMY;
    }

}
