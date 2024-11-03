namespace DeckBuilder;

using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class CardPile : Resource
{

    [Signal] public delegate void CardPileSizeChangedEventHandler(int cardsAmount);

    [Export] public Card[] cards = Array.Empty<Card>();

    public bool Empty()
    {
        return cards.Length == 0;
    }

    public Card DrawCard()
    {
        Card card = cards[0];
        cards = cards[1..]; // Remove the first card
        EmitSignal(CardPile.SignalName.CardPileSizeChanged, cards.Length);
        return card;
    }

    public void AddCard(Card card)
    {
        List<Card> cardsNew = new();
        cardsNew.AddRange(cards);
        cardsNew.Add(card);
        cards = cardsNew.ToArray();
        EmitSignal(CardPile.SignalName.CardPileSizeChanged, cards.Length);
    }

    public void Clear()
    {
        cards = Array.Empty<Card>();
        EmitSignal(CardPile.SignalName.CardPileSizeChanged, cards.Length);
    }

    public override string ToString()
    {
        List<string> cardStrings = new();
        for (int i = 0; i < cards.Length; i++)
        {
            Card card = cards[i];
            cardStrings[i] = $"{i + 1}. {card.id}";
        }

        return string.Join("\n", cardStrings);
    }

}
