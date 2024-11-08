namespace DeckBuilder;

using System.Collections.Generic;
using Godot;

public partial class BlockEffect : Effect
{

    public int amount = 0;

    public override void Execute(List<Node> _targets)
    {
        foreach (Node target in _targets)
        {
            if (target == null) continue;

            if (target is Enemy e)
            {
                e.stats.block += amount;
                SoundPlayer.TryPlayOnInstance("SFXPlayer", sound, true);
                continue;
            }

            if (target is Player p)
            {
                p.stats.block += amount;
                SoundPlayer.TryPlayOnInstance("SFXPlayer", sound, true);                
                continue;
            }
        }
    }

}
