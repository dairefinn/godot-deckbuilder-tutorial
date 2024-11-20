namespace DeckBuilder;

using Godot;
using Godot.Collections;

public partial class ReinforcedArmourRelic : Relic
{

    [Export] public int blockBonus = 3;

    public override void ActivateRelic(RelicUI owner)
    {
        Array<Node> player = owner.GetTree().GetNodesInGroup("player");
        BlockEffect blockEffect = new()
        {
            amount = blockBonus
        };
        blockEffect.Execute(player);

        owner.Flash();
    }

}
