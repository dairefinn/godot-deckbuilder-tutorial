namespace DeckBuilder;

using System;
using Godot;

public partial class Hand : HBoxContainer
{

	private Events globalEvents;

	public int cardsPlayedThisTurn = 0;
	
	public override void _Ready()
	{
		globalEvents = GetNode<Events>("/root/Events");

		globalEvents.CardPlayed += OnCardPlayed;

		foreach (Node child in GetChildren())
		{
			if (child is CardUI cardUI)
			{
				cardUI.parent = this;
				cardUI.ReparentRequested += OnReparentRequested;
			}
		}
	}

	private void OnReparentRequested(CardUI child)
	{
		child.Reparent(this);
		int newIndex = Math.Clamp(child.originalIndex - cardsPlayedThisTurn, 0, GetChildCount());
		CallDeferred(MethodName.MoveChild, child, newIndex);
	}

	public void OnCardPlayed(Card _card)
	{
		cardsPlayedThisTurn++;
	}

}
