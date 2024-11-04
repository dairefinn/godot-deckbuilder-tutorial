namespace DeckBuilder;

using System.Collections.Generic;
using Godot;

public partial class DamageEffect : Effect
{

    public int amount = 0;

    public override void Execute(List<Node> _targets)
    {
        foreach (Node target in _targets)
        {
            if (target == null) continue;

            if (target is Enemy e)
            {
                e.TakeDamage(amount);
                continue;
            }

            if (target is Player p)
            {
                p.TakeDamage(amount);
                continue;
            }
        }
    }

}
