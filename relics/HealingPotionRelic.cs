namespace DeckBuilder;

using Godot;

public partial class HealingPotionRelic : Relic
{

    [Export] public int healAmount = 6;

    public override void ActivateRelic(RelicUI owner)
    {
        Player player = owner.GetTree().GetFirstNodeInGroup("player") as Player;
        if (player != null)
        {
            player.stats.Heal(healAmount);
            owner.Flash();
        }
    }

}
