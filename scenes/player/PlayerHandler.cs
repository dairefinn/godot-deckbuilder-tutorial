namespace DeckBuilder;

using Godot;

public partial class PlayerHandler : Node
{

	const float HAND_DRAW_INTERVAL = 0.25f;
	const float HAND_DISCARD_INTERVAL = 0.25f;

	[Export] public Player player;
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
		player.statusHandler.StatusesApplied += OnStatusesApplied;
		StartTurn();
	}

	public void StartTurn()
	{
		character.block = 0;
		character.ResetMana();
		player.statusHandler.ApplyStatusesByType(Status.Type.START_OF_TURN);
	}

	public void EndTurn()
	{
		hand.DisableHand();
		player.statusHandler.ApplyStatusesByType(Status.Type.END_OF_TURN);
	}

	public void DiscardCards()
	{
		if (!IsInstanceValid(this))	return;

		if (hand.GetChildCount() == 0)
		{
			Events.Instance.EmitSignal(Events.SignalName.PlayerHandDiscarded);
			return;
		}

		Tween tween = CreateTween();
		foreach (Node cardUINode in hand.GetChildren())
		{
			if (cardUINode is not CardUI cardUI) continue;
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
		if (!IsInstanceValid(this)) return;

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
		if (card.exhausts || card.type == Card.Type.POWER) return;

		character.discard.AddCard(card);
	}

	public void OnStatusesApplied(Status.Type type)
	{
		switch (type)
		{
			case Status.Type.START_OF_TURN:
				DrawCards(character.cardsPerTurn);
				break;
			case Status.Type.END_OF_TURN:
				DiscardCards();
				break;
		}
	}

}
