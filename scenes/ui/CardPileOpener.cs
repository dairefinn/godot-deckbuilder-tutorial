namespace DeckBuilder;

using Godot;

public partial class CardPileOpener : TextureButton
{

    [Export] public Label counter;
    [Export] public CardPile cardPile {
        get => _cardPile;
        set => SetCardPile(value);
    }
    private CardPile _cardPile;

    public void SetCardPile(CardPile value)
    {
        if (!IsInstanceValid(this)) return;

        _cardPile = value;

        if (!_cardPile.IsConnected(CardPile.SignalName.CardPileSizeChanged, new Callable(this, MethodName.OnCardPileSizeChanged)))
        {
            _cardPile.CardPileSizeChanged += OnCardPileSizeChanged;
            OnCardPileSizeChanged(_cardPile.cards.Count);
        }
    }

    public void OnCardPileSizeChanged(int cardsAmount)
    {
        if (!IsInstanceValid(counter)) return;
        counter.Text = cardsAmount.ToString();
    }

}
