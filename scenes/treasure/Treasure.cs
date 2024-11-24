namespace DeckBuilder;

using System.Linq;
using Godot;
using Godot.Collections;


public partial class Treasure : Control
{

    [Export] public Array<Relic> treasureRelicPool = new();
    [Export] public RelicHandler relicHandler;
    [Export] public CharacterStats charStats;

    public AnimationPlayer animationPlayer;
    public TextureRect treasureChest;

    public Relic foundRelic;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");
        treasureChest = GetNode<TextureRect>("TreasureChest");

        treasureChest.GuiInput += OnTreasureChestGuiInput;
    }

    public void GenerateRelic()
    {
        Array<Relic> availableRelics = new(treasureRelicPool.Where(relic => {
            bool canAppear = relic.CanAppearAsReward(charStats);
            bool alreadyHadIt = relicHandler.HasRelic(relic.id);
            return canAppear && !alreadyHadIt;
        }));

        if (availableRelics.Count == 0)
        {
            foundRelic = null;
            return;
        }

        foundRelic = RNG.ArrayPickRandom(availableRelics);
    }

    // Called from the AnimationPlayer at the end of the 'open' animation
    public void OnTreasureOpened()
    {
        Events.Instance.EmitSignal(Events.SignalName.TreasureRoomExited, foundRelic);
    }

    public void OnTreasureChestGuiInput(InputEvent @event)
    {
        if (animationPlayer.CurrentAnimation == "open") return;

        if (@event.IsActionPressed("left_mouse"))
        {
            animationPlayer.Play("open");
        }
    }

}
