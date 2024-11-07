namespace DeckBuilder;

using Godot;

[GlobalClass]
public partial class CharacterStats : Stats
{

    [Export] public CardPile startingDeck;
    [Export] public int cardsPerTurn;
    [Export] public int maxMana;

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
        return mana > card.cost;
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
