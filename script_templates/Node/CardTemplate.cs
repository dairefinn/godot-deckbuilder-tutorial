// meta-name: Card Logic
// meta-description: What happens when a card is played

namespace DeckBuilder;

using Godot;
using Godot.Collections;


public partial class CardTemplate : Card
{

    public override void ApplyEffects(Array<Node> targets, ModifierHandler modifiers)
    {
        GD.Print("My card has been played");
        GD.Print("Targets: " + string.Join(", ", targets));
    }

    public override string GetDefaultTooltip()
    {
        return tooltipText;
    }

    public override string GetUpdatedTooltip(ModifierHandler playerModifiers, ModifierHandler enemyModifiers)
    {
        return tooltipText;
    }

}
