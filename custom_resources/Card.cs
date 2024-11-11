namespace DeckBuilder;

using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Card : Resource
{

    public enum Type {
        ATTACK = 0,
        DEFEND = 1,
        POWER = 2
    }

    public enum Rarity {
        COMMON = 0,
        UNCOMMON = 1,
        RARE = 2,
    }

    public enum Target {
        SELF = 0,
        SINGLE_ENEMY = 1,
        ALL_ENEMIES = 2,
        EVERYONE = 3
    }

    public static readonly Dictionary<Rarity, Color> RARITY_COLORS = new()
    {
        {Rarity.COMMON, Colors.Gray},
        {Rarity.UNCOMMON, Colors.CornflowerBlue},
        {Rarity.RARE, Colors.Gold}
    };

    [ExportGroup("Card attributes")]
    [Export] public string id;
    [Export] public Type type;
    [Export] public Rarity rarity;
    [Export] public Target target;
    [Export] public int cost;
    [Export] public AudioStream sound;

    [ExportGroup("Card visuals")]
    [Export] public Texture2D icon;
    [Export(PropertyHint.MultilineText)] public string tooltipText;

    public bool GetIsSingleTargeted() {
        return target == Target.SINGLE_ENEMY;
    }

    public Array<Node> GetTargets(Array<Node> targets)
    {
        if (targets.Count == 0) return new Array<Node>();

        SceneTree tree = targets[0].GetTree();
        
        switch(target)
        {
            case Target.SELF:
                return tree.GetNodesInGroup("player");
            case Target.ALL_ENEMIES:
                return tree.GetNodesInGroup("enemies");
            case Target.EVERYONE:
                return tree.GetNodesInGroup("player") + tree.GetNodesInGroup("enemies");
            case Target.SINGLE_ENEMY:
            default:
                GD.PrintErr("Invalid target type");
                return new Array<Node>();
        }
    }

    public void Play(Array<Node> targets, CharacterStats characterStats)
    {
        Events.Instance.EmitSignal(Events.SignalName.CardPlayed, this);
        characterStats.mana -= cost;

        if (GetIsSingleTargeted())
        {
           ApplyEffects(targets);
        }
        else
        {
            ApplyEffects(GetTargets(targets));
        }
    }

    public virtual void ApplyEffects(Array<Node> targets)
    {
    }

}
