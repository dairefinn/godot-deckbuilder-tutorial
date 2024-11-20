namespace DeckBuilder;

using Godot;

public partial class ManaPotionRelic : Relic
{

    public override void ActivateRelic(RelicUI owner)
    {
        void Handler()
        {
            AddMana(owner);
            Events.Instance.PlayerHandDrawn -= Handler;
        }

        Events.Instance.PlayerHandDrawn += Handler;
    }

    public void AddMana(RelicUI owner)
    {
        owner.Flash();
        Player player = owner.GetTree().GetFirstNodeInGroup("player") as Player;
        if (player != null)
        {
            player.stats.mana += 1;
        }
    }

}
