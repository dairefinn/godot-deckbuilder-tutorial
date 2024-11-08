namespace DeckBuilder;

using System;
using System.Collections.Generic;
using System.Linq;
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
    [Export] public int cost;
    [Export] public AudioStream sound;

    [ExportGroup("Card visuals")]
    [Export] public Texture2D icon;
    [Export(PropertyHint.MultilineText)] public string tooltipText;

    public bool GetIsSingleTargeted() {
        return target == Target.SINGLE_ENEMY;
    }

    public List<Node> GetTargets(List<Node> targets)
    {
        if (targets.Count == 0) return new List<Node>();

        SceneTree tree = targets[0].GetTree();
        
        switch(target)
        {
            case Target.SELF:
                return tree.GetNodesInGroup("player").ToList();
            case Target.ALL_ENEMIES:
                return tree.GetNodesInGroup("enemies").ToList();
            case Target.EVERYONE:
                return (tree.GetNodesInGroup("player") + tree.GetNodesInGroup("enemies")).ToList();
            case Target.SINGLE_ENEMY:
            default:
                GD.PrintErr("Invalid target type");
                return new List<Node>();
        }
    }

    public void Play(List<Node> targets, CharacterStats characterStats)
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

    public virtual void ApplyEffects(List<Node> targets)
    {
    }

}
