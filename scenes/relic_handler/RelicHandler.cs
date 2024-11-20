namespace DeckBuilder;

using System;
using System.Linq;
using Godot;
using Godot.Collections;


public partial class RelicHandler : HBoxContainer
{

    [Signal] public delegate void RelicsActivatedEventHandler(Relic.Type type);

    public static readonly float RELIC_APPLY_INTERVAL = 0.5f;
    public static readonly PackedScene RELIC_UI = GD.Load<PackedScene>("res://scenes/relic_handler/relic_ui.tscn");

    public RelicsControl relicsControl;
    public HBoxContainer relics;

    public override void _Ready()
    {
        relicsControl = GetNode<RelicsControl>("RelicsControl");
        relics = GetNode<HBoxContainer>("%Relics");

        relics.ChildExitingTree += OnRelicsChildExitingTree;
    }

    public void ActivateRelicsByType(Relic.Type type)
    {
        if (type == Relic.Type.EVENT_BASED) return;

        Array<RelicUI> relicQueue = new(GetAllRelicUINodes().ToList().Where(relicUI => relicUI.relic.type == type).ToArray());

        if (relicQueue.Count == 0)
        {
            EmitSignal(SignalName.RelicsActivated, (int)type);
            return;
        }

        Tween tween = CreateTween();
        foreach (RelicUI relicUI in relicQueue)
        {
            tween.TweenCallback(Callable.From(() => relicUI.relic.ActivateRelic(relicUI)));
            tween.TweenInterval(RELIC_APPLY_INTERVAL);
        }

        tween.Finished += () => {
            EmitSignal(SignalName.RelicsActivated, (int)type);
        };
    }

    public void AddRelics(Array<Relic> relicsArray)
    {
        foreach (Relic relic in relicsArray)
        {
            AddRelic(relic);
        }
    }

    public void AddRelic(Relic relic)
    {
        if (HasRelic(relic.id)) return;

        RelicUI newRelicUI = RELIC_UI.Instantiate<RelicUI>();
        relics.AddChild(newRelicUI);
        newRelicUI.relic = relic;
        newRelicUI.relic.InitializeRelic(newRelicUI);
    }

    public bool HasRelic(string id)
    {
        foreach (Node relicNode in relics.GetChildren())
        {
            if (relicNode is not RelicUI relicUI) continue;
            if (!IsInstanceValid(relicUI)) continue;
            if (relicUI.relic.id != id) continue;
            return true;
        }

        return false;
    }

    public Array<Relic> GetAllRelics()
    {
        Array<RelicUI> relicUINodes = GetAllRelicUINodes();
        Array<Relic> relicsArray = new();

        foreach (RelicUI relicUI in relicUINodes)
        {
            relicsArray.Add(relicUI.relic);
        }

        return relicsArray;
    }

    public Array<RelicUI> GetAllRelicUINodes()
    {
        Array<RelicUI> allRelics = new();

        foreach (Node relicNode in relics.GetChildren())
        {
            if (relicNode is not RelicUI relicUI) continue;
            allRelics.Add(relicUI);
        }

        return allRelics;
    }

    public void OnRelicsChildExitingTree(Node relicUINode)
    {
        if (relicUINode == null) return;
        if (relicUINode is not RelicUI relicUI) return;

        if (relicUI.relic != null)
        {
            relicUI.relic.DeactivateRelic(relicUI);
        }
    }

}
