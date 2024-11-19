namespace DeckBuilder;

using Godot;
using Godot.Collections;

public partial class WarriorBlock : Card
{

    public int baseBlock = 5;

    public override void ApplyEffects(Array<Node> targets, ModifierHandler modifiers)
    {
        BlockEffect blockEffect = new()
        {
            amount = baseBlock,
            sound = sound
        };
        blockEffect.Execute(targets);
    }

    public override string GetDefaultTooltip()
    {
        return string.Format(tooltipText, baseBlock);
    }

    public override string GetUpdatedTooltip(ModifierHandler playerModifiers, ModifierHandler enemyModifiers)
    {
        return string.Format(tooltipText, baseBlock);
    }

}
