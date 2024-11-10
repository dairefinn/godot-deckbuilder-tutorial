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
        _cardPile = value;

        if (!_cardPile.IsConnected(CardPile.SignalName.CardPileSizeChanged, new Callable(this, MethodName.OnCardPileSizeChanged)))
        {
            _cardPile.CardPileSizeChanged += OnCardPileSizeChanged;
            OnCardPileSizeChanged(_cardPile.cards.Length);
        }
    }

    public void OnCardPileSizeChanged(int cardsAmount)
    {
        counter.Text = cardsAmount.ToString();
    }

}
