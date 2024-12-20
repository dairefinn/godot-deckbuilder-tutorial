namespace DeckBuilder;

using Godot;

[GlobalClass]
public partial class CharacterStats : Stats
{

    [ExportGroup("Visuals")]
    [Export] public string characterName;
    [Export(PropertyHint.MultilineText)] public string description;
    [Export] public Texture2D portrait;

    [ExportGroup("Gameplay Data")]
    [Export] public CardPile startingDeck;
    [Export] public CardPile draftableCards;
    [Export] public int cardsPerTurn;
    [Export] public int maxMana;
    [Export] public Relic startingRelic;

    public int mana {
        get => _mana;
        set => SetMana(value);
    }
    public int _mana;

    public CardPile deck;
    public CardPile discard;
    public CardPile drawPile;

    public void SetMana(int value)
    {
        _mana = value;
        EmitSignal(CharacterStats.SignalName.StatsChanged);
    }

    public void ResetMana()
    {
        mana = maxMana;
    }

    public bool CanPlayCard(Card card)
    {
        return mana >= card.cost;
    }

    public override void TakeDamage(int damage)
    {
        int initialHealth = health;
        base.TakeDamage(damage);
        if (initialHealth > health)
        {
            Events.Instance.EmitSignal(Events.SignalName.PlayerHit);
        }
    }

    public new CharacterStats CreateInstance()
    {
        CharacterStats instance = Duplicate() as CharacterStats;
        instance.health = maxHealth;
        instance.block = 0;
        instance.ResetMana();
        instance.deck = startingDeck.Duplicate() as CardPile;
        instance.discard = new();
        instance.drawPile = new();
        return instance;
    }

}
