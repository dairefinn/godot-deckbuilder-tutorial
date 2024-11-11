// meta-name: Card Logic
// meta-description: What happens when a card is played

namespace DeckBuilder;

using Godot;
using Godot.Collections;


public partial class CardTemplate : Card
{

    public override void ApplyEffects(Array<Node> targets)
    {
        GD.Print("My card has been played");
        GD.Print("Targets: " + string.Join(", ", targets));
    }

}
