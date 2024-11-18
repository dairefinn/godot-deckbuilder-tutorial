namespace DeckBuilder;

using Godot;
using Godot.Collections;


public partial class WarriorAxeAttack : Card
{

    public int baseDamage = 6;

    public override void ApplyEffects(Array<Node> targets, ModifierHandler modifiers)
    {
        DamageEffect damageEffect = new()
        {
            amount = modifiers.GetModifiedValue(baseDamage, Modifier.Type.DMG_DEALT),
            sound = sound
        };
        damageEffect.Execute(targets);
    }

}
