namespace DeckBuilder;

using System.Collections.Generic;
using Godot;

public partial class WarriorBlock : Card
{

    public override void ApplyEffects(List<Node> targets)
    {
        BlockEffect blockEffect = new()
        {
            amount = 5
        };
        blockEffect.Execute(targets);
    }

}
