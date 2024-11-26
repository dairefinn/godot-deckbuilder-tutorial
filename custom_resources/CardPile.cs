namespace DeckBuilder;

using Godot;
using Godot.Collections;


[GlobalClass]
public partial class CardPile : Resource
{

    [Signal] public delegate void CardPileSizeChangedEventHandler(int cardsAmount);

    [Export] public Array<Card> cards = new();

    public bool Empty()
    {
        return cards.Count == 0;
    }

    public Card DrawCard()
    {
        Card card = cards[0];
        cards = cards[1..]; // Remove the first card
        EmitSignal(CardPile.SignalName.CardPileSizeChanged, cards.Count);
        return card;
    }

    public void AddCard(Card card)
    {
        Array<Card> cardsNew = new();
        cardsNew.AddRange(cards);
        cardsNew.Add(card);
        cards = cardsNew;
        EmitSignal(CardPile.SignalName.CardPileSizeChanged, cards.Count);
    }

    public void Clear()
    {
        cards = new();
        EmitSignal(CardPile.SignalName.CardPileSizeChanged, cards.Count);
    }

    public void Shuffle()
    {
        RNG.ArrayShuffle(cards);
    }

    // We need this method because of a Godot issue reported here: https://github.com/godotengine/godot/issues/74918
    public Array<Card> DuplicateCards()
    {
        Array<Card> newArray = new();

        foreach (Card card in cards)
        {
            newArray.Add(card.Duplicate() as Card);
        }

        return newArray;
    }

    // We need this method because of a Godot issue reported here: https://github.com/godotengine/godot/issues/74918
    public CardPile CustomDuplicate()
    {
        CardPile newCardPile = new()
        {
            cards = DuplicateCards()
        };

        return newCardPile;
    }

    public override string ToString()
    {
        Array<string> cardStrings = new();
        for (int i = 0; i < cards.Count; i++)
        {
            Card card = cards[i];
            cardStrings.Add($"{i + 1}. {card.id}");
        }

        return string.Join("\n", cardStrings);
    }

}
