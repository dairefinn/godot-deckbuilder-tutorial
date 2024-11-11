namespace DeckBuilder;

using Godot;
using Godot.Collections;

public partial class WarriorBlock : Card
{

    public override void ApplyEffects(Array<Node> targets)
    {
        BlockEffect blockEffect = new()
        {
            amount = 5
        };
        blockEffect.sound = sound;
        blockEffect.Execute(targets);
    }

}
