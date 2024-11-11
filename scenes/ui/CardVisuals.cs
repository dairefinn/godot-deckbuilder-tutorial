namespace DeckBuilder;

using Godot;

public partial class CardVisuals : Control
{

    [Export] public Card card {
        get => _card;
        set => SetCard(value);
    }
    private Card _card;

    public Panel panel;
    public Label cost;
    public TextureRect icon;
    public TextureRect rarity;

    public override void _Ready()
    {
        panel = GetNode<Panel>("Panel");
        cost = GetNode<Label>("Cost");
        icon = GetNode<TextureRect>("Icon");
        rarity = GetNode<TextureRect>("Rarity");
    }

    public async void SetCard(Card card)
    {
        if (!IsNodeReady()) {
            await ToSignal(this, "ready");
        }

        _card = card;

        if (!IsInstanceValid(cost)) return;
        if (!IsInstanceValid(icon)) return;
        if (!IsInstanceValid(rarity)) return;

        cost.Text = card.cost.ToString();
        icon.Texture = card.icon;
        rarity.Modulate = Card.RARITY_COLORS[card.rarity];
    }

}
