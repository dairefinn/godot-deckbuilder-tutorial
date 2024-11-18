namespace DeckBuilder;

using Godot;
using Godot.Collections;

public partial class WarriorBlock : Card
{

    public override void ApplyEffects(Array<Node> targets, ModifierHandler modifiers)
    {
        BlockEffect blockEffect = new()
        {
            amount = 5,
            sound = sound
        };
        blockEffect.Execute(targets);
    }

}
