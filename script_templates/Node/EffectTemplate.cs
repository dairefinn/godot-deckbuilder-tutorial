namespace DeckBuilder;

using System.Collections.Generic;
using Godot;

public partial class EffectTemplate : Effect
{
    
    public int memberVariable;

    public override void Execute(List<Node> _targets)
    {
        base.Execute(_targets);
        GD.Print("Sample template for an effect");
    }

}