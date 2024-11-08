namespace DeckBuilder;

using System.Collections.Generic;
using Godot;

public partial class WarriorAxeAttack : Card
{

    public override void ApplyEffects(List<Node> targets)
    {
        DamageEffect damageEffect = new()
        {
            amount = 6
        };
        damageEffect.sound = sound;
        damageEffect.Execute(targets);
    }

}
