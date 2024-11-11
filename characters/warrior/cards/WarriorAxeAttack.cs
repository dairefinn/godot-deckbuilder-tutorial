namespace DeckBuilder;

using Godot;
using Godot.Collections;


public partial class WarriorAxeAttack : Card
{

    public override void ApplyEffects(Array<Node> targets)
    {
        DamageEffect damageEffect = new()
        {
            amount = 6
        };
        damageEffect.sound = sound;
        damageEffect.Execute(targets);
    }

}
