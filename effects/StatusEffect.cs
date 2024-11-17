namespace DeckBuilder;

using Godot;
using Godot.Collections;

public partial class StatusEffect : Effect
{

    public Status status;

    public override void Execute(Array<Node> _targets)
    {
        foreach (Node target in _targets)
        {
            if (target is null) continue;
            
            if (target is Enemy enemy)
            {
                enemy.statusHandler.AddStatus(status);
            }

            if (target is Player player)
            {
                player.statusHandler.AddStatus(status);
            }
        }
    }

}
