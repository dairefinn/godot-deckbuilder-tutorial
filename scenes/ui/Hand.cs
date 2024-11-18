namespace DeckBuilder;

using Godot;

public partial class Hand : HBoxContainer
{

	[Export] public Player player;
	[Export] public CharacterStats charStats;

	private PackedScene cardUIResource = GD.Load<PackedScene>("res://scenes/card_ui/card_ui.tscn");

	public void AddCard(Card card)
	{
		CardUI newCardUI = cardUIResource.Instantiate() as CardUI;
		AddChild(newCardUI);
		newCardUI.ReparentRequested += OnReparentRequested;
		newCardUI.card = card;
		newCardUI.parent = this;
		newCardUI.charStats = charStats;
		newCardUI.playerModifiers = player.modifierHandler;
	}

	public void DiscardCard(CardUI card)
	{
		card.QueueFree();
	}

	public void DisableHand()
	{
		if (!IsInstanceValid(this)) return;

		foreach (Node cardNode in GetChildren())
		{
			if (cardNode is not CardUI card) continue;
			if (!IsInstanceValid(card)) continue;
			card.disabled = true;
		}
	}

	private void OnReparentRequested(CardUI child)
	{
		child.disabled = true;
		child.Reparent(this);
		int newIndex = Mathf.Clamp(child.originalIndex, 0, GetChildCount());
		CallDeferred(MethodName.MoveChild, child, newIndex);
		CallDeferred(MethodName.EnableCard, child);
	}

	private static void EnableCard(CardUI card)
	{
		card.disabled = false;
	}

}
