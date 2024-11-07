namespace DeckBuilder;

using System;
using System.Linq;
using Godot;

public partial class Hand : HBoxContainer
{

	[Export] public CharacterStats charStats;

	private PackedScene cardUIResource = ResourceLoader.Load<PackedScene>("res://scenes/card_ui/card_ui.tscn");

	public void AddCard(Card card)
	{
		CardUI newCardUI = cardUIResource.Instantiate() as CardUI;
		AddChild(newCardUI);
		newCardUI.ReparentRequested += OnReparentRequested;
		newCardUI.card = card;
		newCardUI.parent = this;
		newCardUI.charStats = charStats;
	}

	public void DiscardCard(CardUI card)
	{
		card.QueueFree();
	}

	public void DisableHand()
	{
		foreach (CardUI card in GetChildren().Cast<CardUI>())
		{
			card.disabled = true;
		}
	}

	private void OnReparentRequested(CardUI child)
	{
		child.disabled = true;
		child.Reparent(this);
		int newIndex = Math.Clamp(child.originalIndex, 0, GetChildCount());
		CallDeferred(MethodName.MoveChild, child, newIndex);
		CallDeferred(MethodName.EnableCard, child);
	}

	private static void EnableCard(CardUI card)
	{
		card.disabled = false;
	}

}
