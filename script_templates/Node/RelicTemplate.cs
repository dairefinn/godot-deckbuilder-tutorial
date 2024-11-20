namespace DeckBuilder;

using Godot;

public partial class RelicTemplate : Relic
{

    public override void InitializeRelic(RelicUI owner)
    {
        GD.Print("This happens once when we gain a new relic");
    }

    public override void ActivateRelic(RelicUI owner)
    {
        GD.Print("This happens at specific times based on the Relic.Type property");
    }

    // This method should be implemented by event-based relics which connect to the Event bus
    // to make sure that they are disconnected when a relic gets removed.
    public override void DeactivateRelic(RelicUI owner)
    {
        GD.Print("This gets called when a RelicUI is exiting the SceneTree i.e. getting deleted");
        GD.Print("Event-based relics should disconnect from the Event bus here");
    }

    public override string GetTooltip()
    {
        return tooltip;
    }

}