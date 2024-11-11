namespace DeckBuilder;

using Godot;
using Godot.Collections;

public partial class DamageEffect : Effect
{

    public int amount = 0;

    public override void Execute(Array<Node> _targets)
    {
        foreach (Node target in _targets)
        {
            if (target == null) continue;

            if (target is Enemy e)
            {
                e.TakeDamage(amount);
                SoundPlayer.TryPlayOnInstance("SFXPlayer", sound, true);
                continue;
            }

            if (target is Player p)
            {
                p.TakeDamage(amount);
                SoundPlayer.TryPlayOnInstance("SFXPlayer", sound, true);
                continue;
            }
        }
    }

}
