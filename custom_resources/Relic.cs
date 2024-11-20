namespace DeckBuilder;

using Godot;

[GlobalClass]
public partial class Relic : Resource
{

    public enum Type
    {
        START_OF_TURN,
        START_OF_COMBAT,
        END_OF_TURN,
        END_OF_COMBAT,
        EVENT_BASED
    }

    public enum CharacterType
    {
        ALL,
        ASSASSIN,
        WARRIOR,
        WIZARD
    }

    [Export] public string relicName;
    [Export] public string id;
    [Export] public Type type;
    [Export] public CharacterType characterType;
    [Export] public bool starterRelic;
    [Export] public Texture2D icon;
    [Export(PropertyHint.MultilineText)] public string tooltip;

    public virtual void InitializeRelic(RelicUI owner)
    {
    }

    public virtual void ActivateRelic(RelicUI owner)
    {
    }

    // This method should be implemented by event-based relics which connect to the Event bus
    // to make sure that they are disconnected when a relic gets removed.
    public virtual void DeactivateRelic(RelicUI owner)
    {
    }

    public virtual string GetTooltip()
    {
        return tooltip;
    }

    public bool CanAppearAsReward(CharacterStats character)
    {
        if (starterRelic) return false;

        if (characterType != CharacterType.ALL)
        {
            string relicCharacterName = characterType.ToString().ToLower();
            string characterName = character.characterName.ToLower();
            if (relicCharacterName != characterName) return false;
        }

        return true;
    }

}
