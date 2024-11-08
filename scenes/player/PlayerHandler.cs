namespace DeckBuilder;

using System.Linq;
using Godot;

public partial class PlayerHandler : Node
{

	const float HAND_DRAW_INTERVAL = 0.25f;
	const float HAND_DISCARD_INTERVAL = 0.25f;

	[Export] public Hand hand;

	public CharacterStats character;

	public override void _Ready()
	{
		Events.Instance.CardPlayed += OnCardPlayed;
	}

	public void StartBattle(CharacterStats stats)
	{
		character = stats;
		character.drawPile = character.deck.Duplicate(true) as CardPile;
		character.drawPile.Shuffle();
		character.discard = new CardPile();
		StartTurn();
	}

	public void StartTurn()
	{
		character.block = 0;
		character.ResetMana();
		DrawCards(character.cardsPerTurn);
	}

	public void EndTurn()
	{
		hand.DisableHand();
		DiscardCards();
	}

	public void DiscardCards()
	{
		if (!IsInstanceValid(this))	return;

		Tween tween = CreateTween();
		foreach (CardUI cardUI in hand.GetChildren().Cast<CardUI>())
		{
			if (!IsInstanceValid(cardUI)) continue;
			tween.TweenCallback(Callable.From(() => character.discard.AddCard(cardUI.card)));
			tween.TweenCallback(Callable.From(() => hand.DiscardCard(cardUI)));
			tween.TweenInterval(HAND_DISCARD_INTERVAL);
		}

		tween.Finished += () => {
			Events.Instance.EmitSignal(Events.SignalName.PlayerHandDiscarded);
		};
	}

	public void DrawCards(int cardsAmount)
	{
		Tween tween = CreateTween();
		for (int i = 0; i < cardsAmount; i++)
		{
			tween.TweenCallback(Callable.From(DrawCard));
			tween.TweenInterval(HAND_DRAW_INTERVAL);
		}

		tween.Finished += () => {
			Events.Instance.EmitSignal(Events.SignalName.PlayerHandDrawn);
		};
	}

	public void DrawCard()
	{
		ReshuffleDeckFromDiscard();
		hand.AddCard(character.drawPile.DrawCard());
		ReshuffleDeckFromDiscard();
	}

	public void ReshuffleDeckFromDiscard()
	{
		if (character.drawPile.Empty()) return;

		while (!character.discard.Empty())
		{
			character.drawPile.AddCard(character.discard.DrawCard());
		}

		character.drawPile.Shuffle();
	}

	public void OnCardPlayed(Card card)
	{
		character.discard.AddCard(card);
	}
}
