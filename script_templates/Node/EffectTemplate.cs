namespace DeckBuilder;

using Godot;
using Godot.Collections;

public partial class EffectTemplate : Effect
{
    
    public int memberVariable;

    public override void Execute(Array<Node> _targets)
    {
        base.Execute(_targets);
        GD.Print("Sample template for an effect");
    }

}