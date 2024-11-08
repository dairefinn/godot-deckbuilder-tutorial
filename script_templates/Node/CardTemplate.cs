// meta-name: Card Logic
// meta-description: What happens when a card is played

namespace DeckBuilder;

using System;
using System.Collections.Generic;
using Godot;

public partial class CardTemplate : Card
{

    public override void ApplyEffects(List<Node> targets)
    {
        GD.Print("My card has been played");
        GD.Print("Targets: " + string.Join(", ", targets));
    }

}
