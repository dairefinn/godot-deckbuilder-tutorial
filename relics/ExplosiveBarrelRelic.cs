namespace DeckBuilder;

using Godot;
using Godot.Collections;

public partial class ExplosiveBarrelRelic : Relic
{

    [Export] public int damage = 2;

    public override void ActivateRelic(RelicUI owner)
    {
        Array<Node> enemies = owner.GetTree().GetNodesInGroup("enemies");
        DamageEffect damageEffect = new()
        {
            amount = damage,
            receiverModifierType = Modifier.Type.NO_MODIFIER
        };
        damageEffect.Execute(enemies);

        owner.Flash();
    }

}
